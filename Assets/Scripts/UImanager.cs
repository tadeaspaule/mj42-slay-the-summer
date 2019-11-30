using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UImanager : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI deckText;
    public TextMeshProUGUI discardText;

    public GameObject map;
    public GameObject combatUI;

    public GameObject endOfGameScreen;
    public TextMeshProUGUI endOfGameTitle;
    public TextMeshProUGUI gameOverStats;

    public GameObject cardPrefab;
    public GameObject cardShowcasOuter;
    public Transform cardShowcaseHolder;

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
    }

    public void ToggleMap()
    {
        map.SetActive(!map.activeInHierarchy);
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

    }

    public void NewGame()
    {
        endOfGameScreen.SetActive(false);
        EndCombat();
        map.SetActive(true);
    }
}
