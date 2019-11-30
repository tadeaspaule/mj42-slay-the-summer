using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    public GameManager gameManager;
    public UImanager uImanager;
    public Entity e;

    public TextMeshProUGUI healthText;
    public Image moveIndicator;
    public TextMeshProUGUI armorText;
    public GameObject armorHolder;

    int nextStep = -1;
    const int ATTACK = 0;
    const int ARMOR = 1;
    const int DEBUFF_ATTACK = 2;
    const int DEBUFF_ARMOR = 3;
    int nextAttack = 3;
    int nextArmor = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        nextStep = Random.Range(0,2);
        if (nextStep == ATTACK) moveIndicator.sprite = uImanager.attackIcon;
        else if (nextStep == ARMOR) moveIndicator.sprite = uImanager.armorIcon;
        else if (nextStep == DEBUFF_ATTACK) moveIndicator.sprite = uImanager.attackDebuffIcon;
        else if (nextStep == DEBUFF_ARMOR) moveIndicator.sprite = uImanager.armorDebuffIcon;
        else Debug.Log($"no change {nextStep}");
    }

    public void ExecuteMove()
    {
        if (nextStep == ATTACK) gameManager.player.TakeDamage(nextAttack);
        else if (nextStep == ARMOR) e.armor += nextArmor;
        else if (nextStep == DEBUFF_ATTACK) gameManager.player.TakeDamage(nextAttack);
        else if (nextStep == DEBUFF_ARMOR) gameManager.player.TakeDamage(nextAttack);
    }

    #endregion
}
