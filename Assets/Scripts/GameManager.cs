using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform enemyHolder;
    public LineRenderer targetLine;

    Card usingCard;
    Character target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            targetLine.enabled = false;
            if (usingCard != null && target != null) UseCard();
            usingCard = null;
            target = null;
        }
        if (usingCard != null) {
            targetLine.SetPosition(1,Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void UseCard()
    {

    }

    public void MouseDownOnCard(Card card)
    {
        targetLine.enabled = true;
        usingCard = card;
        // targetLine.SetPosition(0,card.transform.position);
    }

    public void EnteredCharacter(Character character)
    {
        if (!character.e.friendly) target = character;
    }

    public void LeftCharacter()
    {
        target = null;
    }

    float space = 2.4f;

    public void SpawnEnemies()
    {
        foreach (Transform child in enemyHolder) Destroy(child.gameObject);
        int n = Random.Range(1,4);
        List<GameObject> enemies = new List<GameObject>();
        for (int i = 0; i < n; i++) enemies.Add(Instantiate(characterPrefab,Vector3.zero,Quaternion.identity,enemyHolder));
        foreach (GameObject go in enemies) go.transform.localPosition = Vector3.zero;
        if (n == 2) {
            enemies[0].transform.position -= new Vector3(space*0.5f,0f,0f);
            enemies[1].transform.position += new Vector3(space*0.5f,0f,0f);
        }
        else if (n == 3) {
            enemies[0].transform.position -= new Vector3(space,0f,0f);
            enemies[2].transform.position += new Vector3(space,0f,0f);
        }
    }
}
