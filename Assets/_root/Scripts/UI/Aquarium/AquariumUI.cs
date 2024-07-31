using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AquariumUI : UI {
    [SerializeField] private AquariumBoard _board;
    [SerializeField] private AquariumInside _inside;
    [SerializeField] private AquariumDetail _detail;

    private void Awake() {
        _board.OnWidgetSelected += SelectWidget;
        _board.OnClosed += Close;
        _inside.OnBacked += OpenBoard;
    }

    public override void Open(params object[] prs) {
        gameObject.SetActive(true);
    }

    public override void Close() {
        gameObject.SetActive(false);
        UIManager.GetUI<HomeUI>()?.Extend();
    }

    private void SelectWidget(AquariumWidget widget) {
        switch (widget.GetState()) {
            case AquariumWidget.State.Owned:
                OpenInside();
                break;
            case AquariumWidget.State.Purchasable:
                OpenDetail();
                break;
        }
    }

    private void OpenBoard() {
        _board.gameObject.SetActive(true);
        _inside.gameObject.SetActive(false);
    }

    private void OpenInside() {
        _board.gameObject.SetActive(false);
        _inside.gameObject.SetActive(true);
    }

    private void OpenDetail() {
        _detail.gameObject.SetActive(true);
    }
}
