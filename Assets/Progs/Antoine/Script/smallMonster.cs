using UnityEngine;
using UnityEngine.AI;

public class smallMonster : MonoBehaviour
{
    public enum State
    {
        PATROL,
        MOVING_TO_TARGET,
    }

    State currentState = State.PATROL;
    public NavMeshAgent agent;
    public GameObject player;

    public State LastState { get; protected set; } = State.PATROL;
    public float detectionRange { get; protected set; } = 5f;
    public State CurrentState
    {
        get { return currentState; }
        protected set
        {
            LastState = currentState;
            currentState = value;
        }
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Move(Transform target)
    {
        agent.SetDestination(target.position);
    }

    protected bool FinishedPath()
    {
        return agent.hasPath && agent.remainingDistance <= agent.stoppingDistance;
    }
}
