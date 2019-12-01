using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public List<MapNode> connections = new List<MapNode>();
    Map map;

    void Start()
    {
        map = FindObjectOfType<Map>();
    }

    public void NodeClicked()
    {
        map.MapNodeClicked(this);
    }

    public void EnableCurrentIndicator()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DisableCurrentIndicator()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetupConnections(List<MapNode> connections)
    {
        this.connections = connections;
        Vector3 pos = Camera.main.ScreenToWorldPoint(transform.position);
        for (int i = 0; i < connections.Count; i++) {
            LineRenderer lr = transform.GetChild(i+1).GetComponent<LineRenderer>();
            lr.SetPosition(0,transform.position);
            lr.SetPosition(1,connections[i].transform.position);
        }
    }
}
