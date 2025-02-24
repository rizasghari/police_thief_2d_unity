using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Node> nodes = new List<Node>();
    [SerializeField] private Node thiefStartNode;
    [SerializeField] private GameObject plicePrefab;
    [SerializeField] private GameObject thiefPrefab;
    private Graph graph = new Graph();
    void Start()
    {
        if (nodes.Count > 0)
        {
            foreach (var node in nodes)
            {
                graph.AddNode(node);
                foreach (var neighbor in node.Neighbors)
                {
                    graph.AddConnection(node, neighbor);
                }
            }
        }

        SpawnThief();
        SpawnPolices();
    }

    private void SpawnThief()
    {
        var spawnPosition = thiefStartNode.transform.position;
        spawnPosition.y += thiefPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
        nodes.Find(node => node == thiefStartNode).SetIsOccupied(true);
    }

    private void SpawnPolices()
    {
        var exitNods = nodes.FindAll(node => node.IsExit);
        foreach (var node in exitNods)
        {
            var spawnPosition = node.transform.position;
            spawnPosition.y += plicePrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            var spawnedPolice =Instantiate(plicePrefab, spawnPosition, Quaternion.identity);
            Police police = spawnedPolice.GetComponent<Police>();
            police.Initialize(this, node);
            node.SetIsOccupied(true);
        }
    }

    public bool GetNodeOnDropPosition(Vector3 dropPosition, out Node landedNode) {
        foreach (var node in nodes)
        {
            var collider = node.GetComponent<CircleCollider2D>();
            if (collider.bounds.Contains(dropPosition) && !node.IsOccupied)
            {
                landedNode = node; 
                node.SetIsOccupied(true);
                return true; 
            }
        }
        landedNode = null;
        return false;
    }
}
