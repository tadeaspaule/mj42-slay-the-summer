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
