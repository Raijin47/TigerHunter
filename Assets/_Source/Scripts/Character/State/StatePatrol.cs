using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class StatePatrol : IState
{
    public event Action<IState> OnEndPatrol;

    private readonly EnemyBase Enemy;

    private const float _speed = 0.7f;
    private const float _patrolDistance = 20f;

    public StatePatrol(EnemyBase enemy) => Enemy = enemy;

    public void Enter()
    {
        Enemy.Agent.isStopped = false;
        Enemy.Agent.speed = _speed;
        Enemy.Agent.destination = GetPath();
        Enemy.Animator.SetFloat("Velocity", 0.5f);
    }

    public IEnumerator UpdateProcess()
    {
        while(Enemy.Agent.remainingDistance > 0.5f)     
            yield return null;
        
        OnEndPatrol?.Invoke(Enemy.StateIdle);
    }

    private Vector3 GetPath()
    {
        return Enemy.StartPosition + new Vector3(Random.Range(-_patrolDistance, _patrolDistance), 0, Random.Range(-_patrolDistance, _patrolDistance));
    }
}