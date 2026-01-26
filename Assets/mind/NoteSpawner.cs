using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
	public GameObject notePrefab;
	public float spawnRate = 1.5f;
	public float xRange = 7f; // How wide the notes spread out

	void Start()
	{
		InvokeRepeating("SpawnNote", 0.5f, spawnRate);
	}

	void SpawnNote()
	{
		float randomX = Random.Range(-xRange, xRange);
		Vector3 spawnPos = new Vector3(randomX, transform.position.y, transform.position.z);
		Instantiate(notePrefab, spawnPos, Quaternion.identity);
	}

	public void StopSpawning()
	{
		CancelInvoke("SpawnNote");
	}
}