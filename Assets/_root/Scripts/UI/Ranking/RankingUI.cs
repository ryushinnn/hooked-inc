using System.Collections;
using System.Collections.Generic;
using Assassin.Extension;
using Assassin.Utils;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : UI {
    [SerializeField] private Transform _widgetParent;
    [SerializeField] private Button _btnClose;
    [SerializeField] private RectTransform _boardRect;
    [SerializeField] private RankingWidget _selfRankingWidget;
    [SerializeField] private ScrollRect _scrollRect;

    private List<RankingWidget> _widgets = new();
    private float _boardRectCollapsedSize = -1968;
    private float _boardRectExpandedSize = -517;
    private float _boardRectAnimationDuration = 0.5f;
    private Sequence _boardSeq;
    
    private RectTransform _selfRankingRect;
    //test
    private int _selfRanking = 12;
    private RectTransform _selfRankingOnBoardRect;
    private bool _selfRankingExpanded;
    private float _selfRankingRectCollapsedPosY = -90;
    private float _selfRankingRectExpandedPosY = 135;
    private float _selfRankingRectAnimationDuration = 0.25f;
    private Sequence _selfRankingSeq;
    
    private YieldInstruction _waitForEndOfFrame = new WaitForEndOfFrame();
    private float _focusAnimationDuration = 0.25f;
    private Tween _focusTween;
    
    private void Awake() {
        _selfRankingRect = _selfRankingWidget.GetComponent<RectTransform>();
        _btnClose.onClick.AddListener(Close);
        _scrollRect.onValueChanged.AddListener(OnScroll);
        _selfRankingRect.GetComponent<Button>().onClick.AddListener(FocusOnSelfRanking);
        
        foreach (Transform child in _widgetParent) {
            if (child.TryGetComponent(out RankingWidget widget)) {
                _widgets.Add(widget);
            }
        }
        
        _selfRankingOnBoardRect = _widgets[_selfRanking - 1].GetComponent<RectTransform>();
        
        //test
        _selfRankingOnBoardRect.GetComponent<Image>().color = Color.red;
    }

    public override void Open(params object[] prs) {
        gameObject.SetActive(true);
        UIManager.GetUI<HomeUI>()?.Collapse(false);
        Expand();
    }

    public override void Close() {
        gameObject.SetActive(false);
        UIManager.GetUI<HomeUI>()?.Expand();
    }

    private void Expand() {
        ScrollToTop();
        _selfRankingRect.anchoredPosition = new Vector2(_boardRect.anchoredPosition.x, _selfRankingRectCollapsedPosY);
        _boardSeq?.Kill();
        _boardSeq = DOTween.Sequence();
        _boardSeq.Append(DOVirtual.Float(_boardRectCollapsedSize, _boardRectExpandedSize, _boardRectAnimationDuration,
                value => {
                    _boardRect.sizeDelta = new Vector2(_boardRect.sizeDelta.x, value);
                }).SetEase(Ease.OutBack));

        _selfRankingSeq?.Kill();
        _selfRankingSeq = DOTween.Sequence();
        _selfRankingSeq.Insert(_boardRectAnimationDuration, DOVirtual.Float(_selfRankingRectCollapsedPosY, _selfRankingRectExpandedPosY, 
            _selfRankingRectAnimationDuration, value => {
                _selfRankingRect.anchoredPosition = new Vector2(_boardRect.anchoredPosition.x, value);
            }).SetEase(Ease.OutBack));

        _selfRankingExpanded = true;
    }

    private void OnScroll(Vector2 _) {
        if (_selfRankingExpanded == !IsSelfRankingVisible()) return;
        _selfRankingExpanded = !_selfRankingExpanded;
        _selfRankingSeq?.Kill();
        _selfRankingSeq = DOTween.Sequence();
        _selfRankingSeq.Append(DOVirtual.Float(_selfRankingExpanded ? _selfRankingRectCollapsedPosY : _selfRankingRectExpandedPosY, 
            _selfRankingExpanded ? _selfRankingRectExpandedPosY : _selfRankingRectCollapsedPosY, 
            _selfRankingRectAnimationDuration, value => {
                _selfRankingRect.anchoredPosition = new Vector2(_boardRect.anchoredPosition.x, value);
            }).SetEase(_selfRankingExpanded ? Ease.OutBack : Ease.InBack));
    }
    
    private bool IsSelfRankingVisible() {
        var element = _selfRankingOnBoardRect;
        var container = _scrollRect.GetComponent<RectTransform>();

        var elementCorners = new Vector3[4];
        var containerCorners = new Vector3[4];

        element.GetWorldCorners(elementCorners);
        container.GetWorldCorners(containerCorners);

        var isVisible = false;
        foreach (var corner in elementCorners) {
            if (corner.x >= containerCorners[0].x && corner.x <= containerCorners[2].x &&
                corner.y >= containerCorners[0].y && corner.y <= containerCorners[2].y) {
                isVisible = true;
                break;
            }
        }

        if (!isVisible) {
            var rect1 = new Rect(elementCorners[0], element.sizeDelta);
            var rect2 = new Rect(containerCorners[0], container.sizeDelta);
            isVisible = rect1.Overlaps(rect2);
        }

        return isVisible;
    }
    
    private void FocusOnSelfRanking() {
        StartCoroutine(DoFocusOnSelfRanking());
    }

    IEnumerator DoFocusOnSelfRanking() {
        yield return _waitForEndOfFrame;
        var curVPos = _scrollRect.verticalNormalizedPosition;
        var expectedVPos = _scrollRect.CalculateFocusedScrollPosition(_selfRankingOnBoardRect).y;
        _focusTween?.Kill();
        _focusTween = DOVirtual.Float(curVPos, expectedVPos, _focusAnimationDuration, value => {
            _scrollRect.verticalNormalizedPosition = value;
        });
    }
    
    private void ScrollToTop() {
        StartCoroutine(DoScrollToTop());
    }

    IEnumerator DoScrollToTop() {
        yield return _waitForEndOfFrame;
        _scrollRect.verticalNormalizedPosition = 1;
    }
}
