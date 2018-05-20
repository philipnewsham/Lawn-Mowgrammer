using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateIn : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float start;
    private float end;

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        end = 0.0f;
        start = (Screen.width / 2.0f) + (rectTransform.sizeDelta.x / 2.0f);
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        float lerpTime = 0.0f;
        while(lerpTime < 1.0f)
        {
            rectTransform.localPosition = new Vector2(Mathf.Lerp(start, end, lerpTime), 0.0f);
            canvasGroup.alpha = lerpTime;
            lerpTime += Time.deltaTime * 2.0f;
            yield return null;
        }
        rectTransform.localPosition = new Vector2(end, 0.0f);
        canvasGroup.alpha = 1.0f;
    }
}
