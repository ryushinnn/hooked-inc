using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class SafeAreaException : MonoBehaviour {
    private Vector2 _defaultScreen = new(1080, 1920);
    private RectTransform _panel;
    
    private void Awake() {
        ApplyFullArea();
    }

    private void ApplyFullArea() {
        _panel = GetComponent<RectTransform>();
        StartCoroutine(DoApplyFullArea());
    }

    IEnumerator DoApplyFullArea() {
        yield return new WaitForEndOfFrame();
        FixPosition();
        FixSizeDelta();
    }
    
    private void FixPosition() {
        _panel.position = Vector3.zero;
    }

    private void FixSizeDelta() {
        var defRatio = _defaultScreen.y / _defaultScreen.x;
        var scrRatio = (float)Screen.height / Screen.width;
        if (defRatio < scrRatio) {
            _panel.sizeDelta = new Vector2(_defaultScreen.x, _defaultScreen.x * scrRatio);
        } else {
            _panel.sizeDelta = new Vector2(_defaultScreen.y / scrRatio, _defaultScreen.y);
        }
    }
}
