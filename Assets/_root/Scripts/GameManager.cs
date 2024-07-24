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
    [BoxGroup("References"), SerializeField] private Spawner _spawner;
    [BoxGroup("References"), SerializeField] private GameObject _rewardPrefab;
    [BoxGroup("References"), SerializeField] private GameObject _cursor;
    
    [BoxGroup("Configs:"), SerializeField] private Vector2 _spawnInterval;
    [BoxGroup("Configs:"), SerializeField] private Vector2 _spawnSchoolInterval;
    [BoxGroup("Configs:"), SerializeField] private Vector2 _spawnEliteInterval;
    [BoxGroup("Configs:"), SerializeField] private LayerMask _fishLayerMask;

    private RaycastHit2D[] _hits = new RaycastHit2D[20];
    
    protected override void OnAwake() {
    }

    private void Start() {
        SpawnFish();
        Invoke(nameof(SpawnSchoolOfFish), _spawnSchoolInterval.x);
        Invoke(nameof(SpawnEliteFish), _spawnEliteInterval.x);
    }

    private void Update() {
        CatchFish();
    }

    private void SpawnFish() {
        _spawner.Spawn();
        var interval = Random.Range(_spawnInterval.x, _spawnInterval.y);
        Invoke(nameof(SpawnFish), interval);
    }

    private void SpawnSchoolOfFish() {
        _spawner.SpawnSchool();
        var interval = Random.Range(_spawnSchoolInterval.x, _spawnSchoolInterval.y);
        Invoke(nameof(SpawnSchoolOfFish), interval);
    }

    private void SpawnEliteFish() {
        _spawner.SpawnElite();
        var interval = Random.Range(_spawnEliteInterval.x, _spawnEliteInterval.y);
        Invoke(nameof(SpawnEliteFish), interval);
    }

    private void CatchFish() {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) {
            _cursor.SetActive(true);
            var mousePosition = Common.GetCamera().ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            _cursor.transform.position = mousePosition;
            var count = Physics2D.RaycastNonAlloc(mousePosition, Vector2.zero, _hits, Mathf.Infinity, _fishLayerMask);
            for (int i = 0; i < count; i++) {
                if (_hits[i].collider != null) {
                    var fish = _hits[i].collider.gameObject;
                    if (fish.TryGetComponent(out EliteFish elite)) {
                        elite.TakeDamage(1);
                    } else {
                        fish.GetComponent<Fish>().OnCaught();
                        _boat.CollectFish(fish.transform.position);
                    }
                }
            }
        } else {
            _cursor.SetActive(false);
        }
#endif
    }
}
