using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boat : MonoBehaviour {
    [SerializeField] private Transform _fisherman;
    [SerializeField] private Transform[] _standPositions;
    [SerializeField] private float _changePositionInterval;
    
    [SerializeField] private Transform _bucket;
    [SerializeField] private GameObject _fishPrefab;
    [SerializeField] private AnimationCurve _collectFishCurve;

    private void Start() {
        ChangePosition();
    }

    public void CollectFish(Vector3 fishPos, Sprite fishSprite) {
        var fish = ObjectPool.SpawnObject(_fishPrefab, fishPos);
        var dir = _bucket.position - fish.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;
        var vel = 7f;
        var time = dir.magnitude / vel;
        fish.GetComponentInChildren<SpriteRenderer>().sprite = fishSprite;
        fish.transform.rotation = Quaternion.Euler(0, 0, angle);
        fish.transform.DOMove(_bucket.position, time).OnComplete(() => {
            ObjectPool.DestroyObject(fish);
        }).SetEase(_collectFishCurve);

        var oScale = fish.transform.localScale.x;
        fish.transform.DOScale(oScale * 5f, time * 2/3).OnComplete(() => {
            fish.transform.DOScale(oScale, time * 1/3);
        }).SetEase(Ease.OutCubic);
    }

    private void ChangePosition() {
        var newPos = _standPositions[Random.Range(0, _standPositions.Length)];
        _fisherman.DOMove(newPos.position, 1f).SetEase(Ease.Linear);
        _fisherman.DORotateQuaternion(newPos.rotation, 1f).SetEase(Ease.Linear);
        Invoke(nameof(ChangePosition), _changePositionInterval);
    }
}
