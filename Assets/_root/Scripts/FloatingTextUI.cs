using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using UnityEngine;

public class FloatingTextUI : MonoBehaviour {
    [SerializeField] private FloatingText _floatingTextPrefab;

    private List<FloatingText> _floatingTexts = new();
    
    private void OnEnable() {
        MessageDispatcher<MessageID.OnFloatingTextRequested>.AddListener(SpawnFloatingText);
    }

    private void OnDisable() {
        MessageDispatcher<MessageID.OnFloatingTextRequested>.RemoveListener(SpawnFloatingText);
    }
    
    private void SpawnFloatingText(string value, Vector3 worldPos) {
        var ft = GetAvailableFloatingText();
        ft.transform.position = worldPos;
        ft.SetValue(value);
    }

    private FloatingText GetAvailableFloatingText() {
        var ft = _floatingTexts.Find(x => !x.gameObject.activeSelf);
        if (ft == null) {
            ft = Instantiate(_floatingTextPrefab, transform);
            _floatingTexts.Add(ft);
        }

        ft.gameObject.SetActive(true);
        return ft;
    }
}
