// Assets/Scripts/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("UI References")]
    public UIManager ui;
    
    [Header("Game Objects")]
    public PipeSpawner pipeSpawner;
    public GameObject gameOverUI;
    
    [Header("Rhythm System")]
    public GameObject rhythmPromptPrefab;
    public Transform promptParent;
    
    [Header("Scene Management")]
    [SerializeField]
    private string gameSceneName = "Game";

    private void Awake()
    {
        Instance = this;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Game Over!");
        
        // Stop pipe spawning
        if (pipeSpawner != null)
            pipeSpawner.StopSpawning();
        
        // Halt game time
        Time.timeScale = 0f;
        
        // Get final stats from RhythmJudge if available
        if (RhythmJudge.Instance != null)
        {
            var stats = RhythmJudge.Instance.GetFinalStats();
            
            // Show Game Over screen with stats
            if (ui != null)
            {
                ui.ShowGameOverScreen(
                    stats.score,
                    stats.maxCombo,
                    stats.perfectCount,
                    stats.goodCount,
                    stats.missCount
                );
            }
        }
        else
        {
            // Fallback to simple game over UI if no rhythm system
            if (gameOverUI != null)
                gameOverUI.SetActive(true);
        }
    }

    public void OnPipePassed()
    {
        if (rhythmPromptPrefab != null && promptParent != null)
        {
            Instantiate(rhythmPromptPrefab, promptParent);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void Restart()
    {
        RestartGame(); // Alias for compatibility
    }

    public void Quit()
    {
        Application.Quit();
    }
}