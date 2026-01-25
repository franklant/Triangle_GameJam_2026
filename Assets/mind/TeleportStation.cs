using UnityEngine;

public class TeleportStation : MonoBehaviour
{
	[Header("Teleport Settings")]
	public Transform spawnPoint;      // Drag the dev box (spawn point) here
	public GameObject player;         // Drag your Player object here
	[Tooltip("Drag the audio manager prefab here.")]
	public GameObject audioManager; 	  // Drag the Audio Manager prefab here

	[Header("Visual Prompt")]
	public GameObject promptObject;
	public float floatSpeed = 3f;
	public float floatAmount = 0.2f;

	private bool isPlayerInside = false;
	private Vector3 startPos;
	private int spawnNumber;

	void Start()
	{
		if (promptObject != null)
		{
			promptObject.SetActive(false);
			startPos = promptObject.transform.localPosition;
		}

		if (spawnPoint.name.Contains("spawn"))
		{
			spawnNumber = int.Parse(spawnPoint.name.Substring(5));
		}
		Debug.Log("SPAWN: " + spawnNumber);
	}

	void Update()
	{
		// 1. TELEPORT LOGIC
		if (isPlayerInside && Input.GetKeyDown(KeyCode.X))
		{
			TeleportPlayer();
		}

		// 2. FLOATING PROMPT LOGIC
		if (isPlayerInside && promptObject != null)
		{
			float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
			promptObject.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
		}
	}

	void TeleportPlayer()
	{
		if (spawnPoint != null && player != null)
		{
			// Move the player to the exact position of the dev box
			player.transform.position = spawnPoint.position;
			Debug.Log("Teleported to: " + spawnPoint.name);

			if (spawnNumber <= 5 && audioManager != null)
			{
				audioController ac = audioManager.GetComponent<audioController>();
				ac.SwitchLevel(spawnNumber);
			}
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