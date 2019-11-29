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
    
    
    public void UpdateUI()
    {
        deckText.text = gameManager.deck.Count.ToString();
        discardText.text = gameManager.discardPile.Count.ToString();
    }
}
