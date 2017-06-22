using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectController : MonoBehaviour 
{
    public float xAspect = 9.0f;
    public float yAspect = 16.0f;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = CalcAspect(xAspect, yAspect);
        camera.rect = rect;
    }

    private Rect CalcAspect(float width, float height)
    {
        float targetAspect = width / height;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

        if (1.0f > scaleHeight)
        {
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            rect.width = 1.0f;
            rect.height = scaleHeight;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0.0f;
            rect.width = scaleWidth;
            rect.height = 1.0f;
        }
        return rect;
    }
}
