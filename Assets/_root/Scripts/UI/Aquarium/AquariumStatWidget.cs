using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AquariumStatWidget : MonoBehaviour {
    [SerializeField] private Image _imgIcon;
    [SerializeField] private TMP_Text _txtValue;
    [SerializeField] private Sprite[] _sprTridents;
    [SerializeField] private Sprite[] _sprLightnings;
    [SerializeField] private Sprite[] _sprReels;
    [SerializeField] private Sprite[] _sprLines;

    public void SetValue(bool available, string statId, float value) {
        switch (statId) {
            case "trident":
                _imgIcon.sprite = _sprTridents[available ? 1 : 0];
                _txtValue.text = $"+{value}";
                break;
            case "lightning":
                _imgIcon.sprite = _sprLightnings[available ? 1 : 0];
                _txtValue.text = $"+{value}%";
                break;
            case "reel":
                _imgIcon.sprite = _sprReels[available ? 1 : 0];
                _txtValue.text = $"+{value}%";
                break;
            case "line":
                _imgIcon.sprite = _sprLines[available ? 1 : 0];
                _txtValue.text = $"+{value}m";
                break;
        }
    }
}
