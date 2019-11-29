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
        cardHand = transform.GetChild(0);
        for (int i = 0; i < 3; i++) AddCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCard()
    {
        GameObject go = Instantiate(cardPrefab,Vector3.zero,Quaternion.identity,cardHand);
        Card c = go.GetComponent<Card>();
        c.holder = this;
    }

    public void UseCard(Card card)
    {
        gameManager.UseCard(card);
        EndPreview();
    }

    public void StartPreview(Card card)
    {
        previewCard.gameObject.SetActive(true);
    }

    public void EndPreview()
    {
        previewCard.gameObject.SetActive(false);
    }
}
