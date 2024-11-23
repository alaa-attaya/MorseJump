using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import for UI Text components
using TMPro;
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

    private void OnEnable()
    {
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
        if (morseCodeDisplay != null)
        {
            // Update the UI text with the Score and Morse code
            scoreDisplay.text =  $"SCORE\n{number}";
            morseCodeDisplay.text = $"MORSE CODE\n{morseCode}";
         
        }
    }
}
