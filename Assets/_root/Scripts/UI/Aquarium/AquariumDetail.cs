using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AquariumDetail : MonoBehaviour {
    [SerializeField] private Button _btnCancel;
    [SerializeField] private Button _btnPurchase;
    [SerializeField] private RectTransform _dialogRect;
    [SerializeField] private Image _imgBg;
    [SerializeField] private Button _btnBg;
    [SerializeField] private SlidableDialog _slidableDialog;

    private float _dialogRectCollapsedSize = 0;
    private float _dialogRectExtendedSize = 1048;
    private float _dialogRectAnimationDuration = 0.5f;
    private float _bgOpacity = 0.8f;
    private Sequence _extendOrCollapseSeq;

    private void Awake() {
        _btnCancel.onClick.AddListener(Cancel);
        _btnPurchase.onClick.AddListener(Purchase);
        _btnBg.onClick.AddListener(Cancel);
        _slidableDialog.OnCollapsed += Close;
    }

    private void OnEnable() {
        ExtendOrCollapse(true);
    }

    private void ExtendOrCollapse(bool extend, Action onComplete = null) {
        _extendOrCollapseSeq?.Kill();
        _extendOrCollapseSeq = DOTween.Sequence();
        if (extend) {
            _imgBg.color = new Color(0, 0, 0, 0);
            _extendOrCollapseSeq.Append(DOVirtual.Float(_dialogRectCollapsedSize, _dialogRectExtendedSize,
                _dialogRectAnimationDuration, value => {
                    _dialogRect.sizeDelta = new Vector2(_dialogRect.sizeDelta.x, value);
                }).SetEase(Ease.OutBack))
                .Join(_imgBg.DOFade(_bgOpacity, _dialogRectAnimationDuration));
        } else {
            _imgBg.color = new Color(0, 0, 0, _bgOpacity);
            _extendOrCollapseSeq.Append(DOVirtual.Float(_dialogRectExtendedSize, _dialogRectCollapsedSize,
                _dialogRectAnimationDuration, value => {
                    _dialogRect.sizeDelta = new Vector2(_dialogRect.sizeDelta.x, value);
                }).SetEase(Ease.InBack).OnComplete(() => {
                    onComplete?.Invoke();
                }))
                .Join(_imgBg.DOFade(0, _dialogRectAnimationDuration));
        }
    }

    private void Cancel() {
        // ???
        ExtendOrCollapse(false, Close);
    }

    private void Purchase() {
        // ???
        ExtendOrCollapse(false, Close);
    }

    private void Close() {
        gameObject.SetActive(false);
    }
}
