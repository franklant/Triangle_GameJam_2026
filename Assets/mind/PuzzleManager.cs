using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
	[Header("Transition Settings")]
	public string nextAreaName;
	public int targetSpawnID;
	public CanvasGroup fadeCanvasGroup;

	[Header("Dialogue UI")]
	public GameObject dialoguePanel;
	public UnityEngine.UI.Text dialogueText;

	[Header("Investigation Script")]
	[TextArea(3, 10)]
	public string[] dialogueLines;
	private int currentLineIndex = 0;
	private bool isDialogueActive = false;

	[Header("Music Note Puzzle")]
	public int notesRequired = 10;
	private int notesCollected = 0;
	public MonoBehaviour spawner;

	[Header("Glove/Bottle Puzzle")]
	public int bottlesNeeded = 2;
	private int bottlesCaught = 0;

	[Header("Ending UI (Separate Panels)")]
	public EndingData[] allEndings;

	[System.Serializable]
	public struct EndingData
	{
		public string suspectName;
		public GameObject specificEndingPanel;
	}

	// --- FIX FOR THE PHONE GLITCH ---
	// This removes focus from any buttons so Space doesn't re-trigger them
	public void ClearUIFocus()
	{
		if (EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	void Start()
	{
		if (dialoguePanel != null) dialoguePanel.SetActive(false);
		foreach (var ending in allEndings)
		{
			if (ending.specificEndingPanel != null)
				ending.specificEndingPanel.SetActive(false);
		}
		if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0;
	}

	void Update()
	{
		if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
		{
			if (currentLineIndex == 999)
			{
				CloseDialogue();
			}
			else if (dialogueLines != null && currentLineIndex < dialogueLines.Length - 1)
			{
				ShowNextLine();
			}
			else
			{
				StartTransition();
			}
		}
	}

	public void StartTransition()
	{
		isDialogueActive = false;
		if (dialoguePanel != null) dialoguePanel.SetActive(false);

		// Tell teleport system where to go
		sceneTeleportator.nextSpawnPoint = targetSpawnID;

		StartCoroutine(TransitionRoutine());
	}

	IEnumerator TransitionRoutine()
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
		if (!string.IsNullOrEmpty(nextAreaName)) SceneManager.LoadScene(nextAreaName);
	}

	public void ShowItemText(string newText)
	{
		if (dialoguePanel == null) return;

		ClearUIFocus(); // Force the UI to stop looking at the phone buttons

		dialoguePanel.SetActive(true);
		dialogueText.text = newText;
		isDialogueActive = true;
		currentLineIndex = 999;
	}

	public void OnPuzzleComplete()
	{
		if (dialoguePanel == null) return;

		ClearUIFocus(); // Force focus off any buttons

		dialoguePanel.SetActive(true);
		isDialogueActive = true;
		currentLineIndex = 0;
		UpdateText();
	}

	public void ShowEnding(int suspectIndex)
	{
		if (allEndings.Length <= suspectIndex) return;
		foreach (var ending in allEndings)
			if (ending.specificEndingPanel != null) ending.specificEndingPanel.SetActive(false);

		GameObject chosenPanel = allEndings[suspectIndex].specificEndingPanel;
		if (chosenPanel != null) chosenPanel.SetActive(true);
	}
	public void ReturnToMainMenu()
	{
		// Ensure "TitleScreen" matches the exact name of your title scene in Build Settings
		SceneManager.LoadScene("title");
	}

	public void CatchBottle() { bottlesCaught++; if (bottlesCaught >= bottlesNeeded) OnPuzzleComplete(); }
	public void IncrementNoteCount() { notesCollected++; if (notesCollected >= notesRequired) OnPuzzleComplete(); }
	void ShowNextLine() { currentLineIndex++; UpdateText(); }
	void UpdateText() { if (dialogueLines.Length > 0 && currentLineIndex < dialogueLines.Length) dialogueText.text = dialogueLines[currentLineIndex]; }
	void CloseDialogue() { isDialogueActive = false; if (dialoguePanel != null) dialoguePanel.SetActive(false); }
}