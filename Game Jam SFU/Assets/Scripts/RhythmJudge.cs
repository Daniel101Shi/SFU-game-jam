using UnityEngine;
public static class RhythmJudge{

    public static void EvulateTiming (float distance, RhythmPrompt prompt) {
        if (distance < 0.05f) {}
        else if (distance < 0.05f) {
            Score("Good", 2);
        }
        else if (distance < 0.1f) {
            Score("Okay", 1);
        }
        else {
            Score("Miss", 0);
        }
        prompt.CleanUp();
    }
    public static void Missed (RhythmPrompt prompt) {
        Score("Miss", 0);
        prompt.CleanUp();
    }

    private static void Score(string grade, int multiplier) {
        Debug.Log($"Rhythm Input: " + grade);

    }

}