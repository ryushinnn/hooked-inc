using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EliteFish : MovableObject {
    [SerializeField] private float _value;
    [SerializeField] private Transform _rotator;
    [SerializeField] private float _maxHp;
    [SerializeField] private Image _imgHpFill;
    
    [SerializeField, ReadOnly] private float _currentHp;

    private Tween _tween;

    private void Awake() {
        ResetHp();
    }

    public override void SetDestination(Vector3 dest, bool fixedVel) {
        Destination = dest;
        var dir = Destination - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;
        var dist = dir.magnitude;
        var time = dist / GetVelocity(fixedVel);
        _rotator.rotation = Quaternion.Euler(0, 0, angle);
        _tween = transform.DOMove(Destination, time).OnComplete(OnDisappear).SetEase(Ease.Linear);
    }
    
    public override void OnCaught() {
        _tween?.Kill();
        OnDisappear();
        Inventory.Instance().ReceiveMoney(_value);
        MessageDispatcher<MessageID.OnFloatingTextRequested>.Trigger.Invoke($"+${_value:N0}", transform.position);
    }
    
    protected override void OnDisappear() {
        ObjectPool.DestroyObject(gameObject);
        GameManager.Instance().OnEliteDisappeared();
    }

    public void ResetHp() {
        _currentHp = _maxHp;
        _imgHpFill.fillAmount = 1;
    }

    public void TakeDamage(float dmg) {
        _currentHp -= dmg;
        _imgHpFill.fillAmount = _currentHp / _maxHp;
        if (_currentHp <= 0) {
            OnCaught();
        }
    }
}
