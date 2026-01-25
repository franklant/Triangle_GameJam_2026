using UnityEngine;

public class ResetMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("MusicLevel") > 0)
        {
            PlayerPrefs.SetInt("MusicLevel", 1);
        }
    }
}
