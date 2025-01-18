using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateIdle : IState
{
    public event Action<IState> OnEndIdle;

    private readonly EnemyBase Enemy;

    private const float _minIdleTime = 2f;
    private const float _maxIdleTime = 5f;

    public StateIdle(EnemyBase enemy) => Enemy = enemy;

    public void Enter()
    {
        Enemy.Animator.SetFloat("Velocity", 0);
        Enemy.Agent.isStopped = true;
    }

    public IEnumerator UpdateProcess()
    {
        float time = Random.Range(_minIdleTime, _maxIdleTime);

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        OnEndIdle?.Invoke(Enemy.StatePatrol);
    }
}