using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject mapNodePrefab;
    public MapNode currentNode = null;
    public Sprite shopSprite;
    float shopChance = 0.3f;
    
    public void SetupMap()
    {
        List<MapNode> nodes = new List<MapNode>();
        MapNode final = transform.GetChild(0).GetComponent<MapNode>();
        final.enemies.Clear();
        Enemy finalBoss = Enemy.GetBoss();
        final.enemies.Add(finalBoss);
        foreach (Transform child in transform)
            if (!child.GetComponent<MapNode>().Equals(final)) Destroy(child.gameObject);
        nodes.Add(final);
        int from = 0;
        int lastN = 1;
        for (int i = 0; i < 4; i++) {
            int n = Random.Range(1,4);
            List<GameObject> gos = new List<GameObject>();
            for (int ii = 0; ii < n; ii++) gos.Add(Instantiate(mapNodePrefab,Vector3.zero,Quaternion.identity,transform));
            float y = -45 - 205*i + 295f;
            if (n == 1) gos[0].transform.localPosition = new Vector3(0f,y,0f);
            else if (n == 2) {
                gos[0].transform.localPosition = new Vector3(-150f,y,0f);
                gos[1].transform.localPosition = new Vector3(150f,y,0f);
            }
            else if (n == 3) {
                gos[0].transform.localPosition = new Vector3(-210f,y,0f);
                gos[1].transform.localPosition = new Vector3(0f,y,0f);
                gos[2].transform.localPosition = new Vector3(210f,y,0f);
            }
            foreach (GameObject go in gos) {
                MapNode mn = go.GetComponent<MapNode>();
                nodes.Add(mn);
                mn.SetupConnections(nodes.GetRange(from,lastN));
                if (Random.Range(0f,1f) <= shopChance) {
                    mn.isShopkeeper = true;
                    mn.GetComponent<Image>().sprite = shopSprite;
                }
                else {
                    int enem = Random.Range(1,4);
                    for (int iii = 0; iii < enem; iii++) {
                        mn.enemies.Add(gameManager.allEnemies[Random.Range(0,gameManager.allEnemies.Count)].GetClone());
                    }
                }                
            }
            from += lastN;
            lastN = n;
        }
    }

    public void MapNodeClicked(MapNode node)
    {
        if (gameManager.inCombat) return;
        if (currentNode != null && !currentNode.connections.Contains(node)) return;
        if (currentNode != null) currentNode.DisableCurrentIndicator();
        currentNode = node;
        currentNode.EnableCurrentIndicator();
        Debug.Log($"Moved to {node.gameObject.name}");
        gameManager.EnterNode(node);
        this.gameObject.SetActive(false);
    }
}
