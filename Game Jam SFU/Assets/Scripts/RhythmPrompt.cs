using UnityEngine;
using UnityEngine.UI;
using System;
    public Image outerCircle;
    public Image targetCircle;
    public float duration = 1f;
    private float timer = 0f;


public class RhythmPrompt : MonoBehaviour {
    void Start() {
        outerCircle.transform.localScale = Vector3.one * 2f;
        
    }
    
    void Update() {
        timer += Time.deltaTime;
        float t = timer / duration;
        float scale = Mathf.Lerp(2f, 1f, t);
        outerCircle.transform.localScale = new Vector3(scale, scale, 1f);

        // check if outer circle is clicked (dont forgot implement Evulate timing in RhythmJudge.cs)
        if (Input.GetMouseButtonDown(0)) {
        float distance = Mathf.Abs(scale - 1f);
        RhythmJudge.EvaluateTiming(distance, this);
        isActive = false;
        }

        // checks  missed input
        if (timer >= duration) {
            RhythmJudge.Missed(this);
            isActive = false;

        }
    }

    void CleanUp() {
        Destroy(gameObject)
    }
}