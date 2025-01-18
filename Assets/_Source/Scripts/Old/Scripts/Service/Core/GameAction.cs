using System;

public class GameAction
{
    public event Action OnStart;
    public event Action<bool> OnPause;
    public event Action OnWin;
    public event Action OnLose;
    public event Action OnRestart;
    public event Action OnEnter;
    public event Action OnExit;
    public event Action OnNext;

    public void SendStart() => OnStart?.Invoke();
    public void SendPause(bool isPause) => OnPause?.Invoke(isPause);
    public void SendLose() => OnLose?.Invoke();
    public void SendWin() => OnWin?.Invoke();
    public void SendRestart() => OnRestart?.Invoke();
    public void SendEnter() => OnEnter?.Invoke();
    public void SendExit() => OnExit?.Invoke();
    public void SendNext() => OnNext?.Invoke();
}