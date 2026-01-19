using UnityEngine;

public class LightSwitch : MonoBehaviour
{
	[Header("Light & Darkness")]
	public GameObject darknessPlane;
	public Animator lightAnimator;

	[Header("Visual Prompt")]
	public GameObject promptObject;
	public float floatSpeed = 3f;    // How fast it moves
	public float floatAmount = 0.2f; // How far up and down it goes

	private bool isPlayerInside = false;
	private bool lightIsOn = false;
	private Vector3 startPos;

	void Start()
	{
		// Setup the prompt icon
		if (promptObject != null)
		{
			promptObject.SetActive(false);
			// Record the starting height of the icon
			startPos = promptObject.transform.localPosition;
		}
	}

	// This handles the Plane toggle via Animation Events
	public void ToggleLight(int state)
	{
		if (darknessPlane != null)
		{
			// state 0 = Light is OFF (show darkness plane)
			// state 1 = Light is ON (hide darkness plane)
			darknessPlane.SetActive(state == 0);
		}
	}

	void Update()
	{
		// 1. LIGHT TOGGLE LOGIC
		if (isPlayerInside && Input.GetKeyDown(KeyCode.X))
		{
			lightIsOn = !lightIsOn;

			if (lightAnimator != null)
			{
				// Plays your specific animation states
				lightAnimator.Play(lightIsOn ? "lighton" : "lightoff");
			}
		}

		// 2. FLOATING PROMPT LOGIC
		if (isPlayerInside && promptObject != null)
		{
			// Calculate new Y position using a Sine wave
			float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
			promptObject.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
		}
	}

	// Trigger detection
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerInside = true;
			if (promptObject != null) promptObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerInside = false;
			if (promptObject != null) promptObject.SetActive(false);
		}
	}
}