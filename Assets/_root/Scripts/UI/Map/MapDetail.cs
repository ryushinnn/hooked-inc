using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MapDetail : MonoBehaviour {
    [SerializeField] private Button _btnPlay;
    [SerializeField] private RectTransform _dialogRect;
    [SerializeField] private Image _imgBg;
    [SerializeField] private Button _btnBg;
    [SerializeField] private SlidableDialog _slidableDialog;
    
    private float _dialogRectCollapsedSize = 0;
    private float _dialogRectExpandedSize = 1710;
    private float _dialogRectAnimationDuration = 0.5f;
    private float _bgOpacity = 0.8f;
    private Sequence _seq;
    
    private void Awake() {
        _btnPlay.onClick.AddListener(Play);
        _btnBg.onClick.AddListener(() => ExpandOrCollapse(false, Close));
        _slidableDialog.OnCollapsed += Close;
    }

    private void OnEnable() {
        ExpandOrCollapse(true);
    }

    private void ExpandOrCollapse(bool expand, Action onComplete = null) {
        _seq?.Kill();
        _seq = DOTween.Sequence();
        if (expand) {
            _imgBg.color = new Color(0, 0, 0, 0);
            _seq.Append(DOVirtual.Float(_dialogRectCollapsedSize, _dialogRectExpandedSize,
                    _dialogRectAnimationDuration, value => {
                        _dialogRect.sizeDelta = new Vector2(_dialogRect.sizeDelta.x, value);
                    }).SetEase(Ease.OutBack))
                .Join(_imgBg.DOFade(_bgOpacity, _dialogRectAnimationDuration));
        } else {
            _imgBg.color = new Color(0, 0, 0, _bgOpacity);
            _seq.Append(DOVirtual.Float(_dialogRectExpandedSize, _dialogRectCollapsedSize,
                    _dialogRectAnimationDuration, value => {
                        _dialogRect.sizeDelta = new Vector2(_dialogRect.sizeDelta.x, value);
                    }).SetEase(Ease.InBack).OnComplete(() => {
                    onComplete?.Invoke();
                }))
                .Join(_imgBg.DOFade(0, _dialogRectAnimationDuration));
        }
    }
    
    private void Play() {
        // ???
        ExpandOrCollapse(false, Close);
    }

    private void Close() {
        gameObject.SetActive(false);
    }
}
