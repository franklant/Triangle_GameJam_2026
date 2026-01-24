using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class sceneTeleportator : MonoBehaviour
{
	[Header("Transition Settings")]
	public string sceneToLoad;
	public int targetSpawnID; // Set this to 0, 1, 2, etc. in the Inspector

	// This 'static' variable persists between scenes
	public static int nextSpawnPoint;

	[Header("Fade Settings")]
	public CanvasGroup fadeCanvasGroup;
	public float fadeDuration = 1.0f;

	[Header("Visual Prompt (The X)")]
	public GameObject promptObject;
	public float floatSpeed = 3f;
	public float floatAmount = 0.2f;

	private bool isPlayerOverlapping = false;
	private bool isTransitioning = false;
	private Vector3 startPos;

	void Start()
	{
		if (promptObject != null)
		{
			promptObject.SetActive(false);
			startPos = promptObject.transform.localPosition;
		}

		if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0;
	}

	void Update()
	{
		if (isTransitioning) return;

		// Press X to Teleport
		if (isPlayerOverlapping && Input.GetKeyDown(KeyCode.X))
		{
			nextSpawnPoint = targetSpawnID; // Save the ID before moving
			StartCoroutine(FadeAndTeleport());
		}

		// Floating logic for the X icon
		if (isPlayerOverlapping && promptObject != null)
		{
			float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
			promptObject.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
		}
	}

	IEnumerator FadeAndTeleport()
	{
		isTransitioning = true;
		float elapsed = 0;

		if (fadeCanvasGroup != null)
		{
			while (elapsed < fadeDuration)
			{
				elapsed += Time.deltaTime;
				fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
				yield return null;
			}
		}

		SceneManager.LoadScene(sceneToLoad);
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