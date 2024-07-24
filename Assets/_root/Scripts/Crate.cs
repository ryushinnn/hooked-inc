using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using UnityEngine;
using Logger = Assassin.Utils.Logger;

public class Crate : MonoBehaviour {
    [SerializeField] private float _velocity;

    private Vector3 _destination;
    private Tween _tween;

    public void SetDestination(Vector3 dest) {
        _destination = dest;
        var dir = _destination - transform.position;
        var time = dir.magnitude / _velocity;
        _tween = transform.DOMove(_destination, time).OnComplete(() => {
            ObjectPool.DestroyObject(gameObject);
        }).SetEase(Ease.Linear);
    }
    
    public void OnPicked() {
        _tween?.Kill();
        ObjectPool.DestroyObject(gameObject);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, _destination);
    }
#endif
}
