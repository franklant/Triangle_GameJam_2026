using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
	[Header("Transition Settings")]
	public string nextAreaName;
	public int targetSpawnID;
	public CanvasGroup fadeCanvasGroup;

	[Header("Dialogue UI")]
	public GameObject dialoguePanel;
	public Text dialogueText;
	public Button nextButton;

	[Header("Script")]
	[TextArea(3, 10)]
	public string[] dialogueLines; // A list of dialogue lines
	private int currentLineIndex = 0;

	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		dialoguePanel.SetActive(false);
		if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0;

		// The button now calls 'ShowNextLine' instead of starting the transition immediately
		nextButton.onClick.AddListener(ShowNextLine);
	}

	public void OnPuzzleComplete()
	{
		dialoguePanel.SetActive(true);
		currentLineIndex = 0;
		UpdateText();
	}

	void ShowNextLine()
	{
		currentLineIndex++;

		// Check if we still have lines left
		if (currentLineIndex < dialogueLines.Length)
		{
			UpdateText();
		}
		else
		{
			// No more lines? Start the teleport!
			StartTransition();
		}
	}

	void UpdateText()
	{
		dialogueText.text = dialogueLines[currentLineIndex];
	}

	void StartTransition()
	{
		sceneTeleportator.nextSpawnPoint = targetSpawnID;
		StartCoroutine(TransitionToNextArea());
	}

	IEnumerator TransitionToNextArea()
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
		SceneManager.LoadScene(nextAreaName);
	}
}