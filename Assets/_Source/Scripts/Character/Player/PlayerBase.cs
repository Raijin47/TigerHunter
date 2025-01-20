using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private ParticleSystem _particle;
    private CharacterAnimation _animator;

    private bool _isPause;

    private float _horizontal;
    private float _vertical;
    private float _moveAmount;

    private bool IsPause
    {
        get => _isPause;
        set
        {
            _isPause = value;
            _movement.MovementDirection = Vector3.zero;
        }
    }

    private void Awake()
    {
        _movement.Init(GetComponent<Rigidbody>());
        _animator = new(GetComponentInChildren<Animator>());
    }

    private void Start()
    {
        _isPause = true;
        Game.Action.OnPause += OnPause;
        Game.Action.OnEnter += () =>
        {
            _animator.OnGame = true;
            OnPause(false);
        };
        Game.Action.OnExit += () =>
        {
            _animator.OnGame = false;
        };
    }

    private void Update()
    {
        if (IsPause) return;
        GetInput();
    }
    private void FixedUpdate()
    {
        if (IsPause) return;
        Movement();
    }
    private void OnPause(bool pause)
    {
        IsPause = pause;
        _animator.IsActive = !pause;
    }

    private void GetInput()
    {
        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;
    }

    private void Movement()
    {
        Vector3 v = _vertical * Vector3.forward;
        Vector3 h = _horizontal * Vector3.right;

        _movement.MovementDirection = (v + h).normalized;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));

        _movement.Move(_moveAmount);
        _movement.Rotate();
        _animator.MovementAnimations(_moveAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyBase enemy))
        {
            _animator.Attack();
            
        }
    }
}