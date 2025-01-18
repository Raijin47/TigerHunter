using System.Collections.Generic;
using UnityEngine;

public class PoolProvider
{
    private readonly List<PoolMember> Members = new();
    private readonly PoolInstantiateObject<PoolMember> PoolInstantiateObject;
    private readonly PoolFactory PoolFactory;

    public PoolProvider(PoolInstantiateObject<PoolMember> poolInstantiateObject)
    {
        PoolFactory = new PoolFactory(poolInstantiateObject);
        PoolInstantiateObject = poolInstantiateObject;
    }

    private void InitPoolMember(PoolMember member)
    {
        member.gameObject.SetActive(true);
        member.Die += OnDie;
        member.Init();
    }

    private void OnDie(PoolMember poolMember)
    {
        Remove(poolMember);
    }

    public PoolMember Create(Vector3 position)
    {
        var unitData = PoolFactory.Spawn(position);
        var unitBase = unitData.Item1;
        if (unitBase == null) return unitBase;
        var isInstantiate = unitData.Item2;
        if (isInstantiate) Add(unitBase);
        else ResetPoolUnit(unitBase);
        return unitBase;
    }

    private void ResetPoolUnit(PoolMember member)
    {
        member.gameObject.SetActive(true);
        member.Resurrect();
    }

    public void Add(PoolMember member)
    {
        Members.Add(member);
        InitPoolMember(member);
    }

    public void Remove(PoolMember member)
    {
        member.gameObject.SetActive(false);
        Members.Remove(member);
        member.Release();
        PoolInstantiateObject.Release(member);
    }
}