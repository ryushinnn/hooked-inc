using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assassin.Core;
using Assassin.Extension;
using Sirenix.OdinInspector;
using UnityEngine;
using Logger = Assassin.Utils.Logger;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private UI[] _uis;

    private Stack<UI> _cache = new();

    protected override void OnAwake() {
        
    }

    public void OpenUI<T>(bool stack = false) where T : UI {
        var ui = _uis.OfType<T>().FirstOrDefault();
        if (!ui) {
            Logger.Log($"UI with type {typeof(T).Name} is missing!!!");
            return;
        }

        if (stack) {
            _cache.Push(ui);
        } else {
            while (_cache.Count > 0) {
                _cache.Pop().Close();
            }
            _cache.Clear();
        }
        
        ui.Open();
    }

    public void CloseUI<T>() where T : UI {
        var ui = _uis.OfType<T>().FirstOrDefault();
        if (!ui) {
            Logger.Log($"UI with type {typeof(T).Name} is missing!!!");
            return;
        }

        ui.Close();
        if (_cache.Count > 0) {
            _cache.Pop().Open();
        }
    }

    [Button]
    private void OpenHome() {
        OpenUI<HomeUI>();
    }
}
