using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardHolder holder;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    #endregion
}
