using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyWidget : MonoBehaviour {
    [SerializeField] private Type _type;
    [SerializeField] private TMP_Text _txtValue;
    [SerializeField] private Button _btnAdd;

    private void Awake() {
        _btnAdd.onClick.AddListener(AddCurrency);
    }

    private void OnEnable() {
        switch (_type) {
            case Type.Hook:
                MessageDispatcher<GameEvent.OnHookChanged>.AddListener(UpdateCurrency);
                break;
            case Type.Money:
                MessageDispatcher<GameEvent.OnMoneyChanged>.AddListener(UpdateCurrency);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDisable() {
        switch (_type) {
            case Type.Hook:
                MessageDispatcher<GameEvent.OnHookChanged>.RemoveListener(UpdateCurrency);
                break;
            case Type.Money:
                MessageDispatcher<GameEvent.OnMoneyChanged>.RemoveListener(UpdateCurrency);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateCurrency(float value) {
        _txtValue.text = Common.GetFormatedNumber(value);
    }

    private void AddCurrency() {
        // ???
    }

    enum Type {
        Hook,
        Money
    }
}


