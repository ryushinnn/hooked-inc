using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : UI {
    [SerializeField] private HomeSide _side;
    [SerializeField] private HomeNavigation _navigation;
    [SerializeField] private HomeProfile _profile;
    [SerializeField] private GameObject _currency;
    
    [Button]
    public void Extend(bool ignoreAnimation = false) {
        _currency.SetActive(true);
        if (!_profile.Extended) _profile.ExtendOrCollapse(true, ignoreAnimation);
        if (!_side.Extended) _side.ExtendOrCollapse(true, ignoreAnimation);
        if (!_navigation.Extended) _navigation.ExtendOrCollapse(true, ignoreAnimation);
    }

    [Button]
    public void Collapse(bool completely, bool ignoreAnimation = false) {
        if (completely == _profile.Extended) _profile.ExtendOrCollapse(!completely, ignoreAnimation);
        _currency.SetActive(!completely);
        if (_side.Extended) _side.ExtendOrCollapse(false, ignoreAnimation);
        if (_navigation.Extended) _navigation.ExtendOrCollapse(false, ignoreAnimation);
    }
}
