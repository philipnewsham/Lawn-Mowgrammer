using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ArrowInfo
{
    public Color color;
    public float distance;
}

public class ArrowController : MonoBehaviour
{
    private Color[] colors = new Color[7] { Color.red, Color.yellow, Color.green, Color.blue, Color.white, Color.black, Color.gray };
    private int arrowCount;

    public static ArrowController controller;

    private void Awake()
    {
        if (controller == null)
            controller = this;
    }

    public ArrowInfo ReturnInfo()
    {
        ArrowInfo info = new ArrowInfo()
        {
            color = colors[arrowCount],
            distance = (arrowCount % 4) * 8.0f
        };
        arrowCount = (arrowCount + 1) % colors.Length;
        return info;
    }
}
