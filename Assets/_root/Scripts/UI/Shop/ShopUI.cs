using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UI {
    [SerializeField] private ShopCategory _category;
    [SerializeField] private ShopProduct _product;
    [SerializeField] private ShopDetail _detail;
    [SerializeField] private Button _btnClose;
    [SerializeField] private RectTransform _boardRect;

    private float _boardRectLastSize;
    private float _boardRectCollapsedSize = -2000;
    private float _boardRectExpandedSize = -689;
    private float _boardRectFullExpandedSize = -400;
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _seq;

    private void Awake() {
        _category.OnWidgetSelected += SelectCategoryWidget;
        _product.OnBacked += OpenCategory;
        _product.OnWidgetSelected += SelectProductWidget;
        _btnClose.onClick.AddListener(Close);
    }

    public override void Open(params object[] prs) {
        gameObject.SetActive(true);
        _boardRectLastSize = _boardRectCollapsedSize;
        OpenCategory();
    }
    
    public override void Close() {
        gameObject.SetActive(false);
        UIManager.GetUI<HomeUI>()?.Expand();
    }

    private void Expand(bool full) {
        var expectedSize = full ? _boardRectFullExpandedSize : _boardRectExpandedSize;
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(DOVirtual.Float(_boardRectLastSize, expectedSize, _boardRectAnimationDuration,
            value => {
                _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
            }).SetEase(Ease.OutBack));

        _boardRectLastSize = expectedSize;
    }

    private void SelectCategoryWidget(ShopCategoryWidget widget) {
        OpenProduct();
    }

    private void SelectProductWidget(ShopProductWidget widget) {
        switch (widget.GetState()) {
            case ShopProductWidget.State.Owned:
                break;
            case ShopProductWidget.State.NotOwned:
                OpenDetail();
                break;
        }
    }
    
    private void OpenCategory() {
        Expand(false);
        _category.gameObject.SetActive(true);
        _product.gameObject.SetActive(false);
    }
    
    private void OpenProduct() {
        Expand(true);
        _category.gameObject.SetActive(false);
        _product.gameObject.SetActive(true);
    }
    
    private void OpenDetail() {
        _detail.gameObject.SetActive(true);
    }
}
