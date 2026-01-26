using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvestigationDialogue : MonoBehaviour
{
	[Header("UI Slots")]
	public GameObject dialoguePanel;
	public UnityEngine.UI.Text dialogueText;

	[Header("Dialogue Content")]
	[TextArea(3, 10)]
	public string[] lines;
	private int currentLineIndex = -1; // Starts at -1 to show the prompt first
	private bool isPlayerInRange = false;
	private bool isDialogueActive = false;

	[Header("Prompt Settings")]
	public string initialPrompt = "[Press Space to Investigate]";

	void Update()
	{
		// Only allow interaction if the player is in the trigger zone
		if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
		{
			if (!isDialogueActive)
			{
				StartDialogue();
			}
			else
			{
				ShowNextLine();
			}
		}
	}

	void StartDialogue()
	{
		isDialogueActive = true;
		currentLineIndex = -1; // Set to -1 to ensure we show the prompt first

		// Fix for the button-focus glitch
		if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);

		if (dialoguePanel != null) dialoguePanel.SetActive(true);

		// Show the prompt in the actual dialogue box
		if (dialogueText != null) dialogueText.text = initialPrompt;
	}

	void ShowNextLine()
	{
		currentLineIndex++;

		if (currentLineIndex < lines.Length)
		{
			UpdateText();
		}
		else
		{
			EndDialogue();
		}
	}

	void UpdateText()
	{
		if (dialogueText != null && lines.Length > 0)
			dialogueText.text = lines[currentLineIndex];
	}

	void EndDialogue()
	{
		isDialogueActive = false;
		if (dialoguePanel != null) dialoguePanel.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerInRange = true;
			// Automatically open the panel to show the prompt when they walk in
			StartDialogue();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerInRange = false;
			EndDialogue();
		}
	}
}