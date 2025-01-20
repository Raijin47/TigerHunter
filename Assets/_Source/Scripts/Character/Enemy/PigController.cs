public class PigController : EnemyBase
{
    private StatePanic _statePanic;

    public override void Init()
    {
        base.Init();

        _statePanic = new(this);
    }

    protected override void Action_OnPlayerSearch(bool value)
    {
        ChangeState(value ? _statePanic : _stateIdle);
    }
}