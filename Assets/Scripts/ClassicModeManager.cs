using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;  // Add the TextMeshPro namespace
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
    

    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;  // TextMeshPro reference for "Your Score" on game over screen
    public TextMeshProUGUI highScoreText; 
    public Button yesButton;
    public Button noButton;
    public Button closeButton;
    public Button retryButton;
    public Button menuButton;

    private AudioSource audioSource;
    public AudioClip classicModeClip;
    public AudioClip menuClip;

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
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<SpawnerManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        yesButton.onClick.AddListener(GoToMenu);
        noButton.onClick.AddListener(ClosePanel);
        closeButton.onClick.AddListener(ClosePanel);

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
        
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
     
    
        // Reset game variables
        gameSpeed = initialGameSpeed;
        enabled = true;
        isGameOver = false;

    

        gameOverPanel.SetActive(false);

        PlayAudioClip(classicModeClip, true);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        gameOverPanel.SetActive(true);

        // Get the current score from SpawnerManager
        int currentScore = spawner.GetScore();

        // Check if the current score is higher than the high score and update if necessary
        if (HighScoreManager.Instance != null)
        {  
            int highScore = HighScoreManager.Instance.GetHighScore();
            if (currentScore > highScore)
            {   highScoreText.text = $"HIGH SCORE\n{currentScore}";
                HighScoreManager.Instance.UpdateHighScore(currentScore);  // Update the high score
            }
        }

        // Set the "Your Score" text on the Game Over screen
        scoreText.text = $"YOUR SCORE\n{currentScore}";

        PlayAudioClip(menuClip, true);
    }

    void Update()
    {
        if (!isGameOver)
        {
            gameSpeed = Mathf.Min(gameSpeed + gameSpeedIncrease * Time.deltaTime, gameSpeedMax);
        }

        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.activeSelf)
            {
                ClosePanel();
            }
            else
            {
                ToggleMenuPanel();
            }
        }
    }

    void ToggleMenuPanel()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
        PlayAudioClip(menuClip, true);
    }

    void ClosePanel()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;

        if (audioSource.clip == menuClip)
        {
            audioSource.Stop();
        }
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    private void PlayAudioClip(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
