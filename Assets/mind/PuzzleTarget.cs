using UnityEngine;

public class PuzzleTarget : MonoBehaviour
{
	public PuzzleManager puzzleManager; // Drag your PuzzleManager object here

	// This built-in Unity function detects mouse clicks on this object
	private void OnMouseDown()
	{
		Debug.Log("Clue Found!");

		// Notify the manager that the puzzle is solved
		if (puzzleManager != null)
		{
			puzzleManager.OnPuzzleComplete();
		}
	}
}