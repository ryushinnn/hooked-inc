using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour {
    private RectTransform _panel;
    private Rect _lastSafeArea = new(0f, 0f, 0f, 0f);
    private ScreenOrientation _lastScreenOrientation = ScreenOrientation.AutoRotation;

    private void Awake() {
        _panel = GetComponent<RectTransform>();
    }

    private void Update() {
        Refresh();
    }

    private void Refresh() {
        if (_lastSafeArea != Screen.safeArea || _lastScreenOrientation != Screen.orientation) {
            ApplySafeArea(Screen.safeArea);
        }
    }

    private void ApplySafeArea(Rect safeArea) {
        _lastSafeArea = safeArea;
        _lastScreenOrientation = Screen.orientation;
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.y /= Screen.height;
        anchorMax.y /= Screen.height;

        _panel.anchorMin = new Vector2(_panel.anchorMin.x, anchorMin.y);
        _panel.anchorMax = new Vector2(_panel.anchorMax.x, anchorMax.y);
    }
}
