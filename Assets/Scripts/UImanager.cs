using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour
{
    public AudioSource audioSource;

    public GameManager gameManager;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI deckText;
    public TextMeshProUGUI discardText;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;

    public AudioClip mapSound;
    public GameObject map;
    public GameObject combatUI;

    public GameObject endOfGameScreen;
    public TextMeshProUGUI endOfGameTitle;
    public TextMeshProUGUI gameOverStats;

    public GameObject cardPrefab;
    public GameObject cardShowcasOuter;
    public Transform cardShowcaseHolder;

    public Sprite attackIcon;
    public Sprite armorIcon;
    public Sprite attackDebuffIcon;
    public Sprite armorDebuffIcon;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI drawText;
    
    void OpenCardShowcase(List<CardData> cards)
    {
        foreach (Transform child in cardShowcaseHolder) Destroy(child.gameObject);
        foreach (CardData cd in cards) {
            GameObject go = Instantiate(cardPrefab,Vector3.zero,Quaternion.identity,cardShowcaseHolder);
            go.GetComponent<Card>().UpdateInfo(cd,false);
        }
    }

    public void ToggleDeck()
    {
        if (gameManager.deck.Count == 0) return;
        bool prevActive = cardShowcasOuter.activeInHierarchy;
        cardShowcasOuter.SetActive(!prevActive);
        if (!prevActive) OpenCardShowcase(gameManager.deck);
    }

    public void ToggleDiscard()
    {
        if (gameManager.discardPile.Count == 0) return;
        bool prevActive = cardShowcasOuter.activeInHierarchy;
        cardShowcasOuter.SetActive(!prevActive);
        if (!prevActive) OpenCardShowcase(gameManager.discardPile);
    }

    public void ToggleAll()
    {
        bool prevActive = cardShowcasOuter.activeInHierarchy;
        cardShowcasOuter.SetActive(!prevActive);
        if (!prevActive) {
            List<CardData> all = new List<CardData>();
            all.AddRange(gameManager.hand);
            all.AddRange(gameManager.deck);
            all.AddRange(gameManager.discardPile);
            OpenCardShowcase(all);
        }
    }
    
    public void UpdateUI()
    {
        deckText.text = gameManager.deck.Count.ToString();
        discardText.text = gameManager.discardPile.Count.ToString();
        resourceText.text = $"{gameManager.mana}/{gameManager.maxMana}";
        healthText.text = $"{gameManager.player.health}/{gameManager.player.maxHealth}";
        goldText.text = gameManager.gold.ToString();
    }

    public void ToggleMap()
    {
        audioSource.PlayOneShot(mapSound);
        map.SetActive(!map.activeInHierarchy);
    }

    public void OpenMap()
    {
        audioSource.PlayOneShot(mapSound);
        map.SetActive(true);
    }

    public void StartCombat()
    {
        combatUI.SetActive(true);
    }

    public void EndCombat()
    {
        combatUI.SetActive(false);
    }

    public void GameOver()
    {
        endOfGameScreen.SetActive(true);
        endOfGameTitle.text = "game over";
        SetupGameStats();
    }

    public void GameWon()
    {
        endOfGameScreen.SetActive(true);
        endOfGameTitle.text = "victory";
        SetupGameStats();
    }

    void SetupGameStats()
    {
        int highscore = PlayerPrefs.GetInt("highscore",0);
        if (gameManager.gold > highscore) {
            highscore = gameManager.gold;
            PlayerPrefs.SetInt("highscore",highscore);
        }
        scoreText.text = gameManager.gold.ToString();
        highscoreText.text = highscore.ToString();
        damageText.text = gameManager.damageDealt.ToString();
        armorText.text = gameManager.armorGained.ToString();
        drawText.text = gameManager.cardsDrawn.ToString();
    }

    public void NewGame()
    {
        endOfGameScreen.SetActive(false);
        EndCombat();
        map.SetActive(true);
    }
}
