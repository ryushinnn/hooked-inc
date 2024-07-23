using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {
    [SerializeField] private Transform _bucket;

    public Vector3 GetBucketPosition() {
        return _bucket.position;
    }
}
