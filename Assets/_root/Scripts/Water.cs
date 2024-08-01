using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Water : MonoBehaviour {
    [SerializeField] private Material[] _waterMaterials;
    [SerializeField] private Material[] _trailMaterial;
    [SerializeField] private WaterPreset[] _presets;

    [Button]
    public void ChangePreset(int index) {
        var preset = _presets[index];
        foreach (var water in _waterMaterials) {
            water.SetFloat("_HsvShift", preset.Water.x);
            water.SetFloat("_HsvSaturation", preset.Water.y);
            water.SetFloat("_HsvBright", preset.Water.z);
        }

        foreach (var trail in _trailMaterial) {
            trail.SetFloat("_HsvShift", preset.Trail.x);
            trail.SetFloat("_HsvSaturation", preset.Trail.y);
            trail.SetFloat("_HsvBright", preset.Trail.z);
        }
    }
}

[Serializable]
public struct WaterPreset {
    public string ID;
    public Vector3 Water;
    public Vector3 Trail;
}
