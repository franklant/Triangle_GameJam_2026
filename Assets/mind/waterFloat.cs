using UnityEngine;

public class WaterFloat : MonoBehaviour
{
	[Header("Movement Settings")]
	public float speed = 1.0f;      // How fast it moves
	public float distance = 0.5f;   // How far it travels left and right

	private Vector3 startPosition;

	void Start()
	{
		// Remember where the water started in the scene
		startPosition = transform.position;
	}

	void Update()
	{
		// Calculate the new X position using a sine wave for smooth back-and-forth motion
		float newX = startPosition.x + Mathf.Sin(Time.time * speed) * distance;

		// Apply the new position while keeping Y and Z the same
		transform.position = new Vector3(newX, startPosition.y, startPosition.z);
	}
}
