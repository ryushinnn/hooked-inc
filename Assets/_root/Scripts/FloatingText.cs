using System.Collections;
using System.Collections.Generic;
using Assassin.Utils.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtValue;

    private Sequence _seq;
    
    public void SetValue(string value) {
        _txtValue.text = value;
        _txtValue.transform.localPosition = Vector3.zero;
        _txtValue.color = Color.white;

        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_txtValue.DOFade(0f, 1f))
            .Join(_txtValue.transform.DOMoveY(_txtValue.transform.position.y + 1, 1f))
            .AppendCallback(()=> {
                gameObject.SetActive(false);
            });
    }
}
