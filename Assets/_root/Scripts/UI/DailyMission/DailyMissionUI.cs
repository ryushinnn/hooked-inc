using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionUI : UI {
    [SerializeField] private Transform _widgetParent;
    [SerializeField] private Button _btnClose;
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private ScrollRect _scrollRect;

    private List<DailyMissionWidget> _widgets = new();
    private float _boardRectCollapsedSize = 200;
    private float _boardRectExpandedSize = 1392;
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _expandSeq;
    private YieldInstruction _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Awake() {
        _btnClose.onClick.AddListener(Close);

        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out DailyMissionWidget widget)) {
                _widgets.Add(widget);
            }
        }
    }

    public override void Open(params object[] prs) {
        base.Open(prs);
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.ExceptSide);
        Expand();
        ScrollToTop();
    }

    public override void Close() {
        base.Close();
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.All);
    }

    private void Expand() {
        _expandSeq?.Kill();
        _expandSeq = DOTween.Sequence();
        _expandSeq.Append(DOVirtual.Float(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack));
    }

    private void ScrollToTop() {
        StartCoroutine(DoScrollToTop());
    }

    IEnumerator DoScrollToTop() {
        yield return _waitForEndOfFrame;
        _scrollRect.verticalNormalizedPosition = 1;
    }
}
