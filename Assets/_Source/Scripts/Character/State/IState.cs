using System.Collections;

public interface IState
{
    public void Enter();
    public IEnumerator UpdateProcess();
}