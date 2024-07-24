using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using UnityEngine;

public class Boat : MonoBehaviour {
    [SerializeField] private Transform _bucket;
    [SerializeField] private GameObject _fishPrefab;
    [SerializeField] private AnimationCurve _collectFishCurve;
    
    public void CollectFish(Vector3 fishPos) {
        var fish = ObjectPool.SpawnObject(_fishPrefab, fishPos);
        var dir = _bucket.position - fish.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var vel = 7f;
        var time = dir.magnitude / vel;
        fish.transform.rotation = Quaternion.Euler(0, 0, angle);
        fish.transform.DOMove(_bucket.position, time).OnComplete(() => {
            ObjectPool.DestroyObject(fish);
        }).SetEase(_collectFishCurve);

        var oScale = fish.transform.localScale.x;
        fish.transform.DOScale(oScale * 2.5f, time * 2/3).OnComplete(() => {
            fish.transform.DOScale(oScale, time * 1/3);
        }).SetEase(Ease.OutCubic);
    }
}
