using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RhythmPrompt : MonoBehaviour 
{
    [Header("Visual Components")]
    public Image outerCircle;
    public Image targetCircle;
    
    [Header("Animation Settings")]
    public float duration = 1f;
    public AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0f, 2f, 1f, 1f);
    public bool enablePulse = true;
    public float pulseIntensity = 0.1f;
    public float pulseSpeed = 8f;
    
    [Header("Visual Feedback")]
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;
    public float feedbackDuration = 0.3f;
    
    private float timer = 0f;
    private bool isActive = true;
    private Vector3 originalTargetScale;
    private Color originalOuterColor;
    private Color originalTargetColor;
    private bool hasBeenHit = false;

    void Start() 
    {
        // Store original values
        originalTargetScale = targetCircle.transform.localScale;
        originalOuterColor = outerCircle.color;
        originalTargetColor = targetCircle.color;
        
        // Set initial outer circle scale
        outerCircle.transform.localScale = Vector3.one * shrinkCurve.Evaluate(0f);
    }
    
    void Update() 
    {
        if (!isActive || hasBeenHit) return;
        
        timer += Time.deltaTime;
        float normalizedTime = timer / duration;
        
        // Smooth shrinking animation using curve
        float currentScale = shrinkCurve.Evaluate(normalizedTime);
        outerCircle.transform.localScale = Vector3.one * currentScale;
        
        // Target circle pulse effect
        if (enablePulse)
        {
            float pulseScale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
            targetCircle.transform.localScale = originalTargetScale * pulseScale;
        }
        
        // Color transition as it approaches target
        float proximityAlpha = Mathf.Clamp01((2f - currentScale) / 1f); // Fade in as it gets closer
        Color outerColor = originalOuterColor;
        outerColor.a = 0.7f + (proximityAlpha * 0.3f); // Increase opacity as it gets closer
        outerCircle.color = outerColor;

        // Check for input
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) 
        {
            HandleInput(currentScale);
        }

        // Check for missed input
        if (normalizedTime >= 1f) 
        {
            HandleMiss();
        }
    }
    
    private void HandleInput(float currentScale)
    {
        if (hasBeenHit) return;
        
        hasBeenHit = true;
        isActive = false;
        
        float distance = Mathf.Abs(currentScale - 1f);
        
        // Determine result and trigger visual feedback
        string result = GetResultFromDistance(distance);
        StartCoroutine(ShowInputFeedback(result));
        
        // Send to rhythm judge
        RhythmJudge.EvaluateTiming(distance, this);
    }
    
    private void HandleMiss()
    {
        if (hasBeenHit) return;
        
        hasBeenHit = true;
        isActive = false;
        
        StartCoroutine(ShowInputFeedback("Miss"));
        RhythmJudge.Missed(this);
    }
    
    private string GetResultFromDistance(float distance)
    {
        if (distance < 0.05f) return "Perfect";
        if (distance < 0.1f) return "Good";
        return "Miss";
    }
    
    private IEnumerator ShowInputFeedback(string result)
    {
        Color feedbackColor = GetFeedbackColor(result);
        
        // Flash effect
        float flashTimer = 0f;
        while (flashTimer < feedbackDuration)
        {
            flashTimer += Time.deltaTime;
            float flashIntensity = Mathf.PingPong(flashTimer * 10f, 1f);
            
            // Apply feedback color with flash
            outerCircle.color = Color.Lerp(originalOuterColor, feedbackColor, flashIntensity);
            targetCircle.color = Color.Lerp(originalTargetColor, feedbackColor, flashIntensity * 0.5f);
            
            yield return null;
        }
        
        // Quick scale effect for good/perfect hits
        if (result != "Miss")
        {
            Vector3 targetScale = targetCircle.transform.localScale;
            Vector3 expandedScale = targetScale * 1.3f;
            
            float scaleTime = 0.15f;
            float scaleTimer = 0f;
            
            while (scaleTimer < scaleTime)
            {
                scaleTimer += Time.deltaTime;
                float scaleProgress = scaleTimer / scaleTime;
                
                if (scaleProgress < 0.5f)
                {
                    // Expand
                    targetCircle.transform.localScale = Vector3.Lerp(targetScale, expandedScale, scaleProgress * 2f);
                }
                else
                {
                    // Contract
                    targetCircle.transform.localScale = Vector3.Lerp(expandedScale, targetScale, (scaleProgress - 0.5f) * 2f);
                }
                
                yield return null;
            }
        }
        
        // Cleanup after delay
        yield return new WaitForSeconds(0.1f);
        CleanUp();
    }
    
    private Color GetFeedbackColor(string result)
    {
        switch (result)
        {
            case "Perfect": return perfectColor;
            case "Good": return goodColor;
            case "Miss": return missColor;
            default: return Color.white;
        }
    }

    public void CleanUp() 
    {
        Destroy(gameObject);
    }
    
    // Method to manually trigger cleanup (for external calls)
    public void ForceCleanup()
    {
        StopAllCoroutines();
        CleanUp();
    }
}