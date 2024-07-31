using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeProfile : MonoBehaviour {
    [SerializeField] private RectTransform _avtRect;
    [SerializeField] private RectTransform _lvRect;
    [SerializeField] private RectTransform _xpRect;
    
    [BoxGroup("Player Info"), SerializeField] private Image _imgAvt;
    [BoxGroup("Player Info"), SerializeField] private TMP_Text _txtLevel;
    [BoxGroup("Player Info"), SerializeField] private Image _imgXp;
    
    public bool Extended => _extended;

    private bool _extended;
    private float _avtRectExtendedPosX = 65;
    private float _avtRectCollapsedPosX = -153;
    private float _lvAndXpRectExtendedPosX = 257;
    private float _lvAndXpRectCollapsedPosX = -151;
    private float _animationDuration = 0.5f;
    private Sequence _extendOrCollapseSeq;

    private void Awake() {
        ExtendOrCollapse(true, true);
    }

    private void OnEnable() {
        MessageDispatcher<GameEvent.OnAvatarChanged>.AddListener(SetAvatar);
        MessageDispatcher<GameEvent.OnXpChanged>.AddListener(SetLevelAndXp);
    }

    private void OnDisable() {
        MessageDispatcher<GameEvent.OnAvatarChanged>.RemoveListener(SetAvatar);
        MessageDispatcher<GameEvent.OnXpChanged>.RemoveListener(SetLevelAndXp);
    }

    public void ExtendOrCollapse(bool extend, bool ignoreAnimation) {
        _extended = extend;
        _extendOrCollapseSeq?.Kill();
        
        if (ignoreAnimation) {
            _avtRect.anchoredPosition = new Vector2(_extended ? _avtRectExtendedPosX : _avtRectCollapsedPosX, _avtRect.anchoredPosition.y);
            _lvRect.anchoredPosition = new Vector2(_extended ? _lvAndXpRectExtendedPosX : _lvAndXpRectCollapsedPosX, _lvRect.anchoredPosition.y);
            _xpRect.anchoredPosition = new Vector2(_extended ? _lvAndXpRectExtendedPosX : _lvAndXpRectCollapsedPosX, _xpRect.anchoredPosition.y);
            return;
        }
        
        _extendOrCollapseSeq = DOTween.Sequence();
        if (_extended) {
            _extendOrCollapseSeq.Append(DOVirtual.Float(_avtRectCollapsedPosX, _avtRectExtendedPosX, _animationDuration,
                value => {
                    _avtRect.anchoredPosition = new Vector2(value, _avtRect.anchoredPosition.y);
                }).SetEase(Ease.OutCubic))
                .Join(DOVirtual.Float(_lvAndXpRectCollapsedPosX, _lvAndXpRectExtendedPosX, _animationDuration,
                    value => {
                        _lvRect.anchoredPosition = new Vector2(value, _lvRect.anchoredPosition.y);
                        _xpRect.anchoredPosition3D = new Vector2(value, _xpRect.anchoredPosition.y);
                    }).SetEase(Ease.OutBack));
        } else {
            _extendOrCollapseSeq.Append(DOVirtual.Float(_avtRectExtendedPosX, _avtRectCollapsedPosX, _animationDuration/2,
                value => {
                    _avtRect.anchoredPosition = new Vector2(value, _avtRect.anchoredPosition.y);
                }).SetEase(Ease.InCubic))
                .Join(DOVirtual.Float(_lvAndXpRectExtendedPosX, _lvAndXpRectCollapsedPosX, _animationDuration/2,
                    value => {
                        _lvRect.anchoredPosition = new Vector2(value, _lvRect.anchoredPosition.y);
                        _xpRect.anchoredPosition = new Vector2(value, _xpRect.anchoredPosition.y);
                    }).SetEase(Ease.InBack));
        }
    }
    
    private void SetAvatar(Sprite spr) {
        _imgAvt.sprite = spr;
    }

    private void SetLevelAndXp(int lv, int curXp, int nextXp) {
        _txtLevel.text = lv.ToString();
        _imgXp.fillAmount = 1f * curXp / nextXp;
    }
}
