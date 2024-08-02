using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCategory : MonoBehaviour {
    [SerializeField] private Transform _widgetParent;
    
    public Action<ShopCategoryWidget> OnWidgetSelected;

    private List<ShopCategoryWidget> _widgets = new();

    private void Awake() {
        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out ShopCategoryWidget widget)) {
                _widgets.Add(widget);
                widget.GetComponent<Button>().onClick.AddListener(() => {
                    OnWidgetSelected?.Invoke(widget);
                });
            }
        }
    }

    private void OnEnable() {
        UIManager.GetUI<HomeUI>()?.ChangeState(HomeUI.State.ExceptSide);
    }
}
