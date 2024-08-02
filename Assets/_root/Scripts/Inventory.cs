using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Core;
using Assassin.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

public class Inventory : Singleton<Inventory> {
    [SerializeField, ReadOnly] private float _currentMoney;
    
    protected override void OnAwake() {
    }

    public void ReceiveMoney(float amount) {
        _currentMoney += amount;
        EventDispatcher<GameEvent.OnMoneyChanged>.Trigger?.Invoke(_currentMoney);
    }

    
}
