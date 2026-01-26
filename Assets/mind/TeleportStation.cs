using UnityEngine;

public class TeleportStation : MonoBehaviour
{
	[Header("Teleport Settings")]
	public Transform spawnPoint;      // Drag the dev box (spawn point) here
	public GameObject player;         // Drag your Player object here
	[Tooltip("Drag Audio Manager Here")]
	public GameObject audioManager;

	[Header("Visual Prompt")]
	public GameObject promptObject;
	public float floatSpeed = 3f;
	public float floatAmount = 0.2f;

	private bool isPlayerInside = false;
	private Vector3 startPos;
	private int spawnNumber;		// keeps track of the spawn point number set.

	void Start()
	{
		if (promptObject != null)
		{
			promptObject.SetActive(false);
			startPos = promptObject.transform.localPosition;
		}

		// extract the spawn point number from it's name to be used as the music level.
		spawnNumber = int.Parse(spawnPoint.name.Substring(5));
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
			// SWITCH MUSIC HERE
			audioManager.GetComponent<audioController>().SwitchLevel(spawnNumber);

			// Move the player to the exact position of the dev box
			player.transform.position = spawnPoint.position;
			Debug.Log("Teleported to: " + spawnPoint.name);
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