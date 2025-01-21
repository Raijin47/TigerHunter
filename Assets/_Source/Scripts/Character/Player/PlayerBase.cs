using DG.Tweening;
using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public event Action<int> OnTakeDamage;

    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Joystick _rotateJoystick;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Transform _transform;
    [SerializeField] private Vector3 _attackArea;
    [SerializeField] private float _rayDistance;

    private CharacterAnimation _animator;
    private Sequence _sequence;
    private LayerMask _layer;

    private bool _isPause;

    private float _attack;

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
        _layer = LayerMask.GetMask("Enemy");
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
        _horizontal = _rotateJoystick.Horizontal;
        _vertical = _joystick.Vertical;
    }

    public void Attack()
    {
        if (IsPause) return;
        IsPause = true;
        _animator.Attack();
        _particle.Play();

        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        _sequence.Append(_transform.DOLocalMoveZ(1, 1f).OnComplete(CheckEnemy)).
            Append(_transform.DOLocalMoveZ(0, .5f)
            .OnComplete(() => 
            {
                IsPause = false;
                _particle.Stop();
            }));
    }

    private void CheckEnemy()
    {
        var enemies = Physics.BoxCastAll(transform.position, _attackArea, Vector3.forward, transform.rotation, _rayDistance, _layer);

        foreach (RaycastHit enemy in enemies)   
            if (enemy.collider.TryGetComponent(out EnemyBase e))
                e.ApplyDamage(Mathf.RoundToInt(_attack));      
    }

    public void ApplyDamage(int value) => OnTakeDamage?.Invoke(value);

    private void Movement()
    {
        _movement.MovementDirection = _vertical * transform.forward;
        _movement.RotateDirection = _horizontal * transform.right;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontal) + Mathf.Abs(_vertical));

        _movement.Move(_moveAmount);
        _movement.Rotate();
        _animator.MovementAnimations(_moveAmount);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(1, 0, 0, 0.4f);
        Gizmos.DrawCube(transform.position + transform.forward * _rayDistance, _attackArea);
    }
}