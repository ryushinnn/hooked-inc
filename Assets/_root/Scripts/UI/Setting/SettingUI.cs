using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI {
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private Button _btnClose;
    
    private float _boardRectExpandedSize = 1044;
    private float _boardRectCollapsedSize = 146;
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
        _seq.Append(DOVirtual.Float(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack));
    }

    private void Close() {
        UIManager.CloseUI<SettingUI>();
    }
}