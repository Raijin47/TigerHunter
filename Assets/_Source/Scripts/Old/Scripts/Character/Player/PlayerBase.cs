using UnityEngine;

public class PlayerBase : CharacterBase
{
    [SerializeField] private Joystick _joystick;

    private float _horizontal;
    private float _vertical;
    private float _moveAmount;

    public bool IsHide { private get; set; }

    protected override void GetInput()
    {
        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;
    }

    protected override void Init()
    {
        Game.Action.OnEnter += () =>
        {
            _animator.OnGame = true;
            OnPause(false);
        };
        Game.Action.OnExit += () =>
        {
            _animator.OnGame = false;
            Restart();
        };
    }

    protected override void Movement()
    {
        Vector3 v = _vertical * Vector3.forward;
        Vector3 h = _horizontal * Vector3.right;

        _movement.MovementDirection = (v + h).normalized;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));

        _movement.Move(_moveAmount);
        _movement.Rotate();
        _animator.MovementAnimations(_moveAmount);
    }

    public void Found()
    {
        if (IsHide) return;

        Game.Action.SendLose();
    }
}