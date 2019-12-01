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

    int handSize = 4;
    public List<CardData> deck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();
    public CardHolder cardHolder;

    public bool inCombat = false;
    bool waitingForAI = false;

    public Map map;
    
    #region Unity methods
    
    // Start is called before the first frame update
    void Start()
    {
        List<CardData> allCards = Helper.readJsonArray<CardData>(cardsJson.ToString());
        foreach (CardData cd in allCards) {
            cards.Add(cd.name,cd);
            if (!cd.baseCard) notBaseCards.Add(cd);
        }
        ResetGame();
        playerChar.gameManager = this;
        playerChar.uImanager = uImanager;
        playerChar.ResetAnimations();
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
        if (usingCard == null) return;
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
        usingCard.cd.UseCard(player,target,map.currentNode.enemies);
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
                Destroy(enemyChar.gameObject);
                map.currentNode.enemies.Remove(c.e);
                count --;
            }
        }
        if (count == 0) {
            EndCombat();
            if (map.currentNode.connections.Count == 0) GameWon();
        }
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
    
    float space = 2.4f;

    public void SpawnEnemies(List<Entity> enemyEntities)
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
        for (int i = 0; i < enemyEntities.Count; i++) {
            enemies[i].GetComponent<Character>().e = enemyEntities[i];
        }
    }

    public void StartCombat(MapNode node)
    {
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
        GenerateRewards();
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
