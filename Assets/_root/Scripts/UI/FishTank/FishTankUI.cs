using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FishTankUI : UI {
    [SerializeField] private Transform _widgetParent;
    [SerializeField] private Button _btnClose;
    [SerializeField] private FishTankDetail _detail;
    [SerializeField] private RectTransform _boardRect;

    private List<FishTankWidget> _widgets = new();
    private float _boardRectCollapsedSize = -1964;
    private float _boardRectExpandedSize = -517;
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _seq;

    private void Awake() {
        _btnClose.onClick.AddListener(Close);
        
        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out FishTankWidget widget)) {
                _widgets.Add(widget);
                widget.GetComponent<Button>().onClick.AddListener(OpenDetail);
            }
        }
    }
    
    public override void Open(params object[] prs) {
        gameObject.SetActive(true);
        UIManager.GetUI<HomeUI>()?.Collapse(false);
        Expand();
    }

    public override void Close() {
        gameObject.SetActive(false);
        UIManager.GetUI<HomeUI>()?.Expand();
    }

    private void Expand() {
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(DOVirtual.Float(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack));
    }
    
    private void OpenDetail() {
        _detail.gameObject.SetActive(true);
    }
}
