using UnityEngine;

public class Police : MonoBehaviour
{
    private Vector3 mousePositionOffset;
    private Node currentNode;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    public void Initialize(GameManager gameManager, Node startNode)
    {
        this.currentNode = startNode;
        this.gameManager = gameManager;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (!gameManager.IsPoliceTurn) return;

        mousePositionOffset = GetMouseWorldPosition() - transform.position;
        gameManager.SetNodesAvailabilityIndicator(currentNode, true);
    }

    void OnMouseDrag()
    {
        if (!gameManager.IsPoliceTurn) return;

        transform.position = GetMouseWorldPosition() - mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (!gameManager.IsPoliceTurn) return;

        gameManager.SetNodesAvailabilityIndicator(currentNode, false);

        Vector3 dropPosition = GetMouseWorldPosition() - mousePositionOffset;
        if (gameManager.GetNodeOnDropPosition(currentNode, GetComponent<Collider2D>(), out Node landedNode, dropPosition))
        {
            currentNode.SetIsOccupied(false);
            currentNode = landedNode;
        }
        SetPosition();
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void SetPosition()
    {
        var newPosition = currentNode.transform.position;
        newPosition.y += spriteRenderer.bounds.size.y / 2;
        transform.position = newPosition;
    }
}
