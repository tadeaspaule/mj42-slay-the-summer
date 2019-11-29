using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform enemyHolder;
    public LineRenderer targetLine;
    public Character playerChar;
    public Entity player;

    Card usingCard;
    Character target;

    public TextAsset cardsJson;
    Dictionary<string,CardData> cards = new Dictionary<string, CardData>();

    int handSize = 4;
    List<CardData> deck = new List<CardData>();
    List<CardData> hand = new List<CardData>();
    List<CardData> discardPile = new List<CardData>();
    public CardHolder cardHolder;
    
    #region Unity methods
    
    // Start is called before the first frame update
    void Start()
    {
        player = new Entity();
        player.friendly = true;
        playerChar.e = player;

        List<CardData> allCards = Helper.readJsonArray<CardData>(cardsJson.ToString());
        foreach (CardData cd in allCards) {
            cards.Add(cd.name,cd);
        }
        SetupStartDeck();
        DrawCards();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            targetLine.enabled = false;
            if (usingCard != null && target != null) UseCard();
            usingCard = null;
            target = null;
        }
        if (usingCard != null) {
            targetLine.SetPosition(1,Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    #endregion
    
    #region Mouse events

    public void MouseDownOnCard(Card card)
    {
        targetLine.enabled = true;
        usingCard = card;
        // targetLine.SetPosition(0,card.transform.position);
    }

    public void EnteredCharacter(Character character)
    {
        if (!character.e.friendly) target = character;
    }

    public void LeftCharacter()
    {
        target = null;
    }

    #endregion

    #region Card stuff
    
    void UseCard()
    {

    }

    void SetupStartDeck()
    {
        deck.Clear();
        for (int i = 0; i < 5; i++) deck.Add(cards["Attack"]);
        for (int i = 0; i < 5; i++) deck.Add(cards["Defend"]);
        for (int i = 0; i < 2; i++) deck.Add(cards["Draw cards"]);
    }

    void DrawCards()
    {
        discardPile.AddRange(hand);
        hand.Clear();
        if (handSize > deck.Count) {
            int over = handSize - deck.Count;
            hand.AddRange(deck);
            deck.AddRange(discardPile);
            discardPile.Clear();
            for (int i = 0; i < over; i++) DrawRandomCard();
        }
        else for (int i = 0; i < handSize; i++) DrawRandomCard();
        cardHolder.UpdateHand(hand);
    }

    void DrawRandomCard()
    {
        int i = Random.Range(0,deck.Count);
        hand.Add(deck[i]);
        deck.RemoveAt(i);
    }

    #endregion
    
    #region Combat

    public void ClickedEndTurn()
    {
        EnemyActs();
        DrawCards();
    }

    void EnemyActs()
    {
        Debug.Log("AI did something");
    }
    
    float space = 2.4f;

    public void SpawnEnemies()
    {
        foreach (Transform child in enemyHolder) Destroy(child.gameObject);
        int n = Random.Range(1,4);
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < n; i++) enemies.Add(Instantiate(characterPrefab,Vector3.zero,Quaternion.identity,enemyHolder));
        foreach (GameObject go in enemies) go.transform.localPosition = Vector3.zero;
        if (n == 2) {
            enemies[0].transform.position -= new Vector3(space*0.5f,0f,0f);
            enemies[1].transform.position += new Vector3(space*0.5f,0f,0f);
        }
        else if (n == 3) {
            enemies[0].transform.position -= new Vector3(space,0f,0f);
            enemies[2].transform.position += new Vector3(space,0f,0f);
        }
    }

    #endregion
}
