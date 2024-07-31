using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AquariumWidget : MonoBehaviour {
    [SerializeField] private Image _imgPreview;
    [SerializeField] private TMP_Text _txtName;
    [SerializeField] private AquariumStatWidget[] _statWidgets; 
    
    [SerializeField] private GameObject _ownedView;
    [SerializeField] private GameObject _unownedView;
    [SerializeField] private GameObject _unownedMark;
    [SerializeField] private TMP_Text _txtProgress;
    [SerializeField] private TMP_Text _txtPrice;
    
    [SerializeField] private Stat[] _stats;

    [SerializeField] private State _state;

    // private CanvasGroup _cg;
    private float _appearAnimationDuration = 0.2f;
    private Sequence _seq;

    public State GetState() {
        return _state;
    }

    public void PlayAppearAnimation(float delay) {
        // if (!_cg) _cg = gameObject.AddComponent<CanvasGroup>();
        // _appearSeq?.Kill();
        // _appearSeq = DOTween.Sequence();
        // _cg.alpha = 0;
        // _appearSeq.AppendInterval(delay)
        //     .Append(_cg.DOFade(1, _appearAnimationDuration));
    }
    
    public enum State {
        Owned,
        Purchasable,
        Unpurchasable
    }
}

//test
[Serializable]
public struct Stat {
    public string StatID;
    public float Value;
}