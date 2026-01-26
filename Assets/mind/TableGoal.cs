using UnityEngine;

public class TableGoal : MonoBehaviour
{
	public PuzzleManager puzzleManager;
	public int totalItemsToFind = 3;
	private int itemsFound = 0;
	private bool waitingForFinalSpace = false;

	private void OnTriggerEnter(Collider other)
	{
		ItemDescription item = other.GetComponent<ItemDescription>();

		if (item != null && item.enabled)
		{
			itemsFound++;
			item.enabled = false; // Prevents double-triggering

			if (itemsFound >= totalItemsToFind)
			{
				// Show the 3rd item's text, but set a flag to wait
				puzzleManager.ShowItemText(item.description);
				waitingForFinalSpace = true;
			}
			else
			{
				// Just show the normal description for items 1 and 2
				puzzleManager.ShowItemText(item.description);
			}
		}
	}

	void Update()
	{
		// If we've shown the 3rd item and the player presses Space
		if (waitingForFinalSpace && Input.GetKeyDown(KeyCode.Space))
		{
			waitingForFinalSpace = false;
			puzzleManager.OnPuzzleComplete();
		}
	}
}