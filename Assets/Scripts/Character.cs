using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    GameManager gameManager;
    public Entity e;

    TextMeshProUGUI healthText;
    Image moveIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        e = new Entity();
        healthText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        moveIndicator = transform.GetChild(0).GetChild(1).GetComponent<Image>();
    }

    void Update()
    {
        healthText.text = $"{e.health}/{e.maxHealth}";
    }

    #region Mouse Interaction

    void OnMouseEnter()
    {
        gameManager.EnteredCharacter(this);
    }

    void OnMouseExit()
    {
        gameManager.LeftCharacter();      
    }

    #endregion
}
