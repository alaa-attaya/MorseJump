using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-1)]
public class ClassicModeManager : MonoBehaviour
{   public static ClassicModeManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    public float gameSpeedMax = 20f;
    private PlayerController player;
    private SpawnerManager spawner;
    public GameObject menuPanel;   // Reference to the Menu Panel
    public Button yesButton;       // Reference to the "Yes" button
    public Button noButton;        // Reference to the "No" button
    public Button closeButton;     // Reference to the "Close" button
        private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    void Start()
    {   
        // Ensure the menu panel is initially inactive
        menuPanel.SetActive(false);
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<SpawnerManager>();
        // Assign button listeners
        yesButton.onClick.AddListener(GoToMenu);
        noButton.onClick.AddListener(ClosePanel);
        closeButton.onClick.AddListener(ClosePanel);
         NewGame();
    }
      public void NewGame()
    {
       
      ObstacleManager[] obstacles = FindObjectsOfType<ObstacleManager>();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

      
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
     
    }

    
     public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
  

       
    }


    void Update()
    {      gameSpeed = Mathf.Min(gameSpeed + gameSpeedIncrease * Time.deltaTime, gameSpeedMax);
        // Listen for Escape key (Windows) or Back button (Android)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenuPanel();
        }
    }

    void ToggleMenuPanel()
    {
        // Toggle the active state of the menu panel
        bool isActive = menuPanel.activeSelf;
        menuPanel.SetActive(!isActive);

        // Pause or resume the game depending on panel state
        Time.timeScale = isActive ? 1f : 0f; // 1f = resume, 0f = pause
    }

    void GoToMenu()
    {
        // Resume game time before switching scenes
        Time.timeScale = 1f;

        // Load the menu scene
        SceneManager.LoadScene("MenuScene");
    }

    void ClosePanel()
    {
        // Close the menu panel and resume the game
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
