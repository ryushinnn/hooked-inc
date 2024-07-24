using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour {
    [SerializeField] private TMP_Text _txtCurrentMoney;
    [SerializeField] private FloatingText _floatingText;
    [SerializeField] private Transform _floatingTextParent;

    private void OnEnable() {
        MessageDispatcher<MessageID.OnMoneyUpdated>.AddListener(OnMoneyUpdated);
        MessageDispatcher<MessageID.OnFloatingTextRequested>.AddListener(SpawnFloatingText);
    }

    private void OnDisable() {
        MessageDispatcher<MessageID.OnMoneyUpdated>.RemoveListener(OnMoneyUpdated);
        MessageDispatcher<MessageID.OnFloatingTextRequested>.RemoveListener(SpawnFloatingText);
    }
    
    private void OnMoneyUpdated(float amount) {
        _txtCurrentMoney.text = $"${amount:N0}";
    }

    private void SpawnFloatingText(string value, Vector3 worldPos) {
        var ft = ObjectPool.SpawnObject(_floatingText.gameObject, _floatingTextParent).GetComponent<FloatingText>();
        ft.transform.position = worldPos;
        ft.transform.localScale = Vector3.one;
        ft.SetValue(value);
    }
}
