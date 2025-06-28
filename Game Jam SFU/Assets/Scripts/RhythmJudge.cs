using UnityEngine;

public static class RhythmJudge
{
    private static int score = 0;
    private static int combo = 1;
    private static int hp = 3;

    private const float perfectThreshold = 0.05f;
    private const float goodThreshold = 0.1f;

    public static void EvulateTiming(float distance, RhythmPrompt prompt)
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
        Debug.Log($"Rhythm Input: {grade}");

        if (grade == "Miss")
        {
            combo = 1;
            hp -= 1;
            if (hp <= 0)
            {
                GameManager.Instance.OnPlayerDeath();
            }
        }
        else
        {
            score += basePoints * combo;
            if (grade == "Perfect")
                combo++;
        }

        // Update UI
        UIManager ui = GameManager.Instance.ui;
        ui.UpdateScore(score);
        ui.UpdateCombo(combo);
        ui.UpdateHP(hp);
    }
}