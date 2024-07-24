using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSizeCamera : MonoBehaviour
{
    [SerializeField] private Vector2 _defaultScreen = new Vector2(1080, 1920);

    private void Awake()
    {
        var ratioScale = (1.0f * Screen.width / Screen.height) / (_defaultScreen.x / _defaultScreen.y);
        ratioScale = ratioScale < 1 ? 1 : ratioScale;

        GetComponent<Camera>().orthographicSize /= ratioScale;
    }
}