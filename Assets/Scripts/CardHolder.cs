using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public GameObject cardPrefab;
    public Card previewCard;
    public GameManager gameManager;
    Transform cardHand;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHand(List<CardData> hand)
    {
        foreach (Transform child in transform.GetChild(0)) Destroy(child.gameObject);
        foreach (CardData cd in hand) AddCard(cd);
    }

    void AddCard(CardData cd)
    {
        GameObject go = Instantiate(cardPrefab,Vector3.zero,Quaternion.identity,transform.GetChild(0));
        Card c = go.GetComponent<Card>();
        c.holder = this;
        c.UpdateInfo(cd);
    }

    public void MouseDownOnCard(Card card)
    {
        gameManager.MouseDownOnCard(card);
    }

    public void StartPreview(Card card)
    {
        previewCard.gameObject.SetActive(true);
        previewCard.UpdateInfo(card.cd);
    }

    public void EndPreview()
    {
        previewCard.gameObject.SetActive(false);
    }
}
