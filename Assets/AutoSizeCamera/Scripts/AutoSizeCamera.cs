using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSizeCamera : MonoBehaviour
{
    [SerializeField] private Vector2 defaultScreen = new Vector2(576, 1024);
    [SerializeField] private Camera camera;

    private void Awake()
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();
        }

        var ratioScale = (1.0f * Screen.width / Screen.height) / (defaultScreen.x / defaultScreen.y);
        ratioScale = ratioScale > 1 ? 1 : ratioScale;

        camera.orthographicSize /= ratioScale;
    }
}