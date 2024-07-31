using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Extension;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HomeSide : MonoBehaviour {
    [BoxGroup("Button"), SerializeField] private Button _btnMission;
    
    private List<RectTransform> _rects = new();

    public bool Expanded => _expanded;

    private bool _expanded;
    private float _rectExpandedPosX = -35;
    private float _rectCollapsedPosX = 148;
    private float _rectAnimationDuration = 0.25f;
    private Sequence _seq;

    private void Awake() {
        _btnMission.onClick.AddListener(OpenDailyMission);
        
        ExpandOrCollapse(true, true);
    }

    public void ExpandOrCollapse(bool extend, bool ignoreAnimation) {
        if (!_rects.IsNotEmpty()) {
            foreach (Transform child in transform) {
                _rects.Add(child.GetComponent<RectTransform>());
            }
        }

        _expanded = extend;
        _seq?.Kill();
        _seq = DOTween.Sequence();
        for (int i = 0; i < _rects.Count; i++) {
            var rect = _rects[i];
            _seq.Join(DOVirtual.Float(
                _expanded ? _rectCollapsedPosX : _rectExpandedPosX,
                _expanded ? _rectExpandedPosX : _rectCollapsedPosX,
                _rectAnimationDuration, value => {
                    rect.anchoredPosition = new Vector2(value, rect.anchoredPosition.y);
                }));
        }
    }
    
    private void OpenDailyMission() {
        UIManager.OpenUI<DailyMissionUI>();
    }
}
