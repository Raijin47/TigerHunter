using System.Collections;
using UnityEngine;
using System;

public class EnemySearch : MonoBehaviour
{
    public event Action<bool> OnPlayerFound;

    [SerializeField] private LayerMask _layer;

    private Coroutine _coroutine;
    [SerializeField, Range(1, 50)] private float _viewRadius = 10f;
    private readonly WaitForSeconds Delay = new(.5f);
    private bool _isViewPlayer;
    private void Start()
    {
        Game.Action.OnEnter += Action_OnEnter;
        Game.Action.OnExit += Release;
        Game.Action.OnPause += Action_OnPause;
    }

    private void Action_OnEnter()
    {
        Release();

        _coroutine = StartCoroutine(SearchProcessCoroutine());
    }
    private void Action_OnPause(bool onPause)
    {
        Release();

        if (!onPause) Action_OnEnter();
    }
    private IEnumerator SearchProcessCoroutine()
    {
        while (true)
        {
            if(_isViewPlayer)
            {
                if (!Physics.CheckSphere(transform.position, _viewRadius, _layer))
                {
                    _isViewPlayer = false;
                    OnPlayerFound?.Invoke(_isViewPlayer);
                }
            }
            else
            {
                if (Physics.CheckSphere(transform.position, _viewRadius, _layer))
                {
                    _isViewPlayer = true;
                    OnPlayerFound?.Invoke(_isViewPlayer);
                }
            }

            yield return Delay;
        }
    }

    private void Release()
    {
        if (_coroutine == null) return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}