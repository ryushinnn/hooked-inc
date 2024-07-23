using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Core;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Logger = Assassin.Utils.Logger;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager> {
    [BoxGroup("References"), SerializeField] private Boat _boat;
    [BoxGroup("References"), SerializeField] private FishSpawner _fishSpawner;
    [BoxGroup("References"), SerializeField] private GameObject _rewardPrefab;
    
    [BoxGroup("Configs:"), SerializeField] private float _spawnIntervalMin, _spawnIntervalMax;
    [BoxGroup("Configs:"), SerializeField] private LayerMask _fishLayerMask;

    private RaycastHit2D[] _hits = new RaycastHit2D[20];
    
    protected override void OnAwake() {
    }

    private void Start() {
        SpawnFish();
    }

    private void Update() {
        CatchFish();
    }

    private void SpawnFish() {
        _fishSpawner.Spawn();
        var interval = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        Invoke(nameof(SpawnFish), interval);
    }

    private void CatchFish() {
#if UNITY_EDITOR
        if (!Input.GetMouseButton(0)) return;
        var mousePosition = Common.GetCamera().ScreenToWorldPoint(Input.mousePosition);
        var count = Physics2D.RaycastNonAlloc(mousePosition, Vector2.zero, _hits, Mathf.Infinity, _fishLayerMask);

        for (int i = 0; i < count; i++) {
            if (_hits[i].collider != null) {
                var fish = _hits[i].collider.gameObject;
                fish.GetComponent<Fish>().OnCaught();
                var reward = ObjectPool.SpawnObject(_rewardPrefab, fish.transform.position);
                reward.transform.DOMove(_boat.GetBucketPosition(), 0.5f).OnComplete(() => {
                    ObjectPool.DestroyObject(reward);
                }).SetEase(Ease.Linear);
            }
        }
#endif
    }

    private void CollectReward(Vector3 fishPos) {
        var reward = ObjectPool.SpawnObject(_rewardPrefab, fishPos);
        reward.transform.DOMove(_boat.GetBucketPosition(), 0.5f).OnComplete(() => {
            ObjectPool.DestroyObject(reward);
        }).SetEase(Ease.Linear);
    }
}
