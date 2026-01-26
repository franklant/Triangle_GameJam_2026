using UnityEngine;

public class MusicNote : MonoBehaviour
{
	public float speed = 2f;
	private PuzzleManager manager;

	void Start()
	{
		// Find the manager in the scene
		manager = Object.FindAnyObjectByType<PuzzleManager>();

		// Destroy the note after 5 seconds so they don't clutter the game
		Destroy(gameObject, 5f);
	}

	void Update()
	{
		// Float upward
		transform.Translate(Vector3.up * speed * Time.deltaTime);
	}

	void OnMouseDown()
	{
		// When clicked, tell the manager and disappear
		if (manager != null)
		{
			manager.IncrementNoteCount();
		}
		Destroy(gameObject);
	}
}
