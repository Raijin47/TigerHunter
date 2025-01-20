using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : PoolMember
{
    #region Component
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Coroutine _coroutine;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public Transform Transform => _transform;
    public Vector3 StartPosition { get; set; }
    #endregion

    #region State
    protected StateIdle _stateIdle;
    private StatePatrol _statePatrol;

    public StatePatrol StatePatrol => _statePatrol;
    public StateIdle StateIdle => _stateIdle;

    #endregion
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _transform = transform;
        var startPos = transform.position;

        _stateIdle = new(this);
        _statePatrol = new(this);


        _stateIdle.OnEndIdle += ChangeState;
        _statePatrol.OnEndPatrol += ChangeState;

        Game.Action.OnEnter += () => ChangeState(_statePatrol);
        GetComponent<EnemySearch>().OnPlayerFound += Action_OnPlayerSearch;
    }

    protected abstract void Action_OnPlayerSearch(bool value);

    protected void ChangeState(IState state)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        state.Enter();
        _coroutine = StartCoroutine(state.UpdateProcess());
    }
}