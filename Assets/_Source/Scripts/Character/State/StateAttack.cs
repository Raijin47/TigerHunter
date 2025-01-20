using System;
using System.Collections;
using UnityEngine;

public class StateAttack : IState
{
    public event Action<IState> OnCannotAttack;

    private readonly OrcConroller Enemy;
    private readonly WaitForSeconds IntervalAttack = new(1.5f);
    private const float _attackDistance = 2.5f;

    public StateAttack(OrcConroller enemy) => Enemy = enemy;
    public void Enter()
    {
        Enemy.Animator.SetFloat("Velocity", 0);
        Enemy.Agent.isStopped = true;
    }

    public IEnumerator UpdateProcess()
    {
        while(Vector3.Distance(Game.Locator.Player.transform.position, Enemy.Transform.position) < _attackDistance)
        {
            Vector3 direction = Game.Locator.Player.transform.position - Enemy.Transform.position;
            Quaternion targetRot = Quaternion.LookRotation(direction);
            Enemy.Transform.rotation = Quaternion.Slerp(Enemy.Transform.rotation, targetRot, Time.deltaTime);

            Enemy.Animator.SetTrigger("Attack");
            yield return IntervalAttack;
        }

        OnCannotAttack?.Invoke(Enemy.StatePursuit);
    }
}