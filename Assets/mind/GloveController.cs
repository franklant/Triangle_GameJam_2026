using UnityEngine;

public class GloveController : MonoBehaviour
{
	[Header("Movement")]
	public float patrolSpeed = 3f;
	public float patrolWidth = 5f;
	public float dropSpeed = 10f;
	public float maxDropDistance = 5f; // How far down it goes before turning back

	private Vector3 startPos;
	private bool isDropping = false;
	private bool isReturning = false;

	void Start()
	{
		// Save the exact starting position so we know where to return to
		startPos = transform.position;
	}

	void Update()
	{
		if (!isDropping && !isReturning)
		{
			// 1. PATROL MODE: Move left and right
			float x = Mathf.PingPong(Time.time * patrolSpeed, patrolWidth) - (patrolWidth / 2f);
			transform.position = new Vector3(startPos.x + x, startPos.y, startPos.z);

			// Trigger drop on Space
			if (Input.GetMouseButtonDown(0))
			{
				isDropping = true;
			}
		}
		else if (isDropping)
		{
			// 2. DROP MODE: Move strictly down on the Y axis
			transform.position += Vector3.down * dropSpeed * Time.deltaTime;

			// Check if we reached the bottom limit
			if (transform.position.y < startPos.y - maxDropDistance)
			{
				isDropping = false;
				isReturning = true;
			}
		}
		else if (isReturning)
		{
			// 3. RETURN MODE: Move strictly back up to the starting Y height
			transform.position += Vector3.up * dropSpeed * Time.deltaTime;

			// Stop once we get back to (or past) the original height
			if (transform.position.y >= startPos.y)
			{
				transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
				isReturning = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check for the "Goal" tag we set up
		if (other.CompareTag("Goal"))
		{
			// Tell the manager we caught one!
			FindObjectOfType<PuzzleManager>().CatchBottle();

			// Hide the bottle spot so we can't hit it again
			other.gameObject.SetActive(false);

			// Immediately start moving back up
			isDropping = false;
			isReturning = true;
		}
	}
}
