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

    bool attacking;
    int nextAttack = 3;
    int nextArmor = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
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

    public void DecideMove()
    {
        attacking = Random.Range(0,3) < 2;
        string s = attacking ? "attackIcon" : "shieldIcon";
        moveIndicator.sprite = Resources.Load<Sprite>($"Icons/{s}");
    }

    public void ExecuteMove()
    {
        if (attacking) gameManager.player.TakeDamage(nextAttack);
        else e.armor += nextArmor;
    }

    #endregion
}
