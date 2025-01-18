using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class PanelBase : MonoBehaviour
{
    [SerializeField] protected RectTransform[] _components;

    protected CanvasGroup _canvas;
    protected Sequence _sequence;

    protected const float _delay = 1f;

    private void Awake() => _canvas = GetComponent<CanvasGroup>();

    protected bool IsActive
    {
        set
        {
            _canvas.interactable = value;
            _canvas.blocksRaycasts = value;
        }
    }

    public bool IsShow
    {
        set
        {
            IsActive = false;

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            if (value) Show();
            else Hide();
        }
    }

    protected virtual void Start()
    {
        IsActive = false;
        _canvas.alpha = 0;
    }

    public void Enter() => IsShow = true;
    public void Exit() => IsShow = false;

    protected abstract void Show();
    protected abstract void Hide();
    protected virtual void OnShowComplated() => IsActive = true;

}