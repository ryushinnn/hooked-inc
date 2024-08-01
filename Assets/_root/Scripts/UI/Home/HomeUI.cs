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
    public void Expand(bool ignoreAnimation = false) {
        _currency.SetActive(true);
        if (!_profile.Expanded) _profile.ExpandOrCollapse(true, ignoreAnimation);
        if (!_side.Expanded) _side.ExpandOrCollapse(true, ignoreAnimation);
        if (!_navigation.Expanded) _navigation.ExpandOrCollapse(true, ignoreAnimation);
    }

    [Button]
    public void Collapse(bool completely, bool ignoreAnimation = false) {
        _currency.SetActive(!completely);
        if (completely == _profile.Expanded) _profile.ExpandOrCollapse(!completely, ignoreAnimation);
        if (_side.Expanded) _side.ExpandOrCollapse(false, ignoreAnimation);
        if (_navigation.Expanded) _navigation.ExpandOrCollapse(false, ignoreAnimation);
    }
}
