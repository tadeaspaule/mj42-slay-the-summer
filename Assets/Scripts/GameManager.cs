using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform enemyHolder;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UseCard(Card card)
    {

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
