using System.Collections;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    public event Action<bool> OnChangeState;

    private const float RequiredTime = 30f;
    private float _currentTime;

    private Coroutine _coroutine;

    private void Start()
    {
        Game.Action.OnPause += Action_OnPause;
        Game.Action.OnStart += Action_OnStart;
        Game.Locator.Change.OnChangeState += Change_OnChangeState;
    }

    private void Change_OnChangeState(bool obj) => Action_OnStart();

    private void Action_OnStart()
    {
        _currentTime = RequiredTime;
        Release();
        _coroutine = StartCoroutine(UpdateTimer());
    }

    private void Action_OnPause(bool onPause)
    {
        Release();
        if (!onPause) _coroutine = StartCoroutine(UpdateTimer()); 
    }

    private IEnumerator UpdateTimer()
    {
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            yield return null;
        }

        OnChangeState?.Invoke(true);
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