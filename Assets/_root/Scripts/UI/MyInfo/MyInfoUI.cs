using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoUI : UI {
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private Button _btnClose;

    private Vector2 _boardRectExpandedSize = new(1080, -1409);
    private Vector2 _boardRectCollapsedSize = new(560, -1915);
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _seq;

    private void Awake() {
        _btnClose.onClick.AddListener(Close);
    }

    public override void OnOpen(params object[] prs) {
        gameObject.SetActive(true);
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.ExceptSide);
        Expand();
    }

    public override void OnClose() {
        gameObject.SetActive(false);
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.All);
    }

    private void Expand() {
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(DOVirtual.Vector2(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = value;
            }).SetEase(Ease.OutBack));
    }

    private void Close() {
        UIManager.CloseUI<MyInfoUI>();
    }
}
