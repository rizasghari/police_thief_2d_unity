using UnityEngine;

public class Thief : MonoBehaviour
{
    private Node currentNode;
    private GameManager gameManager;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Initialize(GameManager gameManager, Node startNode)
    {
        this.currentNode = startNode;
        this.gameManager = gameManager;

        this.gameManager.OnTurnChanged.AddListener(OnTurnChanged);
    }

    private void OnTurnChanged()
    {
        if (!gameManager.IsPoliceTurn)
        {
            Invoke("Move", 0.5f);
        }
    }

    private void Move()
    {
        var availableNeighbors = gameManager.Graph.GetNeighbors(currentNode).FindAll(node => !node.IsOccupied);
        if (availableNeighbors.Count > 0)
        {
            Debug.Log("Neighbors: " + availableNeighbors.Count);

            Node destination;

            destination = availableNeighbors.Find(node => node.IsExit);

            if (destination == null)
            {
                destination = availableNeighbors[Random.Range(0, availableNeighbors.Count)];
            }

            currentNode.SetIsOccupied(false);
            currentNode = destination;
            currentNode.SetIsOccupied(true);

            SetPosition();

            gameManager.OnThiefMoved(currentNode);

            if (currentNode.IsExit)
            {
                gameManager.GameOver();
            }
        }
        else
        {
            gameManager.LevelCompleted();
        }
    }

    private void SetPosition()
    {
        gameManager.PlayPutAudio();

        var newPosition = currentNode.transform.position;
        newPosition.y += spriteRenderer.bounds.size.y / 2;
        transform.position = newPosition;
    }
}
