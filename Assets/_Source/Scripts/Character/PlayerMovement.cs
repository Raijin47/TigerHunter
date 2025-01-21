using System;
using UnityEngine;

[Serializable]
public class PlayerMovement
{
    [SerializeField, Range(1, 10)] private float _moveSpeed;
    [SerializeField, Range(0.1f, 10)] private float _rotateSpeed;

    private Rigidbody _rigidbody;

    public Vector3 MovementDirection { private get; set; }
    public Vector3 RotateDirection { private get; set; }

    public void Init(Rigidbody rb) => _rigidbody = rb;

    public void Move(float moveAmount)
    {
        _rigidbody.drag = moveAmount > 0 ? 0 : 4;
        _rigidbody.velocity = moveAmount * _moveSpeed * MovementDirection;
    }

    public void Rotate()
    {
        if (RotateDirection == Vector3.zero) return;

        Quaternion targetLock = Quaternion.LookRotation(RotateDirection);
        Quaternion targetRotation = Quaternion.Slerp(_rigidbody.rotation, targetLock, Time.fixedDeltaTime * _rotateSpeed);

        _rigidbody.MoveRotation(targetRotation);
    }

    public void Rotate(Vector3 target)
    {
        Quaternion targetLock = Quaternion.LookRotation(target);
        Quaternion targetRotation = Quaternion.Slerp(_rigidbody.rotation, targetLock, Time.fixedDeltaTime * _rotateSpeed);

        _rigidbody.MoveRotation(targetRotation);
    }
}