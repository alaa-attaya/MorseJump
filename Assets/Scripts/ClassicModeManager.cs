using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClassicModeManager : MonoBehaviour
{
    public GameObject menuPanel;   // Reference to the Menu Panel
    public Button yesButton;       // Reference to the "Yes" button
    public Button noButton;        // Reference to the "No" button
    public Button closeButton;     // Reference to the "Close" button

    void Start()
    {
        // Ensure the menu panel is initially inactive
        menuPanel.SetActive(false);

        // Assign button listeners
        yesButton.onClick.AddListener(GoToMenu);
        noButton.onClick.AddListener(ClosePanel);
        closeButton.onClick.AddListener(ClosePanel);
    }

    void Update()
    {
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
