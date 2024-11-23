using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioManager : MonoBehaviour
{
    // Reference to the AudioSource component
    private AudioSource audioSource;

    // The audio clip to play when the menu loads
    public AudioClip menuMusic;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if we are in the MenuScene and the AudioSource & AudioClip are assigned
        if (SceneManager.GetActiveScene().name == "MenuScene") 
        {
            if (audioSource != null && menuMusic != null)
            {
                audioSource.clip = menuMusic;
                audioSource.loop = true; // Enable looping
                audioSource.Play();      // Start playing the audio
            }
            else
            {
                Debug.LogWarning("AudioSource or MenuMusic is missing");
            }
        }
    }
}
