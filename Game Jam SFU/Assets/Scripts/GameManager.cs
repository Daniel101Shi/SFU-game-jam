// Assets/Scripts/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Dev1 References")]
    public PipeSpawner pipeSpawner;
    public GameObject gameOverUI;

    [Header("Dev2 Placeholder")]
    public GameObject rhythmPromptPrefab;
    public Transform promptParent;

    void Awake()
    {
        Instance = this;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Game Over!");
        pipeSpawner.StopSpawning();
        gameOverUI.SetActive(true);
    }

    public void OnPipePassed()
    {
        Instantiate(rhythmPromptPrefab, promptParent);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
