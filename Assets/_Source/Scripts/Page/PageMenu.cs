using DG.Tweening;
using UnityEngine;

public class PageMenu : PanelBase
{
    [SerializeField] private ButtonBase _buttonStart;

    protected override void Hide()
    {
        _sequence.Append(_canvas.DOFade(0, _delay));
    }

    protected override void Show()
    {
        _sequence.SetDelay(_delay).
            Append(_canvas.DOFade(1, _delay)).



            OnComplete(OnShowComplated);
    }

    protected override void Start()
    {
        _canvas.alpha = 1;
        IsActive = true;

        _buttonStart.OnClick.AddListener(Game.Action.SendEnter);
        Game.Action.OnEnter += Exit;
        Game.Action.OnExit += Enter;
    }
}