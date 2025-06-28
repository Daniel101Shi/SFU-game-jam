// Assets/Scripts/HitResultManager.cs
using UnityEngine;
using System.Collections.Generic;

public class HitResultManager : MonoBehaviour
{
    public static HitResultManager Instance;

    [Header("Prefab Reference")]
    public GameObject hitResultPrefab; // Assign your HitResult prefab here

    [Header("Pool Settings")]
    public int poolSize = 5;

    private Queue<HitResultDisplay> resultPool = new Queue<HitResultDisplay>();
    private List<HitResultDisplay> activeResults = new List<HitResultDisplay>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializePool()
    {
        // Create pool of result displays
        for (int i = 0; i < poolSize; i++)
        {
            GameObject resultObj = Instantiate(hitResultPrefab, transform);
            HitResultDisplay resultDisplay = resultObj.GetComponent<HitResultDisplay>();
            
            if (resultDisplay == null)
            {
                Debug.LogError("HitResultManager: hitResultPrefab must have HitResultDisplay component!");
                continue;
            }

            resultObj.SetActive(false);
            resultPool.Enqueue(resultDisplay);
        }
    }

    public void ShowHitResult(string result, Vector3 worldPosition)
    {
        HitResultDisplay display = GetAvailableDisplay();
        if (display != null)
        {
            display.ShowResult(result, worldPosition);
            activeResults.Add(display);
        }
    }

    private HitResultDisplay GetAvailableDisplay()
    {
        // Try to get from pool first
        if (resultPool.Count > 0)
        {
            return resultPool.Dequeue();
        }

        // If pool is empty, try to reuse an inactive one
        for (int i = activeResults.Count - 1; i >= 0; i--)
        {
            if (!activeResults[i].gameObject.activeInHierarchy)
            {
                HitResultDisplay display = activeResults[i];
                activeResults.RemoveAt(i);
                display.ResetDisplay();
                return display;
            }
        }

        // If none available, create a new one
        GameObject newResultObj = Instantiate(hitResultPrefab, transform);
        HitResultDisplay newDisplay = newResultObj.GetComponent<HitResultDisplay>();
        
        if (newDisplay == null)
        {
            Debug.LogError("HitResultManager: Failed to create new HitResultDisplay!");
            Destroy(newResultObj);
            return null;
        }

        return newDisplay;
    }

    // Call this method when a result animation finishes to return it to pool
    public void ReturnToPool(HitResultDisplay display)
    {
        if (activeResults.Contains(display))
        {
            activeResults.Remove(display);
            display.ResetDisplay();
            resultPool.Enqueue(display);
        }
    }

    // Clean up on scene change
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}