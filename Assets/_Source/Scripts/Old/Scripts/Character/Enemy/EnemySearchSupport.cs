using System;
using System.Collections;
using UnityEngine;

public class EnemySearchSupport : MonoBehaviour
{
    public event Action OnPlayerSearch;

    [SerializeField] private LayerMask _layer;
    [SerializeField, Range(1, 50)] private float _viewRadius = 10f;

    private readonly WaitForSeconds Delay = new(.5f);
    private Coroutine _coroutine;

    private void Start()
    {
        Game.Action.OnEnter += Action_OnEnter;
        Game.Action.OnExit += Release;
        Game.Action.OnPause += Action_OnPause;
    }

    private void Action_OnPause(bool onPause)
    {
        Release();

        if (!onPause) Action_OnEnter();
    }

    private void Action_OnEnter()
    {
        Release();

        _coroutine = StartCoroutine(CheckPlayerCoroutine());
    }

    private IEnumerator CheckPlayerCoroutine()
    {
        while(true)
        {
            if (Physics.SphereCast(transform.position, _viewRadius, Vector3.zero, out RaycastHit hit, _layer))            
                OnPlayerSearch?.Invoke();
            
            yield return Delay;
        }
    }

    private void Release()
    {
        if (_coroutine == null) return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .2f);
        Gizmos.DrawSphere(transform.position, _viewRadius);
    }
}