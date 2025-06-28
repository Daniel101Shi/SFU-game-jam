public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager ui;
    public GameObject rhythmPromptPrefab;
    public Transform promptParent;

    private void Awake()
    {
        Instance = this;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Game Over!");

        // Halt game time
        Time.timeScale = 0f;

        // Get final stats from RhythmJudge
        var stats = RhythmJudge.GetFinalStats();

        // Show Game Over screen
        ui.ShowGameOverScreen(
            stats.score,
            stats.maxCombo,
            stats.perfectCount,
            stats.goodCount,
            stats.missCount
        );
    }

    public void OnPipePassed()
    {
        Instantiate(rhythmPromptPrefab, promptParent);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
