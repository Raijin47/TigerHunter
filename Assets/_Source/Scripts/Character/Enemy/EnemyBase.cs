using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : PoolMember
{
    public event Action<int> OnTakeDamage;

    #region Component
    [SerializeField] private GameObject[] _skins;
    [SerializeField] private Animator[] _animators;
    [SerializeField] private ParticleSystem _particle;

    private NavMeshAgent _agent;
    private Transform _transform;
    private Coroutine _coroutine;
    private EnemySearch _enemySearch;

    private int _mode;
    public int Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            bool isPig = value == 0;

            _skins[0].SetActive(isPig);
            _skins[1].SetActive(!isPig);
            _agent.radius = isPig ? .5f : .7f;
            _agent.height = isPig ? .7f : 2f;
        }
    }
    public Animator Animator => _animators[Mode];
    public NavMeshAgent Agent => _agent;
    public Transform Transform => _transform;
    public Vector3 StartPosition { get; set; }
    #endregion

    #region State
    private StateIdle _stateIdle;
    private StatePatrol _statePatrol;
    private StatePanic _statePanic;
    private StatePursuit _statePursuit;
    private StateAttack _stateAttack;

    public StateAttack StateAttack => _stateAttack;
    public StatePursuit StatePursuit => _statePursuit;
    public StatePatrol StatePatrol => _statePatrol;
    public StateIdle StateIdle => _stateIdle;

    #endregion
    public override void Init()
    {
        _agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        _enemySearch = GetComponent<EnemySearch>();

        _stateIdle = new(this);
        _statePatrol = new(this);
        _statePanic = new(this);
        _statePursuit = new(this);
        _stateAttack = new(this);

        Resurrect();
    }

    public override void Resurrect()
    {
        Mode = Game.Locator.Spawner.IsDangerousTime ? 1 : 0;

        _statePursuit.OnCanAttack += ChangeState;
        _stateAttack.OnCannotAttack += ChangeState;
        _stateIdle.OnEndIdle += ChangeState;
        _statePatrol.OnEndPatrol += ChangeState;
        _enemySearch.OnPlayerFound += Action_OnPlayerSearch;

        ChangeState(_statePatrol);
    }

    public void Change()
    {
        Mode = Game.Locator.Spawner.IsDangerousTime ? 1 : 0;
        _particle.Play();
        
        ChangeState(_enemySearch.IsViewPlayer ? (Mode == 0 ? _statePanic : _statePursuit) : _stateIdle);
    }

    public override void Release()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public void ApplyDamage(int value)
    {
        OnTakeDamage?.Invoke(value);
        Debug.Log("TakeDamage");
    }

    private void Action_OnPlayerSearch(bool value)
    {
        ChangeState(value ? (Mode == 0 ? _statePanic : _statePursuit) : _stateIdle);
    }

    private void ChangeState(IState state)
    {
        Release();

        state.Enter();
        _coroutine = StartCoroutine(state.UpdateProcess());
    }
}