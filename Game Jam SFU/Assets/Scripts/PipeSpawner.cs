// Assets/Scripts/PipeSpawner.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject pipePrefab;
    public float initialSpawnInterval = 2f;
    public float minGapSize = 2f;
    public float maxGapSize = 3.5f;
    public float spawnXPosition = 10f;

    [Header("Difficulty Scaling")]
    public float spawnIntervalDecrease = 0.01f;
    public float minSpawnInterval = 1f;
    public float gapShrinkRate = 0.005f;
    public float minGapSizeLimit = 1.5f;

    private float currentSpawnInterval;
    private float currentGapSize;
    private List<GameObject> pipePool = new List<GameObject>();
    private bool isSpawning = true;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentGapSize = maxGapSize;

        // Preload pool
        for (int i = 0; i < 5; i++)
        {
            var p = Instantiate(pipePrefab);
            p.SetActive(false);
            pipePool.Add(p);
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

        var top = GetPipeFromPool();
        top.transform.position = topPos;
        top.transform.rotation = Quaternion.Euler(0, 0, 180);
        top.SetActive(true);

        var bottom = GetPipeFromPool();
        bottom.transform.position = bottomPos;
        bottom.transform.rotation = Quaternion.identity;
        bottom.SetActive(true);
    }

    private GameObject GetPipeFromPool()
    {
        if (pipePrefab == null)
        {
            Debug.LogError("PipeSpawner: pipePrefab is not assigned. Please assign a pipe prefab in the Inspector.");
            return null;
        }
        foreach (var pipe in pipePool)
            if (!pipe.activeInHierarchy)
                return pipe;

        var newPipe = Instantiate(pipePrefab);
        newPipe.SetActive(false);
        pipePool.Add(newPipe);
        return newPipe;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}
