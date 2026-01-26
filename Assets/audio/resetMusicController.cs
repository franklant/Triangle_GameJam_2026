using UnityEngine;

public class resetMusicController : MonoBehaviour
{
    [Tooltip("Drag the Audio Manager Here")]
    public GameObject audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        // Reset the music level to 0 if the player preferences is already set.
        if (PlayerPrefs.GetInt("MusicLevel") != 0)
        {
            PlayerPrefs.SetInt("MusicLevel", 0);
        } 

        // if all else fails, just switch the current music level to zero.
        audioManager.GetComponent<audioController>().SwitchLevel(1);
    }
}
