using UnityEngine;

public class Pool
{
    private readonly PoolInstantiateObject<PoolMember> InstatntiateObject;
    private readonly PoolProvider Provider;
    
    public Pool(PoolMember poolMember)
    {
        InstatntiateObject = new PoolInstantiateObject<PoolMember>(poolMember);
        Provider = new PoolProvider(InstatntiateObject);
    }

    public PoolMember Spawn(Vector3 position)
    {
        return Provider.Create(position);
    }
}