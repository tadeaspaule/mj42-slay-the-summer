using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public UImanager uImanager;

    public GameObject characterPrefab;
    public Transform enemyHolder;
    public LineRenderer targetLine;
    public Character playerChar;
    public Entity player;
    public int gold;
    public int mana = 3;
    public int maxMana = 3;

    Card usingCard;
    Character target;

    public TextAsset cardsJson;
    List<CardData> notBaseCards = new List<CardData>();
    Dictionary<string,CardData> cards = new Dictionary<string, CardData>();
    
    public TextAsset enemiesJson;
    public List<Enemy> allEnemies;

    int handSize = 4;
    public List<CardData> deck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();
    public CardHolder cardHolder;

    List<Entity> currentEnemies = new List<Entity>();

    public bool inCombat = false;
    bool waitingForAI = false;

    // stat stuff
    public int cardsUsed = 0;
    public int armorGained = 0;
    public int damageDealt = 0;
    public int cardsDrawn = 0;

    public Map map;

    public Character shopkeeper;
    public Transform shopkeeperPrices;
    public Transform shopkeeperSelection;
    
    #region Unity methods
    
    // Start is called before the first frame update
    void Start()
    {
        List<CardData> allCards = Helper.readJsonArray<CardData>(cardsJson.ToString());
        allEnemies = Helper.readJsonArray<Enemy>(enemiesJson.ToString());
        foreach (CardData cd in allCards) {
            cards.Add(cd.name,cd);
            if (!cd.baseCard) notBaseCards.Add(cd);
        }
        ResetGame();
        playerChar.gameManager = this;
        playerChar.uImanager = uImanager;
        playerChar.ResetAnimations();
        shopkeeper.e = new Entity();
        shopkeeper.e.name = "shopkeeper";
        shopkeeper.ResetAnimations();
    }

    void ResetGame()
    {
        waitingForAI = false;
        inCombat = false;
        player = new Entity();
        player.friendly = true;
        player.name = "player";
        playerChar.e = player;
        SetupStartDeck();
        map.SetupMap();
        cardsUsed = 0;
        armorGained = 0;
        damageDealt = 0;
        cardsDrawn = 0;
        gold = 300;
        uImanager.UpdateUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            targetLine.enabled = false;
            if (usingCard != null && target != null) UseCard();
        }
        if (usingCard != null) {
            targetLine.SetPosition(1,Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    #endregion
    
    #region Mouse events

    public void MouseDownOnCard(Card card)
    {
        if (card.cd.cost > mana) return;
        targetLine.enabled = true;
        usingCard = card;
        // targetLine.SetPosition(0,card.transform.position);
    }

    public void EnteredCharacter(Character character)
    {
        if (usingCard == null || !character.targettable) return;
        if (character.e.friendly == usingCard.cd.IsSelfCard()) target = character;
    }

    public void LeftCharacter()
    {
        target = null;
    }

    #endregion

    #region Card stuff
    
    void UseCard()
    {
        cardsUsed++;
        armorGained += usingCard.cd.armorSelf;
        damageDealt += usingCard.cd.UseCard(player,target,currentEnemies);
        if (usingCard.cd.damageAll > 0) {
            foreach (Transform child in enemyHolder) {
                child.GetComponent<Character>().PlayHurtAnim();
            }
        }
        else if (usingCard.cd.damageTarget > 0) target.PlayHurtAnim();
        playerChar.PlayAttackAnim();
        mana -= usingCard.cd.cost;
        discardPile.Add(usingCard.cd);
        for (int i = 0; i < usingCard.cd.cardDraw; i++) {
            cardHolder.AddCard(DrawRandomCard());
        }
        Destroy(usingCard.gameObject);
        usingCard = null;
        target = null;
        uImanager.UpdateUI();
        CheckDeaths();
    }

    void CheckDeaths()
    {
        if (player.health == 0) {
            GameOver();
            return;
        }
        int count = enemyHolder.childCount;
        foreach (Transform enemyChar in enemyHolder) {
            Character c = enemyChar.GetComponent<Character>();
            if (c.e.health == 0) {
                StartCoroutine(DelayedDeath(c,c.PlayDeathAnim()));
                currentEnemies.Remove(c.e);
                count --;
            }
        }
        if (count == 0) {
            EndCombat();
            if (map.currentNode.connections.Count == 0) GameWon();
            else GenerateRewards();
        }
    }

    IEnumerator DelayedDeath(Character character, float delay)
    {
        character.targettable = false;
        yield return new WaitForSeconds(delay);
        try {
            Destroy(character.gameObject);
        }
        catch {}
    }

    void SetupStartDeck()
    {
        deck.Clear();
        discardPile.Clear();
        hand.Clear();
        for (int i = 0; i < 5; i++) deck.Add(cards["Ice Shuriken"]);
        for (int i = 0; i < 5; i++) deck.Add(cards["Carrot Snack"]);
        for (int i = 0; i < 2; i++) deck.Add(cards["Snowball Stockpile"]);
    }

    void StartPlayerTurn()
    {
        waitingForAI = false;
        // draws cards
        discardPile.AddRange(hand);
        hand.Clear();
        for (int i = 0; i < handSize; i++) DrawRandomCard();
        // replenishes mana
        mana = maxMana;
        // update UI        
        cardHolder.UpdateHand();
        uImanager.UpdateUI();
        foreach (Transform child in enemyHolder) {
            child.GetComponent<Character>().DecideMove();
        }
    }

    CardData DrawRandomCard()
    {
        cardsDrawn++;
        if (deck.Count == 0) {
            deck.AddRange(discardPile);
            discardPile.Clear();
        }
        int i = Random.Range(0,deck.Count);
        CardData cd = deck[i];
        hand.Add(cd);
        deck.RemoveAt(i);
        return cd;
    }

    #endregion
    
    #region Combat

    public void ClickedEndTurn()
    {
        if (waitingForAI) return;
        EnemyActs();
    }

    void EnemyActs()
    {
        waitingForAI = true;
        discardPile.AddRange(hand);
        hand.Clear();
        cardHolder.UpdateHand();
        StartCoroutine(ExecuteEnemyMoves(0));
    }

    IEnumerator ExecuteEnemyMoves(int index)
    {
        enemyHolder.GetChild(index).GetComponent<Character>().ExecuteMove();
        uImanager.UpdateUI();
        yield return new WaitForSeconds(0.5f);
        CheckDeaths();
        if (player.health > 0 && enemyHolder.childCount > index+1)
            StartCoroutine(ExecuteEnemyMoves(index+1));
        else if (player.health > 0) StartPlayerTurn();
    }
    
    float space = 2.8f;

    public void SpawnEnemies(List<Enemy> enemyEntities)
    {
        foreach (Transform child in enemyHolder) Destroy(child.gameObject);
        int n = enemyEntities.Count;
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < n; i++) enemies.Add(Instantiate(characterPrefab,Vector3.zero,Quaternion.identity,enemyHolder));
        foreach (GameObject go in enemies) {
            go.transform.localPosition = Vector3.zero;
            Character c = go.GetComponent<Character>();
            c.gameManager = this;
            c.uImanager = uImanager;
        }
        if (n == 2) {
            enemies[0].transform.position -= new Vector3(space*0.5f,0f,0f);
            enemies[1].transform.position += new Vector3(space*0.5f,0f,0f);
        }
        else if (n == 3) {
            enemies[0].transform.position -= new Vector3(space,0f,0f);
            enemies[2].transform.position += new Vector3(space,0f,0f);
        }
        else if (n == 1 && enemyEntities[0].name == "boss") {
            enemies[0].transform.localScale = new Vector3(2f,2f,2f);
        }
        for (int i = 0; i < n; i++) {
            Character c = enemies[i].GetComponent<Character>();
            enemyEntities[i].SetupCharacter(c);
            c.ResetAnimations();
            currentEnemies.Add(c.e);
        }
    }

    public void EnterNode(MapNode node)
    {
        if (node.isShopkeeper) GenerateShop();
        else StartCombat(node);
    }

    void StartCombat(MapNode node)
    {
        ToggleShop(false);
        currentEnemies.Clear();
        SpawnEnemies(node.enemies);
        inCombat = true;
        mana = maxMana;
        deck.AddRange(discardPile);
        deck.AddRange(hand);
        discardPile.Clear();
        hand.Clear();
        for (int i = 0; i < handSize; i++) DrawRandomCard();
        cardHolder.UpdateHand();
        uImanager.StartCombat();
        uImanager.UpdateUI();
        StartPlayerTurn();
    }

    public void EndCombat()
    {
        inCombat = false;
        uImanager.EndCombat();
    }

    #endregion

    #region Shopkeeper

    int cost;
    int minCost = 50;

    void GenerateShop()
    {
        ToggleShop(true);
        cost = gold / 2;
        if (cost < minCost) cost = minCost;
        for (int i = 0; i < 3; i++) {
            shopkeeperSelection.GetChild(i).gameObject.SetActive(true);
            shopkeeperSelection.GetChild(i).GetComponent<Card>().UpdateInfo(notBaseCards[Random.Range(0,notBaseCards.Count)],false);
            shopkeeperPrices.GetChild(i).gameObject.SetActive(true);
            shopkeeperPrices.GetChild(i).GetComponent<TextMeshProUGUI>().text = $"{cost} gold";
        }
    }

    void ToggleShop(bool active)
    {        
        shopkeeperPrices.gameObject.SetActive(active);
        shopkeeperSelection.gameObject.SetActive(active);
        shopkeeper.gameObject.SetActive(active);
    }

    public void PurchaseCard(Card card)
    {
        if (cost > gold) return;
        gold -= cost;
        deck.Add(card.cd);
        card.gameObject.SetActive(false);
        shopkeeperPrices.GetChild(int.Parse(card.gameObject.name)).gameObject.SetActive(false);
        uImanager.UpdateUI();
    }

    #endregion
    
    #region Battle Rewards

    public GameObject rewardsOuter;
    int goldReward;
    public TextMeshProUGUI goldText;
    public GameObject goldRewardObject;
    public GameObject cardRewardObject;
    public GameObject cardRewardSelection;

    void GenerateRewards()
    {
        rewardsOuter.SetActive(true);
        goldRewardObject.SetActive(true);
        cardRewardObject.SetActive(true);
        cardRewardSelection.SetActive(false);
        foreach (Transform c in cardRewardSelection.transform) {
            c.GetComponent<Card>().UpdateInfo(notBaseCards[Random.Range(0,notBaseCards.Count)],false);
        }
        goldReward = Random.Range(20,40);
        goldText.text = $"{goldReward} gold";
    }

    public void ClickedSkipRewards()
    {
        rewardsOuter.SetActive(false);
        uImanager.OpenMap();
    }

    public void ClickedGoldReward()
    {
        goldRewardObject.SetActive(false);
        gold += goldReward;
        uImanager.UpdateUI();
        uImanager.PlayCoinSound();
    }

    public void ClickedCardRewards()
    {
        cardRewardSelection.SetActive(true);
    }

    public void SelectedCardReward(Card card)
    {
        cardRewardSelection.SetActive(false);
        cardRewardObject.SetActive(false);
        deck.Add(card.cd);
    } 

    #endregion
    
    #region End of game

    void GameOver()
    {
        uImanager.GameOver();
    }
    
    void GameWon()
    {
        uImanager.GameWon();
    }

    public void ClickedNewGame()
    {
        foreach (Transform child in enemyHolder) Destroy(child.gameObject);
        ResetGame();
        uImanager.NewGame();
    }

    public void ClickedMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}
