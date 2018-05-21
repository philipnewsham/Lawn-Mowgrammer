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
        Vector2 startPosition = new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y - rectTransform.sizeDelta.y);
        Vector2 endPosition = new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y);
        StartCoroutine(Animation(startPosition, endPosition));
    }

    IEnumerator Animation(Vector2 start, Vector2 end)
    {
        float lerpTime = 0.0f;
        while(lerpTime < 1.0f)
        {
            rectTransform.localPosition = Vector2.Lerp(start, end, lerpTime);
            canvasGroup.alpha = lerpTime;
            lerpTime += Time.deltaTime * 2.0f;
            yield return null;
        }
        rectTransform.localPosition = end;
        canvasGroup.alpha = 1.0f;
    }
}
