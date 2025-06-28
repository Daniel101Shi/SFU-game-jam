public GameObject rhythmPromptPrefab;
public Transform promptParent;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager ui;

    void Awake()
    {
        Instance = this;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Game Over!");
        // Add game over handling logic here
    }
    public void OnPipePassed()
    {
        Instantiate(rhythmPromptPrefab, promptParent);
    }
}
