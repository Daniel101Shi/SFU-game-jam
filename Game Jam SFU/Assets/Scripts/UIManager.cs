using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text hpText;

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
}
