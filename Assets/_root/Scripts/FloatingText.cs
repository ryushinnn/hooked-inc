using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtValue;
    
    public void SetValue(string value) {
        _txtValue.text = value;
        DOVirtual.DelayedCall(0.5f, ()=> {
            ObjectPool.DestroyObject(gameObject);
        });
    }
}
