using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnderwaterFish : MonoBehaviour {
    [SerializeField] private float _velocityMin, _velocityMax;
    [SerializeField] private Animator _animator;

    private Vector3 _destination;
    private Tween _tween;
    private int _paramSpeed = Animator.StringToHash("Speed");
    
    public void SetDestination(Vector3 dest, bool fixedVel) {
        _destination = dest;
        var dir = _destination - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var dist = dir.magnitude;
        var vel = fixedVel ? (_velocityMin + _velocityMax) / 2 : Random.Range(_velocityMin, _velocityMax);
        var time = dist / vel;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        _tween = transform.DOMove(_destination, time).OnComplete(() => {
            ObjectPool.DestroyObject(gameObject);
        }).SetEase(Ease.Linear);
        _animator.SetFloat(_paramSpeed, vel);
    }

    public void OnCaught() {
        _tween?.Kill();
        ObjectPool.DestroyObject(gameObject);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, _destination);
    }
#endif
}
