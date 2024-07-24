using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EliteFish : Fish {
    [SerializeField] private float _maxHp;
    [SerializeField] private Image _imgHpFill;

    [SerializeField, ReadOnly] private float _currentHp;

    private void Awake() {
        ResetHp();
    }

    public void ResetHp() {
        _currentHp = _maxHp;
        _imgHpFill.fillAmount = 1;
    }

    public void TakeDamage(float dmg) {
        _currentHp -= dmg;
        _imgHpFill.fillAmount = _currentHp / _maxHp;
        if (_currentHp <= 0) {
            OnCaught();
        }
    }
}
