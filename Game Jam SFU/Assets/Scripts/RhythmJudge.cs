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

    // Fixed method name: EvaluateTiming (was EvulateTiming)
    public static void EvaluateTiming(float distance, RhythmPrompt prompt)
    {
        if (distance < perfectThreshold)
        {
            Score("Perfect", 3);
        }
        else if (distance < goodThreshold)
        {
            Score("Good", 1);
        }
        else
        {
            Score("Miss", 0);
        }

        prompt.CleanUp();
    }

    public static void Missed(RhythmPrompt prompt)
    {
        Score("Miss", 0);
        prompt.CleanUp();
    }

    private static void Score(string grade, int basePoints)
    {
        if (grade == "Miss")
        {
            combo = 1;
            hp -= 1;
            missCount++;
            
            // Update UI moved to the end of the method
            
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

    // New method to update UI in real-time
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