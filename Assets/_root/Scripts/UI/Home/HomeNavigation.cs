using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HomeNavigation : MonoBehaviour {
    [SerializeField] private RectTransform _navigationRect;
    [SerializeField] private RectTransform _menuRect;
    [SerializeField] private VerticalLayoutGroup _menuLayout;
    [SerializeField] private Image _imgMenuBg;
    [SerializeField] private CanvasGroup _cg;

    [BoxGroup("Button"), SerializeField] private Button _btnStart;
    [BoxGroup("Button"), SerializeField] private Button _btnShop;
    [BoxGroup("Button"), SerializeField] private Button _btnMap;
    [BoxGroup("Button"), SerializeField] private Button _btnCollection;
    [BoxGroup("Button"), SerializeField] private Button _btnMenu;
    [BoxGroup("Button"), SerializeField] private Button _btnCloseMenu;

    [BoxGroup("Menu"), SerializeField] private GameObject _menu;
    [BoxGroup("Menu"), SerializeField] private Button _btnFishTank;
    [BoxGroup("Menu"), SerializeField] private Button _btnAquarium;
    [BoxGroup("Menu"), SerializeField] private Button _btnRanking;
    [BoxGroup("Menu"), SerializeField] private Button _btnSetting;
    
    public bool Extended => _extended;

    private bool _extended;
    private float _navRectCollapsedSize = 0;
    private float _navRectExtendedSize = 1000;
    private float _navRectCollapedPosY = -330;
    private float _navRectExtendedPosY = 42;
    private float _navRectAnimationDuration = 0.25f;
    private Sequence _navigationSeq;

    private float _menuBgOpacity = 0.8f;
    private float _menuRectCollapsedSize = 225;
    private float _menuRectExtendedSize = 736;
    private float _menuLayoutCollapsedSpacing = -184;
    private float _menuLayoutExtendedSpacing = -33;
    private float _menuRectAnimationDuration = 0.5f;
    private Sequence _menuSeq;

    private void Awake() {
        _btnStart.onClick.AddListener(StartGame);
        _btnShop.onClick.AddListener(OpenShop);
        _btnMap.onClick.AddListener(OpenMap);
        _btnCollection.onClick.AddListener(OpenCollection);
        _btnMenu.onClick.AddListener(OpenMenu);
        _btnCloseMenu.onClick.AddListener(() => CloseMenu(false));
        _btnFishTank.onClick.AddListener(OpenFishTank);
        _btnAquarium.onClick.AddListener(OpenAquarium);
        _btnRanking.onClick.AddListener(OpenRanking);
        _btnSetting.onClick.AddListener(OpenSetting);
        
        ExtendOrCollapse(true, true);
    }

    public void ExtendOrCollapse(bool extend, bool ignoreAnimation) {
        _extended = extend;
        _navigationSeq?.Kill();

        if (ignoreAnimation) {
            _navigationRect.sizeDelta = new Vector2(_extended ? _navRectExtendedSize : _navRectCollapsedSize, _navigationRect.sizeDelta.y);
            _navigationRect.anchoredPosition = new Vector2(_navigationRect.anchoredPosition.x, _extended ? _navRectExtendedPosY : _navRectCollapedPosY);
            return;
        }
        
        _navigationSeq = DOTween.Sequence();
        if (_extended) {
            _navigationRect.sizeDelta = new Vector2(0, _navigationRect.sizeDelta.y);
            _navigationSeq.Append(DOVirtual.Float(_navRectCollapedPosY, _navRectExtendedPosY, _navRectAnimationDuration,
                value => {
                    _navigationRect.anchoredPosition = new Vector2(_navigationRect.anchoredPosition.x, value);
                }).SetEase(Ease.OutBack))
                .Append(DOVirtual.Float(_navRectCollapsedSize, _navRectExtendedSize, _navRectAnimationDuration,
                    value => {
                        _navigationRect.sizeDelta = new Vector2(value, _navigationRect.sizeDelta.y);
                    }).SetEase(Ease.OutBack));
        } else {
            _navigationSeq.Append(DOVirtual.Float(_navRectExtendedPosY, _navRectCollapedPosY, _navRectAnimationDuration,
                value => {
                    _navigationRect.anchoredPosition = new Vector2(_navigationRect.anchoredPosition.x, value);
                }).SetEase(Ease.InBack));
        }
    }
    
    private void OpenShop() {
        UIManager.OpenUI<ShopUI>();
    }
    
    private void OpenMap() {
        UIManager.OpenUI<MapUI>();
    }

    private void StartGame() {
        // ???
    }

    private void OpenCollection() {
        UIManager.OpenUI<CollectionUI>();
    }

    private void OpenMenu() {
        if (!_cg) _cg = _menuRect.gameObject.AddComponent<CanvasGroup>();
        _menu.SetActive(true);
        _imgMenuBg.color = Color.clear;
        _cg.alpha = 0;
        _menuSeq?.Kill();
        _menuSeq = DOTween.Sequence();
        _menuSeq.Append(DOVirtual.Float(_menuRectCollapsedSize, _menuRectExtendedSize, _menuRectAnimationDuration,
            value => {
                _menuRect.sizeDelta = new Vector2(_menuRect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack))
            .Join(DOVirtual.Float(_menuLayoutCollapsedSpacing, _menuLayoutExtendedSpacing, _menuRectAnimationDuration, 
                value => {
                _menuLayout.spacing = value;
            }))
            .Join(_imgMenuBg.DOFade(_menuBgOpacity,_menuRectAnimationDuration))
            .Join(_cg.DOFade(1,_menuRectAnimationDuration).SetEase(Ease.OutCubic));
    }

    private void CloseMenu(bool ignoreAnimation = true) {
        if (ignoreAnimation) {
            _menu.SetActive(false);
            return;
        }

        _imgMenuBg.color = new Color(0, 0, 0, _menuBgOpacity);
        _cg.alpha = 1;
        _menuSeq.Kill();
        _menuSeq = DOTween.Sequence();
        _menuSeq.Append(DOVirtual.Float(_menuRectExtendedSize, _menuRectCollapsedSize, _menuRectAnimationDuration/2,
            value => {
                _menuRect.sizeDelta = new Vector2(_menuRect.sizeDelta.x, value);
            }).SetEase(Ease.InBack))
            .Join(DOVirtual.Float(_menuLayoutExtendedSpacing, _menuLayoutCollapsedSpacing, _menuRectAnimationDuration/2, 
                value => {
                _menuLayout.spacing = value;
            }).SetEase(Ease.InBack))
            .Join(_imgMenuBg.DOFade(0,_menuRectAnimationDuration/2))
            .Join(_cg.DOFade(0,_menuRectAnimationDuration/2).SetEase(Ease.InCubic))
            .AppendCallback(() => {
                _menu.SetActive(false);
            });
    }

    private void OpenFishTank() {
        CloseMenu();
        UIManager.OpenUI<FishTankUI>();
    }

    private void OpenAquarium() {
        CloseMenu();
        UIManager.OpenUI<AquariumUI>();
    }

    private void OpenRanking() {
        CloseMenu();
        UIManager.OpenUI<RankingUI>();
    }

    private void OpenSetting() {
        CloseMenu();
        UIManager.OpenUI<SettingUI>();
    }
}
