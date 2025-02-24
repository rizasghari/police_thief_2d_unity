using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    private Dictionary<Node, List<Node>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<Node, List<Node>>();
    }

    public void AddNode(Node node)
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<Node>();
        }
    }

    public void AddConnection(Node source, Node destination)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            AddNode(source);
        }
        if (!adjacencyList.ContainsKey(destination))
        {
            AddNode(destination);
        }

        if (!adjacencyList[source].Contains(destination))
        {
            adjacencyList[source].Add(destination);
        }
    }

    public List<Node> GetNeighbors(Node vertex)
    {
        if (adjacencyList.ContainsKey(vertex))
        {
            return adjacencyList[vertex];
        }
        return new List<Node>();
    }

    public void PrintGraph()
    {
        foreach (var node in adjacencyList)
        {
            Debug.Log(node.Key + ": ");
            foreach (var neighbor in node.Value)
            {
                Debug.Log(neighbor + " ");
            }
            Console.WriteLine();
        }
    }
}

