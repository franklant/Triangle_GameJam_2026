using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	[Header("Settings")]
	public string sceneToLoad; // Type the exact name of your scene here (e.g., "puzzle")

	void Update()
	{
		// Detect if the Space bar is pressed
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Load the specified scene
			if (!string.IsNullOrEmpty(sceneToLoad))
			{
				SceneManager.LoadScene(sceneToLoad);
			}
			else
			{
				Debug.LogWarning("No scene name entered in the Inspector!");
			}
		}
	}
}