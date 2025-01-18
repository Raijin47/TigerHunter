using DG.Tweening;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField, Range(1,10)] private float _followSpeed;

    [SerializeField] private Transform _pivot;

    private Transform _transform;
    private Sequence _sequence;

    private readonly Vector3 MenuPos = new(0, 0.5f, -2.3f);
    private readonly Vector3 GamePos = new(0, 10, -6);
    private readonly Vector3 MenuRot = new(-1, 0, 0);
    private readonly Vector3 GameRot = new(60, 0, 0);
    private readonly float Duration = 2f;

    private void Awake() => _transform = transform;
    private void Start()
    {
        //Game.Action.OnEnterGame += () => IsGame = true;
        //Game.Action.OnExitGame += () => IsGame = false;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = Vector3.Lerp(_transform.position, _target.position, Time.deltaTime * _followSpeed);
        _transform.position = targetPosition;
    }

    private bool IsGame
    {
        set
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.
                Append(transform.DORotate(new Vector3(0, value ? 0 : 180, 0), Duration / 2)).
                Join(_pivot.DOLocalRotate(value ? GameRot : MenuRot, Duration)).
                Join(_pivot.DOLocalMove(value ? GamePos : MenuPos, Duration));
        }
    }

}