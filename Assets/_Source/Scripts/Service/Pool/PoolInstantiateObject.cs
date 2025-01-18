using System.Collections.Generic;
using UnityEngine;

public class PoolInstantiateObject<T> where T : Object
{
    private readonly List<T> UsedPoolInstantiate = new();
    private readonly List<T> FreePoolInstantiate = new();

    public T _data;

    public PoolInstantiateObject(T data)
    {
        _data = data;
    }

    public T Instantiate()
    {
        return Object.Instantiate(_data);
    }

    public (T, bool) GetInstantiate()
    {
        if (FreePoolInstantiate.Count <= 0)
        {
            var element = Instantiate();
            UsedPoolInstantiate.Add(element);
            return (element, true);
        }
        var elementGet = FreePoolInstantiate[^1];
        FreePoolInstantiate.Remove(elementGet);
        UsedPoolInstantiate.Add(elementGet);
        return (elementGet, false);
    }

    public void Release(T data)
    {
        UsedPoolInstantiate.Remove(data);
        FreePoolInstantiate.Add(data);
    }
}