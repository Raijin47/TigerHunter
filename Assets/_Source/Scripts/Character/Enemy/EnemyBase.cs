using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : PoolMember
{
    #region Component
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _transform;
    private Vector3 _startPosition;
    private Coroutine _coroutine;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public Transform Transform => _transform;
    public Vector3 StartPosition => _startPosition;
    #endregion

    #region State
    private StateIdle _stateIdle;
    private StatePatrol _statePatrol;
    private StatePursuit _statePursuit;
    private StateAttack _stateAttack;

    public StatePatrol StatePatrol => _statePatrol;
    public StateIdle StateIdle => _stateIdle;
    public StateAttack StateAttack => _stateAttack;
    public StatePursuit StatePursuit => _statePursuit;
    #endregion

    public override void Init()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _transform = transform;
        _startPosition = _transform.position;
        var startPos = transform.position;

        _stateIdle = new(this);
        _statePatrol = new(this);
        _statePursuit = new(this);
        _stateAttack = new(this);

        _stateIdle.OnEndIdle += ChangeState;
        _statePatrol.OnEndPatrol += ChangeState;
        _statePursuit.OnCanAttack += ChangeState;
        _stateAttack.OnCannotAttack += ChangeState;

        Game.Action.OnEnter += () => ChangeState(_statePatrol);
        GetComponent<EnemySearch>().OnPlayerFound += Action_OnPlayerSearch;
    }

    private void Action_OnPlayerSearch(bool value) => ChangeState(value ? _statePursuit : _stateIdle);

    private void ChangeState(IState state)
    {
        ReleaseCoroutine();
        state.Enter();
        _coroutine = StartCoroutine(state.UpdateProcess());
    }
   
    private void ReleaseCoroutine()
    {
        if (_coroutine == null) return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}