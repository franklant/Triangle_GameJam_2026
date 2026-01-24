using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public Transform[] spawnPoints; // A list of positions
	public GameObject player;

	void Start()
	{
		// Get the ID we saved from the previous scene
		int id = sceneTeleportator.nextSpawnPoint;

		// Move the player to that specific position
		if (id < spawnPoints.Length)
		{
			player.transform.position = spawnPoints[id].position;
		}
	}
}