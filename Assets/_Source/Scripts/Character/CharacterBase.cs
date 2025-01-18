using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected CharacterMovement _movement;
    protected CharacterAnimation _animator;
    private bool _isPause;
    private Vector3 _startPosition;

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
        _startPosition = transform.position;

        Game.Action.OnPause += OnPause;
        Game.Action.OnLose += OnLose;
        Game.Action.OnWin += OnWin;
        Game.Action.OnRestart += Restart;

        Init();
    }
    protected abstract void Init();
    protected void Update()
    {
        if (IsPause) return;
        GetInput();
    }
    protected void FixedUpdate()
    {
        if (IsPause) return;
        Movement();
    }
    protected void Restart()
    {
        _animator.Reset();

        var rb = GetComponent<Rigidbody>();
        rb.position = _startPosition;
        rb.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnWin()
    {
        _animator.OnWin = true;
        IsPause = true;
    }

    private void OnLose()
    {
        _animator.OnLose = true;
        IsPause = true;
    }

    protected abstract void GetInput();
    protected abstract void Movement();

    protected void OnPause(bool pause)
    {
        IsPause = pause;
        _animator.IsActive = !pause;
    }
}