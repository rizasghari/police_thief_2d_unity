using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private bool isExit;
    [SerializeField] private List<Node> neighbors;
    [SerializeField] SpriteRenderer availabilityIndicator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite availableSprite;
    [SerializeField] Sprite unavailableSprite;
    [SerializeField] Sprite normalNodeSprite;
    [SerializeField] Sprite exitNodeSprite;

    private bool isOccupied;

    public bool IsExit => isExit;
    public bool IsOccupied => isOccupied;

    public List<Node> Neighbors => neighbors;

    private void Awake() {
        if (isExit)
        {
            spriteRenderer.sprite = exitNodeSprite;
        }
        else
        {
            spriteRenderer.sprite = normalNodeSprite;
        }
    }

    public void SetIsOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }

    public void SetNodesAvailabilityIndicator(bool show, bool isAvailable)
    {
        if (!show)
        {
            availabilityIndicator.gameObject.SetActive(false);
        }
        else
        {
            availabilityIndicator.gameObject.SetActive(true);
            if (isAvailable)
            {
                availabilityIndicator.sprite = availableSprite;
            }
            else
            {
                availabilityIndicator.sprite = unavailableSprite;
            }
        }
    }
}
