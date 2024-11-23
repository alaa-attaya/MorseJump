using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool isSpawning = false; // Flag to track if we are in the middle of spawning

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

            // Spawn obstacles based on the Morse code sequence
            foreach (char symbol in morseCode)
            {
                // Spawn the appropriate obstacle for dot or dash
                SpawnObstacle(symbol);
                // Wait for the time between obstacles in the sequence
                yield return new WaitForSeconds(morseCodeSpeed);
            }

            // Increment the number (it will now continue to 10, 11, 12, etc.)
            currentNumber++;

            // After each Morse code sequence, wait for a random spawn rate before starting again
            yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));
        }
    }

    private string NumberToMorseCode(int number)
    {
        string morseCode = "";

        // Convert the number to Morse code by breaking it into digits
        // Convert each digit to its Morse code equivalent
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
            // If the symbol is a dot, pick a prefab from the dotPrefabs array based on spawn chance
            obstaclePrefab = GetRandomPrefabWithChance(dotPrefabs);
        }
        else if (morseSymbol == '-')
        {
            // If the symbol is a dash, pick a prefab from the dashPrefabs array based on spawn chance
            obstaclePrefab = GetRandomPrefabWithChance(dashPrefabs);
        }

        // Instantiate the obstacle at the current position if a prefab is selected
        if (obstaclePrefab != null)
        {
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.transform.position += transform.position; // Adjust spawn position based on the manager's position
        }
    }

    private GameObject GetRandomPrefabWithChance(SpawnableObject[] prefabs)
    {
        // Randomly select a prefab based on the spawn chance
        float randomValue = Random.value; // Get a random value between 0 and 1
        float cumulativeChance = 0f;

        // Loop through the prefabs and compare the random value with the spawn chances
        foreach (var spawnable in prefabs)
        {
            cumulativeChance += spawnable.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                // If the random value is less than or equal to the cumulative chance, spawn this prefab
                return spawnable.prefab;
            }
        }

        // If no prefab is selected (could happen if spawnChance values don't sum to 1), return null or fallback prefab
        return null;
    }
}
