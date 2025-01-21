using System;
using System.Collections;
using UnityEngine;

public class StatePursuit : IState
{
    public event Action<IState> OnCanAttack;

    private readonly EnemyBase Enemy;

    private const float _speed = 3f;
    private const float _attackDistance = 3f;

    public StatePursuit(EnemyBase enemy) => Enemy = enemy;

    public void Enter()
    {
        Enemy.Agent.isStopped = false;
        Enemy.Agent.speed = _speed;
        Enemy.Animator.SetFloat("Velocity", 1f);
    }

    public IEnumerator UpdateProcess()
    {
        while (Vector3.Distance(Game.Locator.Player.transform.position, Enemy.Transform.position) > _attackDistance)
        {
            Enemy.Agent.destination = Game.Locator.Player.transform.position;
            yield return null;
        }

        OnCanAttack?.Invoke(Enemy.StateAttack);
    }
}