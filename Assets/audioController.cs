using System;
using UnityEngine;

public class audioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip layer1;
    public AudioClip layer2;
    public AudioClip layer3;
    public AudioClip layer4;
    public AudioClip layer5;
    public int currentLevel;
    private AudioClip[] layers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevel = 1;
        AudioClip[] l = {layer1, layer2, layer3, layer4, layer5};
        layers = l;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // Set the current music level to 1 if the player prefs is not set already.
        if (PlayerPrefs.GetInt("MusicLevel") == 0) 
        {
            PlayerPrefs.SetInt("MusicLevel", currentLevel);
        }
        else 
        {
            Debug.Log("BOOOOOM");
            currentLevel = PlayerPrefs.GetInt("MusicLevel");
        }

        audioSource.clip = layers[currentLevel - 1];
        audioSource.Play();
    }

    public void IncreaseLevel()
    {
        if (currentLevel < 5)
        {
            currentLevel += 1;
            audioSource.clip = layers[currentLevel - 1];
            audioSource.Play();
        }
    }

    public void SwitchLevel(int level)
    {
        if (level <= 5 && level != currentLevel)
        {
            currentLevel = level;
            audioSource.clip = layers[currentLevel - 1];
            audioSource.Play();
        }
    }
}
