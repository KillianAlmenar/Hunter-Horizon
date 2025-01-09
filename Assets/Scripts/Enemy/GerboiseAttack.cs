using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Gerboise
{
    [SerializeField] ScratchDamageCollider ScratchColliderRight;
    [SerializeField] ScratchDamageCollider ScratchColliderLeft;
    private enum ATTACK
    {
        SCRATCH,
        TAIL,
        NOTHING,
    }

    private float distanceRequired = 4;
    [SerializeField] private bool canDamage = false;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] private bool animTrigger = false;
    private float distanceOffset = 1f;
    [SerializeField]
    ATTACK currentAttack = ATTACK.NOTHING;
    private bool DamageDealed = false;

    public override void Attack()
    {
        if (!isAttacking)
        {
            agent.isStopped = false;
            if (cooldownAttack <= 0)
            {
                ChooseAttack();
                if (isEnrage)
                {
                    cooldownAttack = Random.Range(0.5f, 1f);
                }
                else
                {
                    cooldownAttack = Random.Range(1f, 1.5f);
                }
            }
            else
            {
                cooldownAttack -= Time.deltaTime;
            }

            Vector3 distanceToPlayer = transform.position - target.transform.position;

            if (distanceToPlayer.magnitude > distanceRequired)
            {
                agent.speed = maxSpeed;
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = distanceRequired;
            }

        }
        else
        {
            isAttackingDebug += Time.deltaTime;

            if (isAttackingDebug >= 10)
            {
                isAttacking = false;
            }

            Vector3 distanceToPlayer = transform.position - target.transform.position;



            if (distanceToPlayer.magnitude > (distanceRequired + distanceOffset))
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = distanceRequired + distanceOffset;
            }
            else if (distanceToPlayer.magnitude < (distanceRequired - distanceOffset))
            {
                if (!canDamage && !animTrigger)
                {
                    agent.isStopped = false;
                    agent.speed = maxSpeed / 2;
                    //Calculate the opposite of the player
                    Vector3 destination = new Vector3(target.transform.position.x - transform.position.x, transform.position.y, target.transform.position.z - transform.position.z);

                    agent.stoppingDistance = 0;

                    if (distanceToPlayer.magnitude < 1.5f)
                    {
                        destination *= 1.5f;
                    }

                    //Go to the opposite destination
                    agent.SetDestination(transform.position - destination);
                }
            }
            else
            {
                if (!animTrigger)
                {
                    switch (currentAttack)
                    {
                        case ATTACK.SCRATCH:
                            Scratch();
                            break;
                        case ATTACK.TAIL:
                            Tail();
                            break;
                    }
                }

            }
        }

        LookAtPlayer(this);
    }

    private void Scratch()
    {
        agent.isStopped = true;
        animator.SetTrigger("Scratch");

        animTrigger = true;
    }

    private void Tail()
    {
        agent.isStopped = true;
        animator.SetTrigger("Tail");

        animTrigger = true;
    }

    private void ChooseAttack()
    {
        agent.isStopped = false;

        currentAttack = (ATTACK)Random.Range(0, 2);

        switch (currentAttack)
        {
            case ATTACK.SCRATCH:
                distanceRequired = 1.5f;
                damage = 5;
                break;
            case ATTACK.TAIL:
                distanceRequired = 1f;
                damage = 10;
                break;

        }
        isAttacking = true;
        DamageDealed = false;

    }

    public void switchcanDamage()
    {
        if(!isDead)
        {
            canDamage = !canDamage;
            agent.isStopped = !agent.isStopped;
        }

    }

    public void StopAttack()
    {
        isAttacking = false;
        animTrigger = false;
        CheckFear();
    }

}