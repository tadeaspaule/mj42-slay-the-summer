using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public CardHolder holder;
    public CardData cd;
    bool mouseActive;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    public Image cardImage;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateInfo(CardData cd)
    {
        UpdateInfo(cd,true);
    }

    public void UpdateInfo(CardData cd, bool mouseActive)
    {
        this.mouseActive = mouseActive;
        this.cd = cd;
        nameText.text = cd.name;
        costText.text = cd.cost.ToString();
        descriptionText.text = cd.GetCardText();
        cardImage.sprite = Resources.Load<Sprite>($"Cards/{cd.name}");
    }

    #region Mouse Interaction

    public void MouseEntered()
    {
        if (!mouseActive) return;
        holder.StartPreview(this);
    }

    public void MouseLeft()
    {
        if (!mouseActive) return;
        holder.EndPreview();        
    }

    public void MouseDown()
    {
        if (!mouseActive) return;
        holder.MouseDownOnCard(this);
    }

    #endregion
}
