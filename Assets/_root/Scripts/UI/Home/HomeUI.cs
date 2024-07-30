using System;
using System.Collections;
using System.Collections.Generic;
using Assassin.Utils;
using Assassin.Utils.ObjectPool;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : UI {
    [BoxGroup("Player Info"), SerializeField] private Image _imgAvt;
    [BoxGroup("Player Info"), SerializeField] private TMP_Text _txtLevel;
    [BoxGroup("Player Info"), SerializeField] private Image _imgXp;

    [BoxGroup("Button"), SerializeField] private Button _btnMission;
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

    private void Awake() {
        _btnMission.onClick.AddListener(OpenDailyMission);
        _btnStart.onClick.AddListener(StartGame);
        _btnShop.onClick.AddListener(OpenShop);
        _btnMap.onClick.AddListener(OpenMap);
        _btnCollection.onClick.AddListener(OpenCollection);
        _btnMenu.onClick.AddListener(OpenMenu);
        _btnCloseMenu.onClick.AddListener(CloseMenu);
        _btnFishTank.onClick.AddListener(OpenFishTank);
        _btnAquarium.onClick.AddListener(OpenAquarium);
        _btnRanking.onClick.AddListener(OpenRanking);
        _btnSetting.onClick.AddListener(OpenSetting);
    }

    private void OnEnable() {
        MessageDispatcher<GameEvent.OnAvatarChanged>.AddListener(SetAvatar);
        MessageDispatcher<GameEvent.OnXpChanged>.AddListener(SetLevelAndXp);
    }

    private void OnDisable() {
        MessageDispatcher<GameEvent.OnAvatarChanged>.RemoveListener(SetAvatar);
        MessageDispatcher<GameEvent.OnXpChanged>.RemoveListener(SetLevelAndXp);
    }

    private void SetAvatar(Sprite spr) {
        _imgAvt.sprite = spr;
    }

    private void SetLevelAndXp(int lv, int curXp, int nextXp) {
        _txtLevel.text = lv.ToString();
        _imgXp.fillAmount = 1f * curXp / nextXp;
    }

    private void OpenDailyMission() {
        UIManager.OpenUI<DailyMissionUI>();
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
        _menu.SetActive(true);
    }

    private void CloseMenu() {
        _menu.SetActive(false);
    }

    private void OpenFishTank() {
        UIManager.OpenUI<FishTankUI>();
    }

    private void OpenAquarium() {
        UIManager.OpenUI<AquariumUI>();
    }

    private void OpenRanking() {
        UIManager.OpenUI<RankingUI>();
    }

    private void OpenSetting() {
        UIManager.OpenUI<SettingUI>();
    }
}
