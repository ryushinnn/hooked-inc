using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using UnityEngine;
using Logger = Assassin.Utils.Logger;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour {
    [SerializeField] private Fish[] _fishPrefabs;
    [SerializeField] private float _borderOffset;

    private Camera _camera;
    private float _screenHeight, _screenWidth;

    private void Awake() {
        _camera = Common.GetCamera();
        _screenHeight = _camera.orthographicSize * 2;
        _screenWidth = _screenHeight * Screen.width / Screen.height;
    }

    public void Spawn() {
        var sides = Enum.GetValues(typeof(Side));
        var side = (Side)sides.GetValue(Random.Range(0, sides.Length));
        var fishPrefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Length)];
        var spawnPosition = new Vector3();
        var destination = new Vector3();
        
        switch (side) {
            case Side.Top:
                spawnPosition = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                destination = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                break;
            case Side.Down:
                spawnPosition = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                destination = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                break;
            case Side.Left:
                spawnPosition = new Vector3(_camera.transform.position.x - _screenWidth / 2 - _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                destination = new Vector3(_camera.transform.position.x + _screenWidth / 2 + _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                break;
            case Side.Right:
                spawnPosition = new Vector3(_camera.transform.position.x + _screenWidth / 2 + _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                destination = new Vector3(_camera.transform.position.x - _screenWidth / 2 - _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var fish = ObjectPool.SpawnObject(fishPrefab.gameObject, spawnPosition);
        fish.transform.SetParent(transform);
        fish.GetComponent<Fish>().SetDestination(destination);
    }
}

public enum Side {
    Top,
    Down,
    Left,
    Right
}
