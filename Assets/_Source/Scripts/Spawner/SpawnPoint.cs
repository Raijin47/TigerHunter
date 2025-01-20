using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool _isUsed = false;
    
    public event Action<SpawnPoint, bool> OnUsedPoint;

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