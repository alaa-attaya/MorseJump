using System.Collections;
using UnityEngine;
using TMPro;  // Import for TextMeshPro

public class SpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;  // The prefab for the obstacle
        [Range(0f, 1f)]
        public float spawnChance;  // Chance of spawning the obstacle (0.0 to 1.0)
    }

    // Arrays for dot and dash obstacles
    public SpawnableObject[] dotPrefabs;  // Array of spawnable objects for dots (.)
    public SpawnableObject[] dashPrefabs; // Array of spawnable objects for dashes (-)
    public float minSpawnRate = 1f;      // Minimum spawn rate for the next obstacle
    public float maxSpawnRate = 2f;      // Maximum spawn rate for the next obstacle
    public float morseCodeSpeed = 0.5f;  // Time between obstacles in the sequence

    public TextMeshProUGUI morseCodeDisplay; // UI Text for displaying Morse code
    public TextMeshProUGUI scoreDisplay; // UI Text for displaying Score
    private AudioSource audioSource; // AudioSource for sounds

    public AudioClip scoreIncrementClip; // Sound for score increment

    // Morse code for 0-9
    private static readonly string[] morseCodeNumbers = new string[] 
    {
        "-----",  // 0
        ".----",  // 1
        "..---",  // 2
        "...--",  // 3
        "....-",  // 4
        ".....",  // 5
        "-....",  // 6
        "--...",  // 7
        "---..",  // 8
        "----."   // 9
    };

    private int currentNumber = 0;  // The current number whose Morse code we're using
    private int currentScore = 0;   // The actual score that gets updated and returned

    private void Awake()
    {
        // Set up the AudioSource component
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();  // Ensure the AudioSource is assigned
        }
    }

    private void OnEnable()
    {   ResetScore();
        StartCoroutine(EndlessSpawning());
    }

    private void OnDisable()
    {
        StopCoroutine(EndlessSpawning());
    }

    private IEnumerator EndlessSpawning()
    {
        while (true)
        {
            // Convert the current number to Morse code
            string morseCode = NumberToMorseCode(currentNumber);

            // Update the display with the current number and Morse code
            UpdateDisplay(currentNumber, morseCode);

            // Play the score increment sound before displaying the score
            PlayScoreIncrementSound();

            // Spawn obstacles based on the Morse code sequence
            foreach (char symbol in morseCode)
            {
                // Spawn the appropriate obstacle for dot or dash
                SpawnObstacle(symbol);
                // Wait for the time between obstacles in the sequence
                yield return new WaitForSeconds(morseCodeSpeed);
            }

            // Increment the number
            currentNumber++;

            // Wait for a random time between sequences
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        }
    }

    private string NumberToMorseCode(int number)
    {
        string morseCode = "";

        // Convert the number to Morse code by breaking it into digits
        foreach (char digit in number.ToString())
        {
            int digitInt = digit - '0'; // Convert char to int
            morseCode += morseCodeNumbers[digitInt] + " "; // Append the Morse code for the digit
        }

        return morseCode.Trim(); // Remove trailing space
    }

    private void SpawnObstacle(char morseSymbol)
    {
        GameObject obstaclePrefab = null;

        if (morseSymbol == '.')
        {
            // Pick a prefab from the dotPrefabs array
            obstaclePrefab = GetRandomPrefabWithChance(dotPrefabs);
        }
        else if (morseSymbol == '-')
        {
            // Pick a prefab from the dashPrefabs array
            obstaclePrefab = GetRandomPrefabWithChance(dashPrefabs);
        }

        // Instantiate the obstacle at the current position
        if (obstaclePrefab != null)
        {
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.transform.position += transform.position; // Adjust spawn position
        }
    }

    private GameObject GetRandomPrefabWithChance(SpawnableObject[] prefabs)
    {
        float randomValue = Random.value; // Get a random value between 0 and 1
        float cumulativeChance = 0f;

        foreach (var spawnable in prefabs)
        {
            cumulativeChance += spawnable.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                return spawnable.prefab;
            }
        }

        return null; // If no prefab is selected
    }

    private void UpdateDisplay(int number, string morseCode)
    {
        if (morseCodeDisplay != null && scoreDisplay != null)
        {
            // Update the UI text with the Score and Morse code
            scoreDisplay.text = $"SCORE\n{number}";
            morseCodeDisplay.text = $"MORSE CODE\n{morseCode}";

            // Update the current score
            currentScore = number;

            // Change both texts' color for 1 morse code speed
            StartCoroutine(ChangeTextColorTemporarily());
        }
    }

    // Change both score and morse code text color temporarily
    private IEnumerator ChangeTextColorTemporarily()
    {
        // Set the color to #4873F3 (blue)
        scoreDisplay.color = new Color(243f / 255f, 72f / 255f, 72f / 255f); // Hex: #F34848

        morseCodeDisplay.color = new Color(72f / 255f, 115f / 255f, 243f / 255f); // Hex: #4873F3

        // Wait for the morseCodeSpeed duration
        yield return new WaitForSeconds(morseCodeSpeed);

        // Reset the color to default (white)
        scoreDisplay.color = Color.white;
        morseCodeDisplay.color = Color.white;
    }

    // Play sound for score increment
    private void PlayScoreIncrementSound()
    {
        if (scoreIncrementClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(scoreIncrementClip);  // Play the score increment sound immediately
        }
    }

    // Method to reset the score
    public void ResetScore()
    {
        currentNumber = 0;  // Reset the number (display purpose)
        currentScore = 0;   // Reset the actual score
        UpdateDisplay(currentNumber, NumberToMorseCode(currentNumber));  // Update the UI display
    }

    // Method to get the current score
    public int GetScore()
    {
        return currentScore;  // Return the actual score
    }
}
