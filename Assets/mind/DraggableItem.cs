using UnityEngine;

public class DraggableItem : MonoBehaviour
{
	private Vector3 mOffset;
	private float mZCoord;

	void OnMouseDown()
	{
		// 1. Record the distance from camera to object
		mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

		// 2. Calculate the offset so the object doesn't "snap" its center to the mouse
		mOffset = gameObject.transform.position - GetMouseWorldPos();
	}

	private Vector3 GetMouseWorldPos()
	{
		// Get mouse position in pixel coordinates (x,y)
		Vector3 mousePoint = Input.mousePosition;

		// Add the Z coordinate recorded during MouseDown
		mousePoint.z = mZCoord;

		// Convert it to world coordinates
		return Camera.main.ScreenToWorldPoint(mousePoint);
	}

	void OnMouseDrag()
	{
		// Move the object to follow the mouse position + original offset
		transform.position = GetMouseWorldPos() + mOffset;
	}
}