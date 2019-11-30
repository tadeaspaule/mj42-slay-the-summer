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
    TextMeshProUGUI armorText;
    GameObject armorHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!gameObject.name.Equals("player")) e = new Entity();
        healthText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        moveIndicator = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        armorText = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        armorHolder = transform.GetChild(0).GetChild(3).gameObject;
    }

    void Update()
    {
        healthText.text = $"{e.health}/{e.maxHealth}";
        armorHolder.SetActive(e.armor > 0);
        armorText.text = e.armor.ToString();
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
