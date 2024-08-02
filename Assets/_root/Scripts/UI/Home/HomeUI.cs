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

    public void ChangeState(State state, bool ignoreAnimation = false) {
        var hasCurrency = CheckState(State.Currency, state);
        var hasProfile = CheckState(State.Profile, state);
        var hasNavigation = CheckState(State.Navigation, state);
        var hasSide = CheckState(State.Side, state);
        
        if (hasCurrency != _currency.activeSelf) _currency.SetActive(hasCurrency);
        if (hasProfile != _profile.Expanded) _profile.ExpandOrCollapse(hasProfile, ignoreAnimation);
        if (hasNavigation != _navigation.Expanded) _navigation.ExpandOrCollapse(hasNavigation, ignoreAnimation);
        if (hasSide != _side.Expanded) _side.ExpandOrCollapse(hasSide, ignoreAnimation);
    }

    private bool CheckState(State value, State combination) {
        return (combination & value) == value;
    }
    
    [Flags]
    public enum State {
        None = 0,
        Profile = 0x01,
        Currency = 0x02,
        Navigation = 0x04,
        Side = 0x08,
        ProfileAndCurrency = Profile | Currency,
        ExceptSide = Profile | Currency | Navigation,
        All = Profile | Currency | Navigation | Side
    }
}
