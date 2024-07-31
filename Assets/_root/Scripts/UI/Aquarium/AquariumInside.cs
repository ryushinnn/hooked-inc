using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class AquariumInside : MonoBehaviour {
    [SerializeField] private Button _btnBack;
    [SerializeField] private RectTransform _fishRect;
    [SerializeField] private RectTransform _buttonsRect;
    [SerializeField] private Button _btnSelection;
    [SerializeField] private RectTransform _currencyRect;
    [SerializeField] private RectTransform _feedRect;
    [SerializeField] private Transform _arrow;
    
    public Action OnBacked;

    private bool _fishListShowing = false;
    private float _fishRectCollapsedSize = 174;
    private float _fishRectExpandedSize = 775;
    private float _fishRectAnimationDuration = 0.5f;
    private float _buttonsRectCollapedPosX = -420;
    private float _buttonsRectExpandedPosX = 45;
    private float _buttonsRectAnimationDuration = 0.25f;
    private Sequence _fishSeq;

    // private float _currencyRectCollapsedPosX = 550;
    // private float _currencyRectExtendedPosX = -50;
    private float _fishRectCollapsedPosY = -300;
    private float _fishRectExtendedPosY = 2.5f;
    private float _feedRectCollapsedPosY = -300;
    private float _feedRectExtendedPosY = 3.5f;
    private float _appearAnimationDuration = 0.25f;
    private Sequence _appearSeq;

    private void Awake() {
        _btnBack.onClick.AddListener(Back);
        _btnSelection.onClick.AddListener(() => ExpandOrCollapseFishRect());
    }

    private void OnEnable() {
        UIManager.GetUI<HomeUI>()?.Collapse(true);
        Appear();
        ExpandOrCollapseFishRect(false);
    }

    private void ExpandOrCollapseFishRect(bool? forcedValue = null) {
        _fishListShowing = forcedValue ?? !_fishListShowing;
        _fishSeq?.Kill();
        _fishSeq = DOTween.Sequence();

        if (_fishListShowing) {
            _arrow.rotation = Quaternion.Euler(0, 0, 180);
            _fishSeq.Append(DOVirtual.Float(_fishRectCollapsedSize, _fishRectExpandedSize,
                forcedValue.HasValue ? 0 : _fishRectAnimationDuration,
                value => {
                    _fishRect.sizeDelta = new Vector2(_fishRect.sizeDelta.x, value);
                }).SetEase(Ease.OutBack))
                .AppendCallback(() => {
                    _buttonsRect.gameObject.SetActive(true);
                })
                .Append(DOVirtual.Float(_buttonsRectCollapedPosX, _buttonsRectExpandedPosX,
                    forcedValue.HasValue ? 0 : _buttonsRectAnimationDuration,
                    value => {
                        _buttonsRect.anchoredPosition = new Vector2(value, _buttonsRect.anchoredPosition.y);
                    }));
        } else {
            _buttonsRect.anchoredPosition = new Vector2(_buttonsRectCollapedPosX, _buttonsRect.anchoredPosition.y);
            _buttonsRect.gameObject.SetActive(false);
            _arrow.rotation = Quaternion.Euler(0, 0, 0);
            _fishSeq.Append(DOVirtual.Float(_fishRectExpandedSize, _fishRectCollapsedSize,
                forcedValue.HasValue ? 0 : _fishRectAnimationDuration,
                value => {
                    _fishRect.sizeDelta = new Vector2(_fishRect.sizeDelta.x, value);
                }).SetEase(Ease.InBack));
        }
    }

    private void Appear() {
        _appearSeq?.Kill();
        _appearSeq = DOTween.Sequence();
        _appearSeq
                // .Append(DOVirtual.Float(_currencyRectCollapsedPosX, _currencyRectExtendedPosX, _appearAnimationDuration, 
                // value => {
                //     _currencyRect.anchoredPosition = new Vector2(value, _currencyRect.anchoredPosition.y);
                // }).SetEase(Ease.OutBack))
                .Join(DOVirtual.Float(_fishRectCollapsedPosY, _fishRectExtendedPosY, _appearAnimationDuration,
                value => {
                    _fishRect.anchoredPosition = new Vector2(_fishRect.anchoredPosition.x, value);
                }).SetEase(Ease.OutBack))
                .Join(DOVirtual.Float(_feedRectCollapsedPosY, _feedRectExtendedPosY, _appearAnimationDuration,
                value => {
                    _feedRect.anchoredPosition = new Vector2(_feedRect.anchoredPosition.x, value);
                }));
    }

    private void Back() {
        OnBacked?.Invoke();
    }
}
