using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

abstract public class Enemy : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] protected float hp = 100;
    protected float maxHp = 100;
    [SerializeField] protected float maxSpeed = 20;
    [SerializeField] protected float damage = 5;
    [SerializeField] protected float cooldownAttack = 2;
    protected float currentSpeed;
    [SerializeField] protected STATE state;
    protected bool isLeader = false;
    protected float animationSpeed = 0;
    [HideInInspector] public Vector3 destination = new Vector3();
    protected bool isDead = false;
    [SerializeField] protected bool isHit = false;
    [SerializeField] protected float hitCooldown = 0;
    [SerializeField] protected string id = " ";
    [SerializeField] public Elements.Element element;


    [Header("References")]
    [SerializeField] public List<Item> reward;
    protected NavMeshAgent agent;
    protected GameObject target;
    protected FieldOfView fov;
    protected Animator animator;
    [SerializeField] protected Material glowMat;
    protected Rigidbody rb;

    public enum STATE
    {
        IDLE,
        TARGET,
        ATTACK,
        RUN,
        ENCIRCLE,
        DEATH
    }

    protected virtual void Start()
    {
        fov = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        agent.speed = maxSpeed;
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            currentSpeed = agent.velocity.magnitude;
        }

        animationSpeed = Mathf.Max(0.59f, currentSpeed);

        switch (state)
        {
            case STATE.IDLE:
                Idle();
                break;
            case STATE.TARGET:
                Target(destination);
                break;
            case STATE.ATTACK:
                Attack();
                break;
            case STATE.RUN:
                Run();
                break;
            case STATE.DEATH:
                Death();
                break;
        }
    }

    abstract public void Run();
    abstract public void Idle();
    abstract public void FindTarget();
    abstract public void Target(Vector3 position);
    abstract public void Attack();
    abstract public void Death();
    abstract public void Hit(float _damage);

}
