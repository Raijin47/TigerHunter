using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private int _maxEnemyCount = 5;
    [SerializeField] private EnemyBase _enemy;

    private readonly List<PoolMember> UsedEnemy = new();
    private readonly List<SpawnPoint> SpawnPoints = new();
    private readonly WaitForSeconds Interval = new(5f);

    private Coroutine _coroutine;
    private Pool _enemyPool;

    private bool _isDangerousTime;
    public bool IsDangerousTime => _isDangerousTime;

    private void Start()
    {
        _enemyPool = new(_enemy);

        foreach(SpawnPoint point in GetComponentsInChildren<SpawnPoint>())
        {
            point.OnUsedPoint += Point_OnUsedPoint;
            if (!point.IsUsed) SpawnPoints.Add(point);
        }

        Game.Action.OnStart += Action_OnStart;
        Game.Action.OnLose += Action_OnLose;
        Game.Locator.Change.OnChangeState += Change;
        Game.Locator.Timer.OnChangeState += Change;
    }

    private void Action_OnLose()
    {
        throw new System.NotImplementedException();
    }

    private void Point_OnUsedPoint(SpawnPoint point, bool active)
    {
        if (active) SpawnPoints.Remove(point);
        else SpawnPoints.Add(point);
    }

    private void Action_OnStart()
    {
        Release();
        _isDangerousTime = false;
        _coroutine = StartCoroutine(SpawnProcess());
    }

    private IEnumerator SpawnProcess()
    {
        while(true)
        {
            yield return new WaitWhile(() => UsedEnemy.Count >= _maxEnemyCount);
            yield return Interval;
            Spawn();
        }
    }

    private void Spawn()
    {
        int r = Random.Range(0, SpawnPoints.Count);
        Vector3 spawnPosition = SpawnPoints[r].transform.position;
        EnemyBase enemy = _enemyPool.Spawn(spawnPosition) as EnemyBase;

        UsedEnemy.Add(enemy);
        enemy.Mode = _isDangerousTime ? 1 : 0;
        enemy.Die += Enemy_Die;
        enemy.StartPosition = spawnPosition;
    }

    private void Change(bool value)
    {
        _isDangerousTime = value;

        foreach (EnemyBase enemy in UsedEnemy)
            enemy.Change();
    }

    private void Enemy_Die(PoolMember enemy)
    {
        UsedEnemy.Remove(enemy);
        enemy.Die -= Enemy_Die;
    }

    private void Release()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}