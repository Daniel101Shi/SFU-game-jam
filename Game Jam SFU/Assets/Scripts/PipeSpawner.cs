using System.Collections;
using System.Collections.Generic;
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
    private List<GameObject> pipePool = new List<GameObject>();
    private bool isSpawning = true;

    void Start()
    {
        if (pipePrefab == null)
        {
            Debug.LogError("Pipe prefab is not assigned!");
            return;
        }

        if (!pipePrefab.GetComponent<Collider2D>())
        {
            Debug.LogWarning("Pipe prefab is missing a Collider2D component!");
        }

        currentSpawnInterval = initialSpawnInterval;
        currentGapSize = maxGapSize;

        // Initialize the object pool
        int initialPoolSize = 5;
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject newPipe = Instantiate(pipePrefab);
            newPipe.SetActive(false);
            pipePool.Add(newPipe);
        }

        StartCoroutine(SpawnPipesRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnPipesRoutine()
    {
        while (isSpawning)
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
        float centerY = Random.Range(-1f, 1f);
        float halfGap = currentGapSize * 0.5f;
        Vector3 topPos = new Vector3(spawnXPosition, centerY + halfGap, 0f);
        Vector3 bottomPos = new Vector3(spawnXPosition, centerY - halfGap, 0f);

        GameObject topPipe = GetPipeFromPool();
        if (topPipe != null)
        {
            topPipe.transform.position = topPos;
            topPipe.transform.rotation = Quaternion.Euler(0, 0, 180);
            topPipe.SetActive(true);
        }

        GameObject bottomPipe = GetPipeFromPool();
        if (bottomPipe != null)
        {
            bottomPipe.transform.position = bottomPos;
            bottomPipe.transform.rotation = Quaternion.identity;
            bottomPipe.SetActive(true);
        }
    }

    private GameObject GetPipeFromPool()
    {
        if (pipePrefab == null)
        {
            Debug.LogError("Pipe prefab is not assigned in PipeSpawner!");
            return null;
        }

        foreach (GameObject pipe in pipePool)
        {
            if (!pipe.activeInHierarchy)
            {
                return pipe;
            }
        }

        // If no inactive pipes are available, instantiate a new one
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.SetActive(false);
        pipePool.Add(newPipe);
        return newPipe;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}