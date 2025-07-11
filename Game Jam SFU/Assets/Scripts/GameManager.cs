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

    void Start()
    {
        // Initialize the HP system and UI at game start
        RhythmJudge.ResetStats();
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Game Over!");
        
        // Stop pipe spawning
        if (pipeSpawner != null)
            pipeSpawner.StopSpawning();
        
        // Get final stats from RhythmJudge
        var stats = RhythmJudge.GetFinalStats();
        
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
        
        // Use MenuManager if available
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.ShowGameOver(
                stats.score,
                stats.maxCombo,
                stats.perfectCount,
                stats.goodCount,
                stats.missCount
            );
        }
        else
        {
            // Fallback to simple game over UI
            Time.timeScale = 0f;
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