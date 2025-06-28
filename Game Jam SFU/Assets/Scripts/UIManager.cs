using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text hpText;

    public GameObject gameOverScreen;
    public TMP_Text finalScoreText;
    public TMP_Text finalComboText;
    public TMP_Text finalAccuracyText;

    private int score = 0;
    private int combo = 1;
    private int hp = 3;

    public void UpdateScore(int newScore)
    {
        score = newScore;
        scoreText.text = "Score: " + score;
    }

    public void UpdateCombo(int newCombo)
    {
        combo = newCombo;
        comboText.text = "Combo: x" + combo;
    }

    public void UpdateHP(int newHP)
    {
        hp = newHP;
        hpText.text = "HP: " + new string('♥', hp);
    }
    public void ShowGameOverScreen(int score, int maxCombo, int perfect, int good, int miss)
    {
        gameOverScreen.SetActive(true);

        finalScoreText.text = $"Final Score: {score}";
        finalComboText.text = $"Max Combo: x{maxCombo}";
        finalAccuracyText.text = $"Perfect: {perfect}\nGood: {good}\nMiss: {miss}";
    }
}
