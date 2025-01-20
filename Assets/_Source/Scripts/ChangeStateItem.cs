using System;
using UnityEngine;

public class ChangeStateItem : MonoBehaviour
{
    public event Action<bool> OnChangeState;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerBase _))
            OnChangeState?.Invoke(false);
    }
}