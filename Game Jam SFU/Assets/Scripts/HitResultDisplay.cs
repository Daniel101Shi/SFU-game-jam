// Assets/Scripts/HitResultDisplay.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class HitResultDisplay : MonoBehaviour
{
    [Header("Display Settings")]
    public TMP_Text resultText;
    public float displayDuration = 1f;
    public float fadeSpeed = 2f;
    public Vector3 floatDirection = Vector3.up;
    public float floatDistance = 50f;

    [Header("Colors")]
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Color originalColor;
    private bool isAnimating = false;

    void Awake()
    {
        if (resultText == null)
            resultText = GetComponentInChildren<TMP_Text>();
    }

    public void ShowResult(string result, Vector3 worldPosition)
    {
        if (isAnimating) return;

        // Convert world position to screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        transform.position = screenPos;

        // Set text and color based on result
        resultText.text = result;
        Color resultColor = GetColorForResult(result);
        resultText.color = resultColor;
        originalColor = resultColor;

        // Set up animation positions
        startPosition = transform.position;
        targetPosition = startPosition + (floatDirection.normalized * floatDistance);

        // Start animation
        gameObject.SetActive(true);
        StartCoroutine(AnimateResult());
    }

    private Color GetColorForResult(string result)
    {
        switch (result.ToLower())
        {
            case "perfect":
                return perfectColor;
            case "good":
                return goodColor;
            case "miss":
                return missColor;
            default:
                return Color.white;
        }
    }

    private IEnumerator AnimateResult()
    {
        isAnimating = true;
        float elapsedTime = 0f;

        while (elapsedTime < displayDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time in case game is paused
            float progress = elapsedTime / displayDuration;

            // Float upward
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            // Fade out
            Color currentColor = originalColor;
            currentColor.a = Mathf.Lerp(1f, 0f, progress * fadeSpeed);
            resultText.color = currentColor;

            yield return null;
        }

        // Animation complete
        isAnimating = false;
        gameObject.SetActive(false);
    }

    // Method to reset the display for reuse
    public void ResetDisplay()
    {
        if (isAnimating)
        {
            StopAllCoroutines();
            isAnimating = false;
        }
        
        gameObject.SetActive(false);
        Color resetColor = originalColor;
        resetColor.a = 1f;
        resultText.color = resetColor;
    }
}