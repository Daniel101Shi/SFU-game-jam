using UnityEngine;
using UnityEngine.UI;

public class RhythmPrompt : MonoBehaviour 
{
    public Image outerCircle;
    public Image targetCircle;
    public float duration = 1f;
    
    private float timer = 0f;
    private bool isActive = true; // Added missing field

    void Start() 
    {
        outerCircle.transform.localScale = Vector3.one * 2f;
    }
    
    void Update() 
    {
        if (!isActive) return; // Don't update if no longer active
        
        timer += Time.deltaTime;
        float t = timer / duration;
        float scale = Mathf.Lerp(2f, 1f, t);
        outerCircle.transform.localScale = new Vector3(scale, scale, 1f);

        // Check if outer circle is clicked
        if (Input.GetMouseButtonDown(0)) 
        {
            float distance = Mathf.Abs(scale - 1f);
            RhythmJudge.EvaluateTiming(distance, this); // Fixed method name
            isActive = false;
        }

        // Check for missed input
        if (timer >= duration) 
        {
            RhythmJudge.Missed(this);
            isActive = false;
        }
    }

    public void CleanUp() 
    {
        Destroy(gameObject);
    }
}