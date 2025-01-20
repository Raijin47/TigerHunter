using System;
using UnityEngine;

[Serializable]
public class Locator
{
    [SerializeField] private PlayerBase _player;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private Timer _timer;
    [SerializeField] private ChangeStateItem _change;

    public PlayerBase Player => _player;
    public EnemySpawner Spawner => _spawner;
    public Timer Timer => _timer;
    public ChangeStateItem Change => _change;
}