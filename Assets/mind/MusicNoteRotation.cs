using UnityEngine;

public class MusicNoteEffects : MonoBehaviour
{
	[Header("Squash and Stretch")]
	public float stretchAmount = 0.15f;
	public float stretchSpeed = 4f;

	[Header("Wobble")]
	public float wobbleIntensity = 1.2f; // Side-to-side distance
	public float wobbleSpeed = 2.5f;     // Speed of the swing

	private Vector3 originalScale;
	private float startX;
	private float timeOffset;

	void Start()
	{
		originalScale = transform.localScale;
		startX = transform.position.x;

		// Randomize so they don't move in a perfect grid
		timeOffset = Random.Range(0f, 10f);

		// Keep the sprite perfectly upright at the start
		transform.rotation = Quaternion.identity;
	}

	void Update()
	{
		// 1. SQUASH & STRETCH (Stays upright)
		// This pulses the width and height without tilting the sprite
		float squash = Mathf.Sin((Time.time + timeOffset) * stretchSpeed) * stretchAmount;
		transform.localScale = new Vector3(
			originalScale.x + squash,
			originalScale.y - squash,
			originalScale.z
		);

		// 2. WOBBLE (Horizontal drift)
		float xOffset = Mathf.Sin((Time.time + timeOffset) * wobbleSpeed) * wobbleIntensity;

		// We update the X position while allowing the Y to be moved by your float script
		transform.position = new Vector3(startX + xOffset, transform.position.y, transform.position.z);
	}
}