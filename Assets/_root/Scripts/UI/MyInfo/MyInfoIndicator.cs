using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyInfoIndicator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private RectTransform _boardRect;
    
    private float _collapsedSize = -1409;
    private float _expandedSize = -306;
    private float _minSize = -1651;

    private float _snapDuration = 0.3f;
    private Vector2 _beginDragPosition;
    private Vector2 _dragPosition;
    private Sequence _seq;
    
    public void OnBeginDrag(PointerEventData eventData) {
        _seq.Kill();
        _beginDragPosition = eventData.position;
    }
    
    public void OnDrag(PointerEventData eventData) {
        _dragPosition = eventData.position;
        var dist = _dragPosition.y - _beginDragPosition.y;
        _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, Mathf.Max(_minSize, _collapsedSize - dist));
    }

    public void OnEndDrag(PointerEventData eventData) {
        var curSize = _boardRect.sizeDelta.y;
        _seq = DOTween.Sequence();
        var expanded = curSize > (_expandedSize + _collapsedSize) / 2;
        _seq.Append(DOVirtual.Float(curSize, expanded ? _expandedSize : _collapsedSize, _snapDuration, 
            value => {
            _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
        }).SetEase(Ease.OutBack));
    }
}
