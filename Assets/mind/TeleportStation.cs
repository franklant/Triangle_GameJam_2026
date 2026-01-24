using UnityEngine;

public class TeleportStation : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public GameManager jigsawManager; // Link to your Brain
    public Texture2D puzzleTexture;    // The image to solve

    [Header("Teleport Settings")]
    public Transform spawnPoint;      // The "Dev Box" destination
    public GameObject player;         // Your Player object

    [Header("Visual Prompt")]
    public GameObject promptObject;
    public float floatSpeed = 3f;
    public float floatAmount = 0.2f;

    private bool isPlayerInside = false;
    private Vector3 startPos;

    void Start()
    {
        if (promptObject != null)
        {
            promptObject.SetActive(false);
            startPos = promptObject.transform.localPosition;
        }
    }

    void Update()
    {
        // TRIGGER PUZZLE IMMEDIATELY
        if (isPlayerInside && Input.GetKeyDown(KeyCode.X))
        {
            if (jigsawManager != null && puzzleTexture != null)
            {
                // This skips the menu and generates the pieces for your specific texture
                jigsawManager.StartGame(puzzleTexture);

                if (promptObject != null) promptObject.SetActive(false);
            }
        }

        // FLOATING PROMPT LOGIC
        if (isPlayerInside && promptObject != null)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            promptObject.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    // This method is now called by the GameManager when the puzzle ends
    public void TeleportPlayer()
    {
        if (spawnPoint != null && player != null)
        {
            player.transform.position = spawnPoint.position;
            Debug.Log("Puzzle Complete! Teleporting to: " + spawnPoint.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (promptObject != null) promptObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (promptObject != null) promptObject.SetActive(false);
        }
    }
}