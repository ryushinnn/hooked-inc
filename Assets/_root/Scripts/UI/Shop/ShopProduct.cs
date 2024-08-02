using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopProduct : MonoBehaviour {
    [SerializeField] private Transform _widgetParent;
    [SerializeField] private Button _btnBack;
    
    public Action OnBacked;
    public Action<ShopProductWidget> OnWidgetSelected;
    
    private List<ShopProductWidget> _widgets = new();

    private void Awake() {
        _btnBack.onClick.AddListener(Back);
        
        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out ShopProductWidget widget)) {
                _widgets.Add(widget);
                widget.GetComponent<Button>().onClick.AddListener(() => {
                    OnWidgetSelected?.Invoke(widget);
                });
            }
        }
    }

    private void OnEnable() {
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.ProfileAndCurrency);
    }

    private void Back() {
        OnBacked?.Invoke();
    }
}
