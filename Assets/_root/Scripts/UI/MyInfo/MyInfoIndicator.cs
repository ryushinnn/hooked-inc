using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyInfoIndicator : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private bool _allowVelocityCheck;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _arrow;
    
    private float _collapsedSize = -1409;
    private float _expandedSize = -306;
    private float _minSize = -1651;
    private float _lastSize;

    private YieldInstruction _waitForEndOfFrame = new WaitForEndOfFrame();
    private float _snapDuration = 0.3f;
    private Vector2 _beginDragPosition;
    private Vector2 _dragPosition;
    private Sequence _snapSeq;
    private Tween _scrollTwn;

    private float _expectedVelocity = 500;
    private float _swipeTimeLimit = 0.1f;
    private float _beginDragTime;

    private void OnEnable() {
        _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, _collapsedSize);
        _arrow.localRotation = Quaternion.Euler(0, 0, 180);
        LockScrollRect(true);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _snapSeq?.Kill();
        _beginDragPosition = eventData.position;
        _beginDragTime = Time.unscaledTime;
        var curSize = _boardRect.sizeDelta.y;
        _lastSize = Mathf.Abs(_collapsedSize - curSize) < Mathf.Abs(_expandedSize - curSize) ? _collapsedSize : _expandedSize;
    }
    
    public void OnDrag(PointerEventData eventData) {
        _dragPosition = eventData.position;
        var dist = _beginDragPosition.y - _dragPosition.y;
        _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, Mathf.Max(_minSize, _lastSize + dist));
    }

    public void OnEndDrag(PointerEventData eventData) {
        var curSize = _boardRect.sizeDelta.y;
        var swipeTime = Time.unscaledTime - _beginDragTime;
        var vel = Mathf.Abs(curSize - _lastSize) / swipeTime;
        ALog.Log("vel: " + vel + " swipe time: " + swipeTime);
        
        if (_allowVelocityCheck && vel > _expectedVelocity && swipeTime < _swipeTimeLimit) {
            // determine which side to snap by velocity
            var nearCollapsedThreshold = Mathf.Abs(_collapsedSize - curSize) < Mathf.Abs(_expandedSize - curSize);
            var expanded = (nearCollapsedThreshold && _collapsedSize < curSize) || _expandedSize < curSize;
            _snapSeq = DOTween.Sequence();
            _snapSeq.Append(DOVirtual.Float(curSize, expanded ? _expandedSize : _collapsedSize, _snapDuration, 
                value => {
                    _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
                }).SetEase(Ease.OutBack));
            _arrow.localRotation = Quaternion.Euler(0, 0, expanded ? 0 : 180);
            LockScrollRect(!expanded);
        } else {
            // determine which side to snap by distance
            var expanded = curSize > (_expandedSize + _collapsedSize) / 2;
            _snapSeq = DOTween.Sequence();
            _snapSeq.Append(DOVirtual.Float(curSize, expanded ? _expandedSize : _collapsedSize, _snapDuration, 
                value => {
                    _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
                }).SetEase(Ease.OutBack));
            _arrow.localRotation = Quaternion.Euler(0, 0, expanded ? 0 : 180);
            LockScrollRect(!expanded);
        }
    }

    private void LockScrollRect(bool val) {
        _scrollRect.vertical = !val;
        if (val) StartCoroutine(DoScrollToTop());
    }

    IEnumerator DoScrollToTop() {
        yield return _waitForEndOfFrame;
        var curVPos = _scrollRect.verticalNormalizedPosition;
        _scrollTwn?.Kill();
        _scrollTwn = DOVirtual.Float(curVPos, 1, _snapDuration, value => {
            _scrollRect.verticalNormalizedPosition = value;
        });
    }
}
