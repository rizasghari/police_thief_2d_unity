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
        spawnPosition.z = 0;
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
            spawnPosition.z = 0;
            var spawnedPolice = Instantiate(plicePrefab, spawnPosition, Quaternion.identity);
            Police police = spawnedPolice.GetComponent<Police>();
            police.Initialize(this, node);
            node.SetIsOccupied(true);
        }
    }

    public bool GetNodeOnDropPosition(Node currentNode, Collider2D policeCollider, out Node landedNode, Vector2 dropPosition)
    {
        foreach (var node in nodes)
        {
            if (node.TryGetComponent<Collider2D>(out var collider))
            {
                if (!node.IsOccupied && collider.IsTouching(policeCollider) && CheckAreNeighbor(currentNode, node))
                {
                    landedNode = node;
                    node.SetIsOccupied(true);
                    return true;
                }
            }
        }
        landedNode = null;
        return false;
    }

    private bool CheckAreNeighbor(Node origin, Node destination)
    {
        return graph.GetNeighbors(origin).Contains(destination);
    }

    public void SetNodesAvailabilityIndicator(Node currentNode, bool isDragging)
    {
        foreach (var node in nodes)
        {
            node.SetNodesAvailabilityIndicator(show: isDragging, isAvailable: CheckAreNeighbor(currentNode, node) && !node.IsOccupied);

        }
    }
}