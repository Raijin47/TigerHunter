using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField, Range(1, 10)] private float _followSpeed;
    private Transform _transform;

    private void Awake() => _transform = transform;

    private void LateUpdate()
    {
        Vector3 targetPosition = Vector3.Lerp(_transform.position, _target.position, Time.deltaTime * _followSpeed);
        _transform.position = targetPosition;

        //Quaternion targetRotation = Quaternion.Lerp(_transform.rotation, _target.rotation, Time.deltaTime * _followSpeed);
        //_transform.rotation = targetRotation;
    }
}