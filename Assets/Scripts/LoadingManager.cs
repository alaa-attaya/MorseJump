using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public TextMeshProUGUI LoadingText;   // Reference to the UI Text component for the loading message
    public float typingSpeed = 0.05f; // Speed of the typewriter effect
    public float fadeDuration = 0.5f; // Duration for fade in and fade out
    public float pauseDuration = 1f; // Pause duration before the next loop

    private string message = "MORSEJUMP"; // Message to display

    void Start()
    {
        // Start the loading process
        StartCoroutine(LoadMenuScene());
    }

    IEnumerator LoadMenuScene()
    {
        // Start the typewriter effect animation
        StartCoroutine(AnimateLoading());

        // Simulate loading time
        yield return new WaitForSeconds(3f); // Adjust delay as needed

        // Load the Menu Scene
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator AnimateLoading()
    {
        while (true) // Repeat the animation infinitely
        {
            // Typewriter effect: Reveal the message one character at a time
            yield return StartCoroutine(TypewriterEffect());

            // Fade the text out and then fade it back in
            yield return StartCoroutine(FadeText(0f)); // Fade out
            yield return StartCoroutine(FadeText(1f)); // Fade in
            yield return StartCoroutine(FadeText(0f)); // Fade out
            yield return StartCoroutine(FadeText(1f)); // Fade in
            yield return new WaitForSeconds(pauseDuration); // Pause before restarting the effect
        }
    }

    IEnumerator TypewriterEffect()
    {
        LoadingText.text = ""; // Start with an empty text

        foreach (char character in message)
        {
            LoadingText.text += character; // Add one character at a time
            yield return new WaitForSeconds(typingSpeed); // Wait for the typing speed
        }
    }

    // Fade the text to a target alpha value (0 for fade out, 1 for fade in)
    IEnumerator FadeText(float targetAlpha)
    {
        float startAlpha = LoadingText.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            Color newColor = LoadingText.color;
            newColor.a = alpha;
            LoadingText.color = newColor;
            yield return null;
        }

        // Ensure the final alpha is set correctly
        Color finalColor = LoadingText.color;
        finalColor.a = targetAlpha;
        LoadingText.color = finalColor;
    }
}
