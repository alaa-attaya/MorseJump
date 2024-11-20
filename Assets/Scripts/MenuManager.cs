using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // For Button handling
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    // References to the Quit Confirmation Panel and the Buttons
    public GameObject quitPanel;  // Reference to the Quit Confirmation Panel
    public Button quitButton;     // Reference to the Quit Button (in main menu)
    public Button noButton;       // Reference to the "No" button to close the panel
    public Button yesButton;      // Reference to the "Yes" button to quit the game
    public Button closeButton;    // Reference to the "X" button to close the panel

    void Start()
    {
        // Ensure quit panel is initially inactive
        quitPanel.SetActive(false);

        // Assign button listeners
        quitButton.onClick.AddListener(OnQuitClicked);
        noButton.onClick.AddListener(ClosePanel); // Close the panel on "No" button click
        closeButton.onClick.AddListener(ClosePanel); // Close the panel on "X" button click
        yesButton.onClick.AddListener(QuitGame); // Quit the game on "Yes" button click
    }

    public void LoadClassicMode()
    {
        SceneManager.LoadScene("ClassicModeScene");
    }

    // This function will open the confirmation panel when Quit is clicked
    void OnQuitClicked()
    {
        quitPanel.SetActive(true);  // Activate the confirmation panel
    }

    // Close the confirmation panel (when "No" or "X" is clicked)
    void ClosePanel()
    {
        quitPanel.SetActive(false); // Deactivate the confirmation panel
    }

    // Quit the game when "Yes" is clicked
    void QuitGame()
    {
        // For testing in Unity editor
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;  // Stop Play Mode
#else
        Application.Quit();  // Quit the game in a built version
#endif
    }
}
