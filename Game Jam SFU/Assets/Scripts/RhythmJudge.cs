using UnityEngine;

public static class RhythmJudge
{
    private static int score = 0;
    private static int combo = 1;
    private static int maxCombo = 1;
    private static int hp = 3;

    private static int perfectCount = 0;
    private static int goodCount = 0;
    private static int missCount = 0;

    private const float perfectThreshold = 0.05f;
    private const float goodThreshold = 0.1f;

    public static void EvaluateTiming(float distance, RhythmPrompt prompt)
    {
        string result;
        int basePoints;

        if (distance < perfectThreshold)
        {
            result = "Perfect";
            basePoints = 3;
        }
        else if (distance < goodThreshold)
        {
            result = "Good";
            basePoints = 1;
        }
        else
        {
            result = "Miss";
            basePoints = 0;
        }

        // Show hit result at prompt position
        ShowHitResult(result, prompt.transform.position);
        
        // Process the score
        Score(result, basePoints);
        
        prompt.CleanUp();
    }

    public static void Missed(RhythmPrompt prompt)
    {
        // Show miss result at prompt position
        ShowHitResult("Miss", prompt.transform.position);
        
        Score("Miss", 0);
        prompt.CleanUp();
    }

    private static void ShowHitResult(string result, Vector3 position)
    {
        // Display the hit result using the manager
        if (HitResultManager.Instance != null)
        {
            HitResultManager.Instance.ShowHitResult(result, position);
        }
        else
        {
            Debug.LogWarning("HitResultManager not found! Make sure it's in the scene.");
        }
        
        // Play sound effect based on result
        if (AudioManager.Instance != null)
        {
            switch (result)
            {
                case "Perfect":
                    AudioManager.Instance.PlaySound("Perfect");
                    break;
                case "Good":
                    AudioManager.Instance.PlaySound("Good");
                    break;
                case "Miss":
                    AudioManager.Instance.PlaySound("Miss");
                    break;
            }
        }
    }

    private static void Score(string grade, int basePoints)
    {
        if (grade == "Miss")
        {
            combo = 1;
            hp -= 1;
            missCount++;
            
            // Update UI after processing miss
            UpdateUI();
            
            if (hp <= 0)
            {
                GameManager.Instance.OnPlayerDeath();
            }
        }
        else
        {
            score += basePoints * combo;
            if (grade == "Perfect")
            {
                perfectCount++;
                combo++;
            }
            else if (grade == "Good")
            {
                goodCount++;
            }

            if (combo > maxCombo)
                maxCombo = combo;
                
            // Update UI immediately
            UpdateUI();
        }
    }

    // Method to update UI in real-time
    private static void UpdateUI()
    {
        if (GameManager.Instance != null && GameManager.Instance.ui != null)
        {
            GameManager.Instance.ui.UpdateScore(score);
            GameManager.Instance.ui.UpdateCombo(combo);
            GameManager.Instance.ui.UpdateHP(hp);
        }
    }

    // Method to reset stats when game starts
    public static void ResetStats()
    {
        score = 0;
        combo = 1;
        maxCombo = 1;
        hp = 3;
        perfectCount = 0;
        goodCount = 0;
        missCount = 0;
        
        UpdateUI();
    }

    public struct RhythmStats
    {
        public int score;
        public int maxCombo;
        public int perfectCount;
        public int goodCount;
        public int missCount;
    }

    public static RhythmStats GetFinalStats()
    {
        return new RhythmStats
        {
            score = score,
            maxCombo = maxCombo,
            perfectCount = perfectCount,
            goodCount = goodCount,
            missCount = missCount
        };
    }

    // Getter methods for current values (useful for UI)
    public static int GetCurrentScore() => score;
    public static int GetCurrentCombo() => combo;
    public static int GetCurrentHP() => hp;
}