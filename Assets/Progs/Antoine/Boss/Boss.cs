using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    public enum State
    {
        IDLE,
        WALKING,
        ATTACK,
        STURN,
        BATTLEIDLE,
        RUNNING,
        BITE,
        RUSH,
        HEAD,
        TAIL,
        BIGTAIL,
        ORB,
    }

    State currentState = State.IDLE;
    public GameObject player;

    public State LastState { get; protected set; } = State.IDLE;
    public float detectionRange { get; protected set; } = 5f;
    public float idleTimer { get; protected set; } = 2f;
    public float timer { get; protected set; } = 0f;
    public State CurrentState
    {
        get { return currentState; }
        protected set
        {
            LastState = currentState;
            currentState = value;
        }
    }
    protected virtual void SetTrigger(string state, State _state)
    {
        animator.SetTrigger(state);
        idleTimer = Random.Range(1f, 2f);
        timer = 0;
        currentState = _state;
    }
    protected virtual void Reset()
    {
        AnimatorControllerParameter[] parameters = animator.parameters;

        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    protected virtual void Move(Vector3 target, float speed, float acceleration)
    {
        agent.SetDestination(target);
        agent.speed = speed;
        agent.acceleration = acceleration;
    }
    protected bool FinishedPath()
    {
        return agent.hasPath && agent.remainingDistance <= agent.stoppingDistance;
    }
    public bool AnimationEnded()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
    }

    public override void Hit(float _damage)
    {

    }

    public override void Attack()
    {

    }

    public override void Death()
    {

    }

    public override void FindTarget()
    {

    }

    public override void Idle()
    {

    }

    public override void Run()
    {

    }

    public override void Target(Vector3 position)
    {

    }

}
