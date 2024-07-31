using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class AquariumBoard : MonoBehaviour {
    [SerializeField] private Transform _widgetParent;
    [SerializeField] private Button _btnClose;
    
    public Action<AquariumWidget> OnWidgetSelected;
    public Action OnClosed;

    private List<AquariumWidget> _widgets = new();
    private RectTransform _boardRect;
    private float _boardRectCollapsedSize = -1970;
    private float _boardRectExpandedSize = -629;
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _expandSeq;
    
    private void Awake() {
        _btnClose.onClick.AddListener(Close);
        _boardRect = GetComponent<RectTransform>();
        
        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out AquariumWidget widget)) {
                _widgets.Add(widget);
                widget.GetComponent<Button>().onClick.AddListener(() => {
                    OnWidgetSelected?.Invoke(widget);
                });
            }
        }
    }

    private void OnEnable() {
        // ShowWidgets();
        UIManager.GetUI<HomeUI>()?.Collapse(false);
        Expand();
    }

    private void ShowWidgets() {
        // for (int i = 0; i < _widgets.Count; i++) {
        //     _widgets[i].PlayAppearAnimation(0.1f * i);
        // }
    }

    [Button]
    private void Expand() {
        _expandSeq?.Kill();
        _expandSeq = DOTween.Sequence();
        _expandSeq.Append(DOVirtual.Float(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack));
    }

    private void Close() {
        OnClosed?.Invoke();
    }
}
