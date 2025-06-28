using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject pipePrefab;           // Assign your Pipe prefab here
    public float initialSpawnInterval = 2f; // Time between spawns at start
    public float minGapSize = 2f;           // Minimum vertical gap
    public float maxGapSize = 3.5f;         // Maximum vertical gap
    public float spawnXPosition = 10f;      // X position where pipes appear

    [Header("Difficulty Scaling")]
    public float spawnIntervalDecrease = 0.01f;  // How much to shrink interval per spawn
    public float minSpawnInterval = 1f;          // Hard cap on spawn frequency
    public float gapShrinkRate = 0.005f;         // How much to shrink gap size per spawn
    public float minGapSizeLimit = 1.5f;         // Hard cap on minimum gap

    private float currentSpawnInterval;
    private float currentGapSize;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentGapSize = maxGapSize;
        StartCoroutine(SpawnPipesRoutine());
    }

    private IEnumerator SpawnPipesRoutine()
    {
        while (true)
        {
            SpawnPipePair();
            
            // Scale difficulty
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecrease);
            currentGapSize = Mathf.Max(minGapSizeLimit, currentGapSize - gapShrinkRate);

            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnPipePair()
    {
        // Random vertical center position within a safe range
        float centerY = Random.Range(-1f, 1f);

        // Calculate top and bottom positions
        float halfGap = currentGapSize * 0.5f;
        Vector3 topPos = new Vector3(spawnXPosition, centerY + halfGap, 0f);
        Vector3 bottomPos = new Vector3(spawnXPosition, centerY - halfGap, 0f);

        // Instantiate pipes
        Instantiate(pipePrefab, topPos, Quaternion.Euler(0, 0, 180));  // flipped pipe
        Instantiate(pipePrefab, bottomPos, Quaternion.identity);
    }
}
