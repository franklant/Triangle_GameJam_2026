using UnityEngine;

public class CursorFollower : MonoBehaviour
{
	void Start()
	{
		// Optional: Hide the standard system mouse pointer 
		// so only the PNG is visible
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}

	void Update()
	{
		// Get the current mouse position in pixels
		Vector2 mousePosition = Input.mousePosition;

		// Set the UI image's position to the mouse position
		transform.position = mousePosition;
	}
}