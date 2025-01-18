using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _startPosition;


    private void Start()
    {
        _startPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        Game.Action.OnEnter += Action_OnEnter;
        GetComponent<EnemySearchSupport>().OnPlayerSearch += Action_OnPlayerSearch;
    }

    private void Action_OnEnter()
    {
        _agent.destination = GetTarget();

        _agent.SetDestination(GetTarget());
    }

    private void Update()
    {

    }

    private void Action_OnPlayerSearch()
    {
        Debug.Log("SearchPlayer");
    }
    
    private Vector3 GetTarget()
    {
        return _startPosition + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }
}