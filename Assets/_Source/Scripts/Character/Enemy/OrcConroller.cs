public class OrcConroller : EnemyBase
{
    private StatePursuit _statePursuit;
    private StateAttack _stateAttack;

    public StateAttack StateAttack => _stateAttack;
    public StatePursuit StatePursuit => _statePursuit;

    public override void Init()
    {
        base.Init();

        _statePursuit = new(this);
        _stateAttack = new(this);

        _statePursuit.OnCanAttack += ChangeState;
        _stateAttack.OnCannotAttack += ChangeState;
    }

    protected override void Action_OnPlayerSearch(bool value)
    {
        ChangeState(value ? _statePursuit : _stateIdle);
    }
}