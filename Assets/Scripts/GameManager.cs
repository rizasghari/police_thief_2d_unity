using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Node> nodes = new List<Node>();
    [SerializeField] private Node thiefStartNode;
    [SerializeField] private GameObject plicePrefab;
    [SerializeField] private GameObject thiefPrefab;
    [SerializeField] private TMP_Text gameResult;
    [SerializeField] private Color levelCompletedColor;
    [SerializeField] private Color gameOverColor;
    [SerializeField] private AudioClip levelCompletedAudioClip;
    [SerializeField] private AudioClip gameOverAudioClip;
    [SerializeField] private AudioClip pickupAudioClip;
    [SerializeField] private AudioClip putAudioClip;
    [SerializeField] private ParticleSystem policePlacementParticle;
    private AudioSource audioSource;
    public bool IsPoliceTurn { get; private set; }
    public Graph Graph { get; private set; }
    public UnityEvent OnTurnChanged;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Graph = new Graph();
        IsPoliceTurn = true;
    }

    void Start()
    {
        if (nodes.Count > 0)
        {
            foreach (var node in nodes)
            {
                Graph.AddNode(node);
                foreach (var neighbor in node.Neighbors)
                {
                    Graph.AddConnection(node, neighbor);
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
        var spawnedThief = Instantiate(thiefPrefab, spawnPosition, Quaternion.identity);
        Thief thief = spawnedThief.GetComponent<Thief>();
        thief.Initialize(this, thiefStartNode);
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
                    IsPoliceTurn = false;
                    Instantiate(policePlacementParticle, node.transform.position, Quaternion.identity);
                    OnTurnChanged?.Invoke();
                    return true;
                }
            }
        }
        landedNode = null;
        return false;
    }

    private bool CheckAreNeighbor(Node origin, Node destination)
    {
        return Graph.GetNeighbors(origin).Contains(destination);
    }

    public void SetNodesAvailabilityIndicator(Node currentNode, bool isDragging)
    {
        foreach (var node in nodes)
        {
            node.SetNodesAvailabilityIndicator(show: isDragging, isAvailable: CheckAreNeighbor(currentNode, node) && !node.IsOccupied);

        }
    }

    public void LevelCompleted()
    {
        Debug.Log("Level completed");

        PlayAudio(levelCompletedAudioClip);

        gameResult.text = "Level completed";
        gameResult.color = levelCompletedColor;
        gameResult.gameObject.SetActive(true);

        Invoke("RestartLevel", 1.5f);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        PlayAudio(gameOverAudioClip);

        gameResult.text = "Game Over";
        gameResult.color = gameOverColor;
        gameResult.gameObject.SetActive(true);

        Invoke("RestartLevel", 1.5f);
    }

    private void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnThiefMoved(Node currentNode)
    {
        Instantiate(policePlacementParticle, currentNode.transform.position, Quaternion.identity);

        IsPoliceTurn = true;
        OnTurnChanged?.Invoke();
    }

    public void PlayPickupAudio()
    {
        PlayAudio(pickupAudioClip);
    }

    public void PlayPutAudio()
    {
        PlayAudio(putAudioClip);
    }
}