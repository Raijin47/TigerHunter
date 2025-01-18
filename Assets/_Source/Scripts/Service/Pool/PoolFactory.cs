using UnityEngine;

public class PoolFactory
{
    private readonly PoolInstantiateObject<PoolMember> Member;

    public PoolFactory(PoolInstantiateObject<PoolMember> member) => Member = member;

    public (PoolMember, bool) Spawn(Vector3 position)
    {
        var obj = Member.GetInstantiate();
        if (obj.Item1 != null)
        {
            var transform = obj.Item1.transform;
            transform.SetParent(Game.Instance.PoolContainer, false);
            transform.position = position;
        }
        return obj;
    }
}