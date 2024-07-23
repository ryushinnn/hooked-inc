using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour {
    [SerializeField] private float _velocityMin, _velocityMax;

    private Vector3 _destination;
    private Tween _tween;
    
    public void SetDestination(Vector3 dest) {
        _destination = dest;
        var dir = _destination - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        var dist = dir.magnitude;
        var time = dist / Random.Range(_velocityMin, _velocityMax);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        _tween = transform.DOMove(_destination, time).OnComplete(() => {
            ObjectPool.DestroyObject(gameObject);
        }).SetEase(Ease.Linear);
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
