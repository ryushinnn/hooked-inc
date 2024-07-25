using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MovableObject {
    [SerializeField] private float _value;
    [SerializeField] private Transform _rotator;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Tween _tween;
    private int _paramSpeed = Animator.StringToHash("Speed");
    
    public override void SetDestination(Vector3 dest, bool fixedVel) {
        Destination = dest;
        var dir = Destination - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;
        var dist = dir.magnitude;
        var vel = GetVelocity(fixedVel);
        var time = dist / vel;
        _rotator.rotation = Quaternion.Euler(0, 0, angle);
        _tween = transform.DOMove(Destination, time).OnComplete(OnDisappear).SetEase(Ease.Linear);
        _animator.SetFloat(_paramSpeed, vel);
    }

    public override void OnCaught() {
        _tween?.Kill();
        OnDisappear();
        Inventory.Instance().ReceiveMoney(_value);
        MessageDispatcher<MessageID.OnFloatingTextRequested>.Trigger.Invoke($"+${_value:N0}", transform.position);
    }
    
    protected override void OnDisappear() {
        ObjectPool.DestroyObject(gameObject);
    }
    
    public Sprite GetFishSprite() {
        return _spriteRenderer.sprite;
    }
}
