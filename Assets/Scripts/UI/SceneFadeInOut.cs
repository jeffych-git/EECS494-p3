using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;  // Reference to the Canvas Group for fading
    public float fadeDuration = 2f;      // Duration of the fade-in effect


    private void Start()
    {
        // Ensure the game is paused on load
        Time.timeScale = 0;
        // Start with the fade canvas fully opaque
        fadeCanvasGroup.alpha = 1;
        // Start the fade-in coroutine after a brief delay (to ensure canvas setup)
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;

        // Gradually reduce the alpha value of the Canvas Group
        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = 1 - (timer / fadeDuration);
            timer += Time.unscaledDeltaTime; // Use unscaled time to work even when paused
            yield return null;
        }

        // Ensure the Canvas Group is fully transparent at the end
        fadeCanvasGroup.alpha = 0;

        // Resume the game
        Time.timeScale = 1;
    }
}


/*using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;  // Reference to the Canvas Group for fading
    public float fadeDuration = 2f;      // Duration of the fade effect
    public bool fadeInOnStart = true;    // Toggle for fade-in on load
    public bool fadeOutOnExit = false;   // Toggle for fade-out before exiting

    private void Awake()
    {
        // Ensure the game is paused on load if fade-in is enabled
        if (fadeInOnStart)
        {
            Time.timeScale = 0;
            fadeCanvasGroup.alpha = 1; // Start with the canvas fully opaque
        }
        else
        {
            fadeCanvasGroup.alpha = 0; // Start fully transparent if not fading in
        }
    }

    private void Start()
    {
        // Start the fade-in effect if enabled
        if (fadeInOnStart)
        {
            StartCoroutine(Fade(true));
        }
    }

    public void TriggerFadeOutAndExit()
    {
        if (fadeOutOnExit)
        {
            StartCoroutine(Fade(false));
        }
    }

    private IEnumerator Fade(bool fadeIn)
    {
        float timer = 0f;
        float startAlpha = fadeIn ? 1 : 0;
        float endAlpha = fadeIn ? 0 : 1;

        // Adjust the alpha based on fade-in or fade-out
        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            timer += Time.unscaledDeltaTime; // Use unscaled time to work even when paused
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha; // Ensure final alpha is set correctly

        // Unpause after fade-in, and potentially handle scene transition after fade-out
        if (fadeIn)
        {
            Time.timeScale = 1; // Resume game after fade-in
        }
        else
        {
            // Do any additional actions here after fade-out, like loading a new scene
            // SceneManager.LoadScene("NextScene");
        }
    }
}
*/