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

    public SpriteRenderer charImage;
    public TextMeshProUGUI healthText;
    public Image moveIndicator;
    public TextMeshProUGUI armorText;
    public GameObject armorHolder;

    int nextStep = -1;
    const int ATTACK = 0;
    const int ARMOR = 1;
    const int DEBUFF_ATTACK = 2;
    const int DEBUFF_ARMOR = 3;
    public int nextAttack = 3;
    public int nextArmor = 3;
    public float shieldChance;

    Sprite[] idleAnim;
    Sprite[] attackAnim;
    Sprite[] currentPlayingAnim;
    int animI;
    float animTimer = 0f;
    float animStep = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        healthText.text = $"{e.health}/{e.maxHealth}";
        armorHolder.SetActive(e.armor > 0);
        armorText.text = e.armor.ToString();

        if (currentPlayingAnim != null) {
            animTimer += Time.deltaTime;
            if (animTimer >= animStep) {
                animTimer = 0f;
                animI++;
                if (animI >= currentPlayingAnim.Length) {
                    animI = 0;
                    currentPlayingAnim = idleAnim;
                }
                charImage.sprite = currentPlayingAnim[animI];
            }
        }
    }

    public void ResetAnimations()
    {
        Debug.Log(e.name);
        idleAnim = Resources.LoadAll<Sprite>($"animations/{e.name}idle");
        attackAnim = Resources.LoadAll<Sprite>($"animations/{e.name}attack");
        currentPlayingAnim = idleAnim;
        animI = 0;
        animTimer = 0f;
        if (currentPlayingAnim != null) charImage.sprite = currentPlayingAnim[0];
    }

    public float PlayAttackAnim()
    {
        if (attackAnim == null) return 0f;
        currentPlayingAnim = attackAnim;
        animI = 0;
        animTimer = 0f;
        charImage.sprite = currentPlayingAnim[0];
        return attackAnim.Length*animStep;
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
        nextStep = Random.Range(0f,1f) <= shieldChance ? ARMOR : ATTACK;
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
