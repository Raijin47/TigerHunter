using System;
using System.Collections;
using UnityEngine;

public class StateAttack : IState
{
    public event Action<IState> OnCannotAttack;

    private readonly EnemyBase Enemy;
    private readonly WaitForSeconds IntervalAttack = new(2f);
    private readonly WaitForSeconds Delay = new(0.5f);
    private const float _attackDistance = 3f;
    private LayerMask _layer;
    public StateAttack(EnemyBase enemy)
    {
        Enemy = enemy;
        _layer = LayerMask.GetMask("Player");
    }

    public void Enter()
    {
        Enemy.Animator.SetFloat("Velocity", 0);
        Enemy.Agent.isStopped = true;
    }

    public IEnumerator UpdateProcess()
    {
        while(Vector3.Distance(Game.Locator.Player.transform.position, Enemy.Transform.position) < _attackDistance)
        {
            Enemy.Transform.LookAt(Game.Locator.Player.transform);
            Enemy.Animator.SetTrigger("Attack");

            yield return Delay;

            if (Physics.CheckSphere(Enemy.transform.position, 3.5f, _layer))
                Game.Locator.Player.ApplyDamage(10);

            yield return IntervalAttack;
        }

        OnCannotAttack?.Invoke(Enemy.StatePursuit);
    }
}