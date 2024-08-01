using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Water : MonoBehaviour {
    [SerializeField] private Material _waterMaterial;
    [SerializeField] private Material _boatTrailMaterial;
    [SerializeField] private Material _crateTrailMaterial;
    [SerializeField] private WaterPreset[] _presets;

    [Button]
    public void ChangePreset(int index) {
        var preset = _presets[index];
        _waterMaterial.SetFloat("_HsvShift", preset.Water.x);
        _waterMaterial.SetFloat("_HsvSaturation", preset.Water.y);
        _waterMaterial.SetFloat("_HsvBright", preset.Water.z);
        _boatTrailMaterial.SetFloat("_HsvShift", preset.Trail.x);
        _boatTrailMaterial.SetFloat("_HsvSaturation", preset.Trail.y);
        _boatTrailMaterial.SetFloat("_HsvBright", preset.Trail.z);
        _crateTrailMaterial.SetFloat("_HsvShift", preset.Trail.x);
        _crateTrailMaterial.SetFloat("_HsvSaturation", preset.Trail.y);
        _crateTrailMaterial.SetFloat("_HsvBright", preset.Trail.z);
    }
}

[Serializable]
public struct WaterPreset {
    public string ID;
    public Vector3 Water;
    public Vector3 Trail;
}
