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

    public GameObject gameOverScreen;
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
        gameOverScreen.SetActive(true);
    }

    public void NewGame()
    {
        gameOverScreen.SetActive(false);
        EndCombat();
        map.SetActive(true);
    }
}
