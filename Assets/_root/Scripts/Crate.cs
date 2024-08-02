using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using UnityEngine;

public class Crate : MovableObject {
    [SerializeField] private float _value;

    private Tween _tween;

    public override void SetDestination(Vector3 dest, bool fixedVel) {
        Destination = dest;
        var dir = Destination - transform.position;
        var time = dir.magnitude / GetVelocity(fixedVel);
        _tween = transform.DOMove(Destination, time).OnComplete(OnDisappear).SetEase(Ease.Linear);
    }
    
    public override void OnCaught() {
        _tween?.Kill();
        OnDisappear();
        Inventory.Instance().ReceiveMoney(_value);
        EventDispatcher<GameEvent.OnFloatingTextRequested>.Trigger.Invoke($"+${_value:N0}", transform.position);
    }

    protected override void OnDisappear() {
        ObjectPool.DestroyObject(gameObject);
    }
}
