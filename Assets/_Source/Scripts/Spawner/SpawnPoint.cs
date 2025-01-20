using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public event Action<SpawnPoint, bool> OnUsedPoint;

    private bool _isUsed = false;
    public bool IsUsed 
    {
        get => _isUsed;
        private set
        {
            _isUsed = value;
            OnUsedPoint?.Invoke(this, value);
        }
    }

    private void OnTriggerEnter(Collider other) => IsUsed = true;
    private void OnTriggerExit(Collider other) => IsUsed = false;
}