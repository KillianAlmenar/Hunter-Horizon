using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : Enemy
{
    protected override void Update()
    {
        base .Update();
        if(hitCooldown > 0)
        {
            hitCooldown -= Time.deltaTime;
        }

    }

    public override void Attack()
    {
        
    }

    public override void Death()
    {
        
    }

    public override void Idle()
    {
        
    }

    public override void FindTarget()
    {
        
    }

    public override void Run()
    {
        
    }

    public override void Hit(float _damage)
    {
        if (hitCooldown <= 0)
        {
            animator.SetTrigger("hit");
            hitCooldown = 2f;
            agent.isStopped = true;

        }

    }

    public override void Target(Vector3 position)
    {
        
    }

    public void ActivateAgent()
    {

    }
}
