using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private string filePath; // Path to the JSON file
    private HighScoreData highScoreData; // Class to hold the high score data

    // Singleton instance for easy access
    public static HighScoreManager Instance { get; private set; }

    // Reference to the HighScoreDisplay (set this in the Inspector or find it dynamically)
    private HighScoreDisplay highScoreDisplay;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        // Set the file path
        filePath = Path.Combine(Application.persistentDataPath, "HighScore.json");

        // Load high score data from the JSON file
        LoadHighScore();

        // Find the HighScoreDisplay script (or you can assign it directly in the Inspector)
        highScoreDisplay = FindObjectOfType<HighScoreDisplay>();
    }

    public int GetHighScore()
    {
        return highScoreData.highScore;
    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > highScoreData.highScore)
        {
            highScoreData.highScore = newScore;
            SaveHighScore();

            // Call RefreshHighScore to update the display
            if (highScoreDisplay != null)
            {
                highScoreDisplay.RefreshHighScore();
            }
        }
    }

    private void LoadHighScore()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON file and deserialize it
            try
            {
                string json = File.ReadAllText(filePath);
                highScoreData = JsonUtility.FromJson<HighScoreData>(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error loading high score: {ex.Message}. Reinitializing high score.");
                InitializeDefaultHighScore();
            }
        }
        else
        {
            Debug.LogWarning("High score file not found. Initializing default high score.");
            InitializeDefaultHighScore();
        }
    }

    private void InitializeDefaultHighScore()
    {
        highScoreData = new HighScoreData { highScore = 0 };
        SaveHighScore(); // Create the file with default data
    }

    private void SaveHighScore()
    {
        try
        {
            // Serialize the high score data to JSON and save it
            string json = JsonUtility.ToJson(highScoreData, true);
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error saving high score: {ex.Message}");
        }
    }

    // Inner class to hold the high score data
    [System.Serializable]
    public class HighScoreData
    {
        public int highScore;
    }
}
