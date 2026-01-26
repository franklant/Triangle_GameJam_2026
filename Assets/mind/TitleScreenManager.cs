using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
	[Header("Transition Settings")]
	public string firstSceneName = "puzzle"; // Name of your first puzzle scene
	public int startingSpawnID = 1;        // Usually 1 for a new game
	public CanvasGroup fadeCanvasGroup;

	void Start()
	{
		// Ensure the screen is clear at the start
		if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0;

		// Ensure cursor is visible so player can click Start
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void StartGame()
	{
		// Set the spawn point for the first scene
		sceneTeleportator.nextSpawnPoint = startingSpawnID;

		StartCoroutine(FadeAndLoad());
	}

	IEnumerator FadeAndLoad()
	{
		if (fadeCanvasGroup != null)
		{
			float elapsed = 0;
			while (elapsed < 1.0f)
			{
				elapsed += Time.deltaTime;
				fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / 1.0f);
				yield return null;
			}
		}
		SceneManager.LoadScene(firstSceneName);
	}


	public void QuitGame()
	{
		Debug.Log("Game Exiting...");
		Application.Quit();
	}
}