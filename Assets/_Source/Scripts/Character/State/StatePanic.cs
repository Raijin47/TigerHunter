using System.Collections;
using UnityEngine;

public class StatePanic : IState
{
    private readonly EnemyBase Enemy;

    private const float _distance = 10f;
    private const float _speed = 3f;
    private readonly WaitForSeconds Interval = new(.5f);
    public StatePanic(EnemyBase enemy) => Enemy = enemy;

    public void Enter()
    {
        Enemy.Agent.isStopped = false;
        Enemy.Agent.speed = _speed;
        Enemy.Animator.SetFloat("Velocity", 1f);
    }

    public IEnumerator UpdateProcess()
    {
        while (true)
        {
            Enemy.Agent.destination = (Enemy.Transform.position - Game.Locator.Player.transform.position) * _distance;

            yield return Interval;
        }
    }
}