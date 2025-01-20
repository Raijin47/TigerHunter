using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private int _maxEnemyCount = 5;
    [SerializeField] private EnemyBase _pig;
    [SerializeField] private EnemyBase _orc;

    private readonly List<PoolMember> UsedEnemy = new();
    private readonly List<SpawnPoint> SpawnPoints = new();
    private readonly WaitForSeconds Interval = new(2f);
    private Coroutine _coroutine;

    private bool _isDangerousTime;
    private Pool _pigPool;
    private Pool _orcPool;

    private void Start()
    {
        _pigPool = new(_pig);
        _orcPool = new(_orc);

        foreach(SpawnPoint point in GetComponentsInChildren<SpawnPoint>())
        {
            point.OnUsedPoint += Point_OnUsedPoint;
            if (!point.IsUsed) SpawnPoints.Add(point);
        }


        Game.Action.OnStart += Action_OnStart;
    }

    private void Point_OnUsedPoint(SpawnPoint point, bool active)
    {
        if (active) SpawnPoints.Add(point);
        else SpawnPoints.Remove(point);
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
        Vector3 spawnPosition = SpawnPoints[^1].transform.position;

        EnemyBase enemy = _isDangerousTime ? 
            _pigPool.Spawn(spawnPosition) as EnemyBase :
            _orcPool.Spawn(spawnPosition) as EnemyBase;

        UsedEnemy.Add(enemy);
        enemy.Die += Enemy_Die;
        enemy.StartPosition = spawnPosition;
    }

    private void Enemy_Die(PoolMember obj)
    {
        UsedEnemy.Remove(obj);
        obj.Die -= Enemy_Die;
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