using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // For Button handling

public class ButtonSoundManager : MonoBehaviour
{
    // Reference to the AudioSource component (on the same GameObject)
    private AudioSource audioSource;

    // Audio clip to play on button click
    public AudioClip clickSound;

    // List of buttons that we will manually assign in the Inspector
    public Button[] buttons;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Add click listeners to each button manually assigned in the Inspector
        foreach (Button button in buttons)
        {
            // Add listener to the button's onClick event
            button.onClick.AddListener(() => PlayClickSound());
        }
    }

    // This method will be called when the button is clicked
    private void PlayClickSound()
    {
        // Play click sound if the audio clip is assigned
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound); // Play click sound once
        }
    }
}
