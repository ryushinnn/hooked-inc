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
    
    public bool Expanded => _expanded;

    private bool _expanded;
    private float _avtRectExpandedPosX = 65;
    private float _avtRectCollapsedPosX = -153;
    private float _lvAndXpRectExpandedPosX = 257;
    private float _lvAndXpRectCollapsedPosX = -151;
    private float _animationDuration = 0.5f;
    private Sequence _seq;

    private void Awake() {
        ExpandOrCollapse(true, true);
    }

    private void OnEnable() {
        MessageDispatcher<GameEvent.OnAvatarChanged>.AddListener(SetAvatar);
        MessageDispatcher<GameEvent.OnXpChanged>.AddListener(SetLevelAndXp);
    }

    private void OnDisable() {
        MessageDispatcher<GameEvent.OnAvatarChanged>.RemoveListener(SetAvatar);
        MessageDispatcher<GameEvent.OnXpChanged>.RemoveListener(SetLevelAndXp);
    }

    public void ExpandOrCollapse(bool extend, bool ignoreAnimation) {
        _expanded = extend;
        _seq?.Kill();
        
        if (ignoreAnimation) {
            _avtRect.anchoredPosition = new Vector2(_expanded ? _avtRectExpandedPosX : _avtRectCollapsedPosX, _avtRect.anchoredPosition.y);
            _lvRect.anchoredPosition = new Vector2(_expanded ? _lvAndXpRectExpandedPosX : _lvAndXpRectCollapsedPosX, _lvRect.anchoredPosition.y);
            _xpRect.anchoredPosition = new Vector2(_expanded ? _lvAndXpRectExpandedPosX : _lvAndXpRectCollapsedPosX, _xpRect.anchoredPosition.y);
            return;
        }
        
        _seq = DOTween.Sequence();
        if (_expanded) {
            _seq.Append(DOVirtual.Float(_avtRectCollapsedPosX, _avtRectExpandedPosX, _animationDuration,
                value => {
                    _avtRect.anchoredPosition = new Vector2(value, _avtRect.anchoredPosition.y);
                }).SetEase(Ease.OutCubic))
                .Join(DOVirtual.Float(_lvAndXpRectCollapsedPosX, _lvAndXpRectExpandedPosX, _animationDuration,
                    value => {
                        _lvRect.anchoredPosition = new Vector2(value, _lvRect.anchoredPosition.y);
                        _xpRect.anchoredPosition3D = new Vector2(value, _xpRect.anchoredPosition.y);
                    }).SetEase(Ease.OutBack));
        } else {
            _seq.Append(DOVirtual.Float(_avtRectExpandedPosX, _avtRectCollapsedPosX, _animationDuration/2,
                value => {
                    _avtRect.anchoredPosition = new Vector2(value, _avtRect.anchoredPosition.y);
                }).SetEase(Ease.InCubic))
                .Join(DOVirtual.Float(_lvAndXpRectExpandedPosX, _lvAndXpRectCollapsedPosX, _animationDuration/2,
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
