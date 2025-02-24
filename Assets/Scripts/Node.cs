using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private bool isExit;
    [SerializeField] private List<Node> neighbors;
    [SerializeField] SpriteRenderer statusSr;
    private bool isOccupied;

    public bool IsExit => isExit;
    public bool IsOccupied => isOccupied;

    public List<Node> Neighbors => neighbors;

    public void SetIsOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }
}
