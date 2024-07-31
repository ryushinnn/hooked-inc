using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assassin.Core;
using Assassin.Extension;
using Assassin.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private List<UI> _uiList = new();

    private Stack<UI> _cache = new();

    public static T GetUI<T>() where T : UI {
        return Instance()._uiList.OfType<T>().FirstOrDefault();
    }

    public static void OpenUI<T>(bool stack = false, params object[] prs) where T : UI {
        var ui = GetUI<T>();
        if (!ui) {
            ALog.Log($"UI with type {typeof(T).Name} is missing!!!");
            return;
        }

        if (stack) {
            Instance()._cache.Push(ui);
        } else {
            while (Instance()._cache.Count > 0) {
                Instance()._cache.Pop().Close();
            }
            Instance()._cache.Clear();
        }
        
        ui.Open(prs);
    }

    public static void CloseUI<T>() where T : UI {
        var ui = GetUI<T>();
        if (!ui) {
            ALog.Log($"UI with type {typeof(T).Name} is missing!!!");
            return;
        }

        ui.Close();
        if (Instance()._cache.Count > 0) {
            Instance()._cache.Pop().Open();
        }
    }

#if UNITY_EDITOR
    [Button]
    private void FindAllUI() {
        _uiList = new List<UI>();
        foreach (Transform child in transform) {
            if (child.TryGetComponent(out UI ui)) {
                _uiList.Add(ui);
            }
        }
    }
#endif
}
