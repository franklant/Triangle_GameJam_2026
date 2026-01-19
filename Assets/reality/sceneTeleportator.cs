using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTeleportator : MonoBehaviour
{
	[Header("Transition Settings")]
	public string sceneToLoad;

	[Header("Visual Prompt")]
	public GameObject promptObject;
	public float floatSpeed = 3f;    // How fast it moves
	public float floatAmount = 0.2f; // How far up and down it goes

	private bool isPlayerOverlapping = false;
	private Vector3 startPos;

	void Start()
	{
		if (promptObject != null)
		{
			promptObject.SetActive(false);
			// Record the starting height of the X icon
			startPos = promptObject.transform.localPosition;
		}
	}

	void Update()
	{
		// 1. TELEPORT LOGIC (Checking for the X key)
		if (isPlayerOverlapping && Input.GetKeyDown(KeyCode.X))
		{
			SceneManager.LoadScene(sceneToLoad);
		}

		// 2. FLOATING LOGIC
		if (isPlayerOverlapping && promptObject != null)
		{
			// Calculate new Y position using a Sine wave
			float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
			promptObject.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerOverlapping = true;
			if (promptObject != null) promptObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerOverlapping = false;
			if (promptObject != null) promptObject.SetActive(false);
		}
	}
}