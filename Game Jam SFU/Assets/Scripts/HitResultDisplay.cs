using UnityEngine;
using TMPro;
using System.Collections;

public class HitResultDisplay : MonoBehaviour
{
    public TMP_Text hitText;
    public float displayDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = hitText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = hitText.gameObject.AddComponent<CanvasGroup>();
        }
        hitText.text = "";
        canvasGroup.alpha = 0f;
    }

    public void ShowHitResult(string result)
    {
        StopAllCoroutines();
        hitText.text = result;
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(displayDuration);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - t;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
