using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using UnityEngine;
using Logger = Assassin.Utils.Logger;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {
    [SerializeField] private Fish[] _fishPrefabs;
    [SerializeField] private EliteFish[] _eliteFishPrefab; 
    [SerializeField] private float _borderOffset;
    [SerializeField] private MovementType _fishMovementType;

    private Camera _camera;
    private float _screenHeight, _screenWidth;

    private void Awake() {
        _camera = Common.GetCamera();
        _screenHeight = _camera.orthographicSize * 2;
        _screenWidth = _screenHeight * Screen.width / Screen.height;
    }

    public void Spawn() {
        var spawnPosition = new Vector3();
        var destination = new Vector3();
        var fishPrefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Length)];

        if (_fishMovementType == MovementType.TwoDirections) {
            switch ((Direction)(Random.Range(0, 2) * 2)) {
                case Direction.TopToDown:
                    var posX = Random.Range(-_screenWidth / 2, _screenWidth / 2);
                    spawnPosition = new Vector3(posX, _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                    destination = new Vector3(posX, _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                    break;
                case Direction.RightToLeft:
                    var posY = Random.Range(-_screenHeight / 2, _screenHeight / 2);
                    var offsetY = _screenWidth * Mathf.Tan(30 * Mathf.Deg2Rad);
                    spawnPosition = new Vector3(_camera.transform.position.x + _screenWidth / 2 + _borderOffset, posY, 0);
                    destination = new Vector3(_camera.transform.position.x - _screenWidth / 2 - _borderOffset, posY + offsetY, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        } else if (_fishMovementType == MovementType.FourDirections) {
            switch ((Direction)Random.Range(0, 4)) {
                case Direction.TopToDown:
                    spawnPosition = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                    destination = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                    break;
                case Direction.DownToTop:
                    spawnPosition = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                    destination = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                    break;
                case Direction.LeftToRight:
                    spawnPosition = new Vector3(_camera.transform.position.x - _screenWidth / 2 - _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                    destination = new Vector3(_camera.transform.position.x + _screenWidth / 2 + _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                    break;
                case Direction.RightToLeft:
                    spawnPosition = new Vector3(_camera.transform.position.x + _screenWidth / 2 + _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                    destination = new Vector3(_camera.transform.position.x - _screenWidth / 2 - _borderOffset, Random.Range(-_screenHeight / 2, _screenHeight / 2), 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var fish = ObjectPool.SpawnObject(fishPrefab.gameObject, spawnPosition).GetComponent<Fish>();
        fish.transform.SetParent(transform);
        fish.SetDestination(destination, false);
    }

    public void SpawnSchool() {
        var waveLength = Random.Range(2f, 3f);
        var fishCount = Random.Range(30, 50);
        var fishPrefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Length)];
        var routes = new List<Tuple<Vector3, Vector3>>();
        var posY = Random.Range(-_screenHeight / 2, _screenHeight / 2);
        
        switch ((Direction)(Random.Range(0, 2) + 2)) {
            case Direction.RightToLeft:
                for (int i = 0; i < fishCount; i++) {
                    var randY = Random.Range(-1f, 1f);
                    var randX = Random.Range(0, waveLength);
                    var offsetY = _screenWidth * Mathf.Tan(30 * Mathf.Deg2Rad);
                    routes.Add(Tuple.Create(
                        new Vector3(_camera.transform.position.x + _screenWidth / 2 + randX, posY + randY, 0),
                        new Vector3(_camera.transform.position.x - _screenWidth / 2 - (waveLength - randX), posY + randY + offsetY, 0)));
                }
                break;
            case Direction.LeftToRight:
                for (int i = 0; i < fishCount; i++) {
                    var randY = Random.Range(-1f, 1f);
                    var randX = Random.Range(0, waveLength);
                    var offsetY = _screenWidth * Mathf.Tan(30 * Mathf.Deg2Rad);
                    routes.Add(Tuple.Create(
                        new Vector3(_camera.transform.position.x - _screenWidth / 2 - randX, posY + randY, 0),
                        new Vector3(_camera.transform.position.x + _screenWidth / 2 + (waveLength - randX), posY + randY + offsetY, 0)));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        foreach (var route in routes) {
            var fish = ObjectPool.SpawnObject(fishPrefab.gameObject, route.Item1).GetComponent<Fish>();
            fish.transform.SetParent(transform);
            fish.SetDestination(route.Item2, true);
        }
    }

    public void SpawnElite() {
        var spawnPosition = new Vector3();
        var destination = new Vector3();
        var fishPrefab = _eliteFishPrefab[Random.Range(0, _eliteFishPrefab.Length)];

        switch ((Direction)Random.Range(0,1)) {
            case Direction.TopToDown:
                spawnPosition = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                destination = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                break;
            case Direction.DownToTop:
                spawnPosition = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y - _camera.orthographicSize - _borderOffset, 0);
                destination = new Vector3(Random.Range(-_screenWidth / 2, _screenWidth / 2), _camera.transform.position.y + _camera.orthographicSize + _borderOffset, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var fish = ObjectPool.SpawnObject(fishPrefab.gameObject, spawnPosition).GetComponent<EliteFish>();
        fish.transform.SetParent(transform);
        fish.SetDestination(destination, false);
        fish.ResetHp();
    }
}

public enum MovementType {
    TwoDirections,
    FourDirections
}

public enum Direction {
    TopToDown,
    DownToTop,
    RightToLeft,
    LeftToRight
}
