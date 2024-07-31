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

    public bool Extended => _extended;

    private bool _extended;
    private float _rectExtendedPosX = -35;
    private float _rectCollapsedPosX = 148;
    private float _rectAnimationDuration = 0.25f;
    private Sequence _extendOfCollapseSeq;

    private void Awake() {
        _btnMission.onClick.AddListener(OpenDailyMission);
        
        ExtendOrCollapse(true, true);
    }

    public void ExtendOrCollapse(bool extend, bool ignoreAnimation) {
        if (!_rects.IsNotEmpty()) {
            foreach (Transform child in transform) {
                _rects.Add(child.GetComponent<RectTransform>());
            }
        }

        _extended = extend;
        _extendOfCollapseSeq?.Kill();
        _extendOfCollapseSeq = DOTween.Sequence();
        for (int i = 0; i < _rects.Count; i++) {
            var rect = _rects[i];
            _extendOfCollapseSeq.Join(DOVirtual.Float(
                _extended ? _rectCollapsedPosX : _rectExtendedPosX,
                _extended ? _rectExtendedPosX : _rectCollapsedPosX,
                _rectAnimationDuration, value => {
                    rect.anchoredPosition = new Vector2(value, rect.anchoredPosition.y);
                }));
        }
    }
    
    private void OpenDailyMission() {
        UIManager.OpenUI<DailyMissionUI>();
    }
}
