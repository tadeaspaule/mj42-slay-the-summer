using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public CardHolder holder;
    public CardData cd;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void UpdateInfo(CardData cd)
    {
        this.cd = cd;
        nameText.text = cd.name;
        costText.text = cd.cost.ToString();
        descriptionText.text = cd.GetCardText();
    }

    #region Mouse Interaction

    public void MouseEntered()
    {
        holder.StartPreview(this);
    }

    public void MouseLeft()
    {
        holder.EndPreview();        
    }

    public void MouseDown()
    {
        holder.MouseDownOnCard(this);
    }

    #endregion
}
