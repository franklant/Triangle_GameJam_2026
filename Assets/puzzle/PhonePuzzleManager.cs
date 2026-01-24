using UnityEngine;

public class PhonePuzzleManager : MonoBehaviour
{
	[Header("Screens")]
	public GameObject homeScreen;
	public GameObject messageList;
	public GameObject textConversation;

	// Call this from the "Messages" App Icon button
	public void OpenMessageList()
	{
		homeScreen.SetActive(false);
		messageList.SetActive(true);
	}

	// Call this from a specific contact in the list
	public void OpenSpecificText()
	{
		messageList.SetActive(false);
		textConversation.SetActive(true);
	}

	// Call this to go back
	public void GoToHome()
	{
		textConversation.SetActive(false);
		messageList.SetActive(false);
		homeScreen.SetActive(true);
	}
}