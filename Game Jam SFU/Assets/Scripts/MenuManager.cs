// Assets/Scripts/MenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject gameplayUI;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    
    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button quitButton;
    
    [Header("Game Over Buttons")]
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Pause Buttons")]
    public Button resumeButton;
    public Button pauseMenuButton;
    
    [Header("Game Over Stats")]
    public TMP_Text gameOverScoreText;
    public TMP_Text gameOverComboText;
    public TMP_Text gameOverAccuracyText;
    
    [Header("Settings")]
    public string gameSceneName = "Game";
    public string menuSceneName = "MainMenu";
    
    private bool isGamePaused = false;
    private bool isGameStarted = false;

    void Start()
    {
        SetupButtons();
        ShowMainMenu();
    }
    
    void Update()
    {
        // Pause functionality
        if (isGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    private void SetupButtons()
    {
        // Main Menu buttons
        if (playButton != null)
            playButton.onClick.AddListener(StartGame);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        // Game Over buttons
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            
        // Pause buttons
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (pauseMenuButton != null)
            pauseMenuButton.onClick.AddListener(ReturnToMainMenu);
    }
    
    public void ShowMainMenu()
    {
        SetActivePanel(mainMenuPanel);
        Time.timeScale = 1f;
        isGameStarted = false;
        isGamePaused = false;
    }
    
    public void StartGame()
    {
        SetActivePanel(gameplayUI);
        Time.timeScale = 1f;
        isGameStarted = true;
        isGamePaused = false;
        
        // Initialize game systems
        if (GameManager.Instance != null)
        {
            RhythmJudge.ResetStats();
        }
        
        // Play start sound
        AudioManager.Instance?.PlaySound("GameStart");
    }
    
    public void ShowGameOver(int score, int maxCombo, int perfect, int good, int miss)
    {
        SetActivePanel(gameOverPanel);
        Time.timeScale = 0f;
        isGameStarted = false;
        
        // Update game over stats
        if (gameOverScoreText != null)
            gameOverScoreText.text = $"Final Score: {score}";
        if (gameOverComboText != null)
            gameOverComboText.text = $"Max Combo: x{maxCombo}";
        if (gameOverAccuracyText != null)
        {
            int total = perfect + good + miss;
            float accuracy = total > 0 ? ((float)(perfect + good) / total) * 100f : 0f;
            gameOverAccuracyText.text = $"Perfect: {perfect}\nGood: {good}\nMiss: {miss}\nAccuracy: {accuracy:F1}%";
        }
        
        // Play game over sound
        AudioManager.Instance?.PlaySound("GameOver");
    }
    
    public void PauseGame()
    {
        if (!isGameStarted) return;
        
        SetActivePanel(pausePanel);
        Time.timeScale = 0f;
        isGamePaused = true;
        
        AudioManager.Instance?.PlaySound("Pause");
    }
    
    public void ResumeGame()
    {
        if (!isGamePaused) return;
        
        SetActivePanel(gameplayUI);
        Time.timeScale = 1f;
        isGamePaused = false;
        
        AudioManager.Instance?.PlaySound("Resume");
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        
        // If we're in the same scene, just show main menu
        if (mainMenuPanel != null)
        {
            ShowMainMenu();
        }
        else
        {
            // Load main menu scene
            SceneManager.LoadScene(menuSceneName);
        }
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    private void SetActivePanel(GameObject activePanel)
    {
        // Hide all panels
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        
        // Show active panel
        if (activePanel != null) activePanel.SetActive(true);
    }
}