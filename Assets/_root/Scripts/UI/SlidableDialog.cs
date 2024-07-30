using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlidableDialog : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    public Action OnCollapsed;
    
    [SerializeField] private float _extendedSize;
    [SerializeField] private float _collapsedSize;
    [SerializeField] private Image _imgBg;

    private float _snapDuration = 0.3f;
    private RectTransform _rect;
    private Vector2 _beginDragPosition;
    private Vector2 _dragPosition;
    private Sequence _snapSeq;

    private void Awake() {
        _rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _snapSeq?.Kill();
        _beginDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) {
        _dragPosition = eventData.position;
        var dist = _dragPosition.y - _beginDragPosition.y;
        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _extendedSize + dist);
    }
    
    public void OnEndDrag(PointerEventData eventData) {
        var curSize = _rect.sizeDelta.y;
        _snapSeq = DOTween.Sequence();
        if (curSize > (_extendedSize + _collapsedSize) / 2) {
            _snapSeq.Append(DOVirtual.Float(curSize, _extendedSize, _snapDuration, value => {
                _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack));
        } else {
            _snapSeq.Append(DOVirtual.Float(curSize, _collapsedSize, _snapDuration, value => {
                _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, value);
            }))
            .Join(_imgBg.DOFade(0, _snapDuration))
            .AppendCallback(() => {
                OnCollapsed?.Invoke();
            });
        }
    }
}
