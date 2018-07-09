using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeToChildren : MonoBehaviour
{
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

	public void UpdateHeight()
    {
        float height = 0.0f;
        foreach (RectTransform child in rect)
            height += child.sizeDelta.y;

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
	}
}
