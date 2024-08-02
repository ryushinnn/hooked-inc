using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : UI {
    [SerializeField] private Transform _widgetParent;
    [SerializeField] private Button _btnClose;
    [SerializeField] private CollectionDetail _detail;
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private RectTransform _mapRect;
    [SerializeField] private Button _btnNavLeft;
    [SerializeField] private Button _btnNavRight;

    private List<CollectionWidget> _widgets = new();
    private Vector2 _boardRectExpandedSize = new(1041, -305);
    private Vector2 _boardRectCollapsedSize = new(251, -1270);
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _boardSeq;
    
    private float _mapRectExpandedSize = -324;
    private float _mapRectCollapsedSize = -924;
    private float _mapRectAnimationDuration = 0.5f;
    private Sequence _mapSeq;

    private void Awake() {
        _btnClose.onClick.AddListener(Close);
        _btnNavLeft.onClick.AddListener(ChangeMap);
        _btnNavRight.onClick.AddListener(ChangeMap);
        
        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out CollectionWidget widget)) {
                _widgets.Add(widget);
                widget.GetComponent<Button>().onClick.AddListener(OpenDetail);
            }
        }
    }

    public override void Open(params object[] prs) {
        gameObject.SetActive(true);
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.ProfileAndCurrency);
        Expand();
    }

    public override void Close() {
        gameObject.SetActive(false);
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.All);
    }

    [Button]
    private void Expand() {
        _boardSeq?.Kill();
        _boardSeq = DOTween.Sequence();
        _boardSeq.Append(DOVirtual.Vector2(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = value;
            }).SetEase(Ease.OutBack));
    }

    private void ChangeMap() {
        _mapSeq?.Kill();
        _mapSeq = DOTween.Sequence();
        _mapSeq.Append(DOVirtual.Float(_mapRectCollapsedSize, _mapRectExpandedSize, _mapRectAnimationDuration,
            value => {
                _mapRect.sizeDelta = new Vector2(value, _mapRect.sizeDelta.y);
            }).SetEase(Ease.OutBack));
    }
    
    private void OpenDetail() {
        _detail.gameObject.SetActive(true);
    }
}
