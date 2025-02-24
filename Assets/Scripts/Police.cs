using UnityEngine;

public class Police : MonoBehaviour
{
    private Vector3 mousePositionOffset;
    private Node currentNode;
    private GameManager gameManager;

    public void Initialize(GameManager gameManager, Node startNode) {
        this.currentNode = startNode;
        this.gameManager = gameManager;
    }

    void OnMouseDown()
    {
        mousePositionOffset = GetMouseWorldPosition() - transform.position;
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() - mousePositionOffset;
    }

    private void OnMouseUp() {
        Vector3 dropPosition = GetMouseWorldPosition() - mousePositionOffset;
      
        if (gameManager.GetNodeOnDropPosition(dropPosition, out Node landedNode))
        {
            currentNode.SetIsOccupied(false);
            currentNode = landedNode;
        } else {
            transform.position = currentNode.transform.position;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetCurrentNode(Node node) {
        currentNode = node;
    }
}
