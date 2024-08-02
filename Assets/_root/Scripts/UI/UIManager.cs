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
    
    private static UI _curUI;
        
    public static T GetUI<T>() where T : UI {
        return Instance()._uiList.OfType<T>().FirstOrDefault();
    }

    public static void OpenUI<T>(params object[] prs) where T : UI {
        var ui = GetUI<T>();
        if (!ui) {
            ALog.Log($"{typeof(T).Name} is missing!!!");
            return;
        }

        if (_curUI == ui) {
            ALog.Log($"{typeof(T).Name} is already opened");
            return;
        }
        
        _curUI?.Close();
        _curUI = ui;
        _curUI.Open(prs);
    }

    public static void CloseUI<T>() where T : UI {
        var ui = GetUI<T>();
        ui?.Close();
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
