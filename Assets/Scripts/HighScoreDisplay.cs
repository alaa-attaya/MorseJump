using System.Collections;
using System.Collections.Generic;
using TMPro; // Import the TextMesh Pro library
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; // Reference to the TextMeshPro component

    private void Start()
    {
        // Get the TextMeshPro component attached to this GameObject
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Update the text with the current high score
        UpdateHighScoreText();
    }

    private void UpdateHighScoreText()
    {
        if (HighScoreManager.Instance != null)
        {
            int highScore = HighScoreManager.Instance.GetHighScore();
            textMeshPro.text = $"HIGH SCORE\n{highScore}";
        }
        else
        {
            Debug.LogWarning("HighScoreManager instance not found!");
        }
    }

    // Optional: Call this method to refresh the score dynamically
    public void RefreshHighScore()
    {
        UpdateHighScoreText();
    }
}
