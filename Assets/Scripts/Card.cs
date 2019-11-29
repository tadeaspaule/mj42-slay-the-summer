using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardHolder holder;
    public CardData cd;
    
    // Start is called before the first frame update
    void Start()
    {
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

    public void MouseClicked()
    {
        // holder.UseCard(this);
        // Destroy(this.gameObject);
    }

    #endregion
}
