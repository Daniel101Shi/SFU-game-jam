public GameObject rhythmPromptPrefab;
public Transform promptParent;

public void OnPipePassed()
{
    Instantiate(rhythmPromptPrefab, promptParent);
}