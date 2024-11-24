using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class ClassicModeManager : MonoBehaviour
{
    public static ClassicModeManager Instance { get; private set; }

    public float initialGameSpeed = 8f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    public float gameSpeedMax = 22f;
    public bool isGameOver;

    private PlayerController player;
    private SpawnerManager spawner;

    public GameObject menuPanel;       // Reference to the Menu Panel
    public GameObject gameOverPanel;  // Reference to the Game Over Panel

    public Button yesButton;           // Reference to the "Yes" button (Menu)
    public Button noButton;            // Reference to the "No" button (Close menu panel)
    public Button closeButton;         // Reference to the "Close" button
    public Button retryButton;         // Reference to the "Retry" button
    public Button menuButton;          // Reference to the "Menu" button (Game Over)

    // Audio references
    private AudioSource audioSource;       // Central AudioSource
    public AudioClip classicModeClip;      // Background music for Classic Mode
    public AudioClip menuClip;             // Audio for Menu

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start()
    {
        // Ensure panels are initially inactive
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<SpawnerManager>();

        // Set up audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign button listeners for the in-game menu
        yesButton.onClick.AddListener(GoToMenu);
        noButton.onClick.AddListener(ClosePanel);
        closeButton.onClick.AddListener(ClosePanel);

        // Assign button listeners for the Game Over panel
        retryButton.onClick.AddListener(NewGame);
        menuButton.onClick.AddListener(GoToMenu);

        NewGame();
    }

    public void NewGame()
    {
        // Destroy existing obstacles
        ObstacleManager[] obstacles = FindObjectsOfType<ObstacleManager>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        // Reset game variables
        gameSpeed = initialGameSpeed;
        enabled = true;
        isGameOver = false;

        // Reactivate the player and spawner
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);

        // Hide Game Over panel
        gameOverPanel.SetActive(false);

        // Start Classic Mode audio
        PlayAudioClip(classicModeClip, true); // Loop classic mode music
    }

    public void GameOver()
    {
        // Set the game over state
        isGameOver = true;
        gameSpeed = 0f;
        enabled = false;

        // Disable the player and spawner
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        // Show Game Over panel
        gameOverPanel.SetActive(true);

        // Immediately start Menu audio in a loop
        PlayAudioClip(menuClip, true);
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Increase game speed over time
            gameSpeed = Mathf.Min(gameSpeed + gameSpeedIncrease * Time.deltaTime, gameSpeedMax);
        }

        // Listen for Escape key (Windows) or Back button (Android)
        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the menu is currently active
            if (menuPanel.activeSelf)
            {
                ClosePanel();  // If active, close it
            }
            else
            {
                ToggleMenuPanel();  // If not active, show it
            }
        }
    }

    void ToggleMenuPanel()
    {
        // Toggle the active state of the menu panel
        menuPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;

        // Play Menu audio if the menu panel is open
        PlayAudioClip(menuClip, true); // Loop Menu audio
    }

    void ClosePanel()
    {
        // Close the menu panel and resume the game
        menuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (audioSource.clip == menuClip)
        {
            audioSource.Stop(); // Stop Menu audio
        }
    }

    void GoToMenu()
    {
        // Resume game time before switching scenes
        Time.timeScale = 1f;

        // Load the menu scene
        SceneManager.LoadScene("MenuScene");
    }

    private void PlayAudioClip(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
