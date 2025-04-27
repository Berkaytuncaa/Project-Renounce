using UnityEngine;
using TMPro;
using System.Collections;

public class RegularShow : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text[] splashTexts;  // Array of TMP_Text components for splash screens
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip splashSFX;
    
    [Header("Timing")]
    [SerializeField] private float revealDelay = 0.5f;    // Delay before each reveal
    [SerializeField] private float displayDuration = 2f;   // How long each splash stays visible
    [SerializeField] private float fadeSpeed = 2f;         // Speed of fade in/out

    void Start()
    {
        // Hide all texts initially
        foreach (TMP_Text text in splashTexts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        }

        // Start the splash sequence
        StartCoroutine(PlaySplashSequence());
    }

    private IEnumerator PlaySplashSequence()
    {
        // Wait a bit before starting
        yield return new WaitForSeconds(revealDelay);

        // Play sequence for each splash text
        for (int i = 0; i < splashTexts.Length; i++)
        {
            // Play sound effect
            if (audioSource != null && splashSFX != null)
            {
                audioSource.PlayOneShot(splashSFX);
            }

            // Fade in
            yield return StartCoroutine(FadeText(splashTexts[i], true));

            // Wait for display duration
            yield return new WaitForSeconds(displayDuration);

            // Fade out
            yield return StartCoroutine(FadeText(splashTexts[i], false));

            // Wait before next splash
            if (i < splashTexts.Length - 1)
            {
                yield return new WaitForSeconds(revealDelay);
            }
        }
    }

    private IEnumerator FadeText(TMP_Text text, bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f;
        Color currentColor = text.color;

        while (!Mathf.Approximately(currentColor.a, targetAlpha))
        {
            float newAlpha = Mathf.MoveTowards(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            currentColor.a = newAlpha;
            text.color = currentColor;
            yield return null;
        }
    }
}