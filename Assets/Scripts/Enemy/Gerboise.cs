using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public partial class Gerboise : Enemy
{
    [SerializeField] int targetingRange = 5;
    [SerializeField] int checkAllyRange = 15;
    [SerializeField] float EncircleRadius = 5;
    [SerializeField] bool isAvailable = true; //available to fight with other monsters
    [SerializeField] float stoppingDistanceOffset = 0;
    [SerializeField] AnimationCurve RageCurve;
    [SerializeField] protected float fearMeter = 100;
    [SerializeField] EnemySoundPlayer soundPlayer;

    [SerializeField] float idleTime;
    float rageMultiplier = 1.5f;
    bool isEnrage = false;
    int fearMax = 95;
    float Ragetimer = 0;
    float RageCooldown = 20;
    float destroyTimer = 0;
    private bool gotoplayer = false;
    [SerializeField] protected Gerboise Leader;

    [SerializeField] List<Gerboise> allyArray;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Material[] materials;
    private Color RageColor;

    private Camera mainCamera;
    private bool isInCameraField = true;
    private float isAttackingDebug = 0;

    private bool coroutineStarted = true;
    private bool stopAggro = false;

    protected override void Start()
    {
        //start of enemy
        base.Start();
        FindAlly();
        stoppingDistanceOffset = Random.Range(2, 6);

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        float scale = Random.Range(1, 2);
        transform.localScale = new Vector3(scale, scale, scale);

        StartCoroutine("FindAllAlly");

        cooldownAttack = Random.Range(1f, 2.5f);

        materials = skinnedMeshRenderer.materials;

        idleTime = Random.Range(0, 10);

        element = Inventory.instance.GetQuestElements();

        switch (element)
        {
            case Elements.Element.FIRE:
                materials[0].SetColor("Color_39b4056597f14ccf88c570d07791ea17", new Color(1, 0.25f, 0.25f, 1));
                RageColor = new Color(255, 0, 0, 1);
                id = "gerboise_fire";
                break;
            case Elements.Element.ICE:
                materials[0].SetColor("Color_39b4056597f14ccf88c570d07791ea17", new Color(0.25f, 0.25f, 1, 1));
                RageColor = new Color(0, 125, 255, 1);
                id = "gerboise_ice";
                break;
            case Elements.Element.WIND:
                materials[0].SetColor("Color_39b4056597f14ccf88c570d07791ea17", new Color(0.25f, 1, 0.25f, 1));
                RageColor = new Color(0, 255, 0, 1);
                id = "gerboise_wind";
                break;
            case Elements.Element.ELECTRICITY:
                materials[0].SetColor("Color_39b4056597f14ccf88c570d07791ea17", new Color(1, 1, 0.25f, 1));
                RageColor = new Color(255, 255, 0, 1);
                id = "gerboise_electricity";
                break;
        }



        transform.Rotate(new Vector3(0, Random.Range(0, 180), 0));
    }

    protected override void Update()
    {

        if (mainCamera != null)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                viewportPoint.z > 0)
            {
                if (!isInCameraField)
                {
                    isInCameraField = true;
                }
            }
            else
            {
                if (isInCameraField)
                {
                    isInCameraField = false;
                }
            }
        }

        if (RageCooldown > 0)
        {
            RageCooldown -= Time.deltaTime;
        }

        if (hp <= 0)
        {
            state = STATE.DEATH;
        }

        //Update of enemy
        base.Update();
        animator.SetFloat("Speed", animationSpeed);

        soundPlayer.playIdleSounds = state == STATE.IDLE;
        soundPlayer.speed = currentSpeed / maxSpeed;

        if (canDamage && state == STATE.ATTACK)
        {
            soundPlayer.PlayAttackSound();
        }
        if (state == STATE.ATTACK)
        {
            MusicManager.instance.BeginFight();
        }

        if (state == STATE.ENCIRCLE)
        {
            if (allyArray.Count <= 0)
            {
                state = STATE.IDLE;
            }
            GoToPlayer();
        }

        //If collide with player when is attacking
        if ((ScratchColliderRight != null || ScratchColliderLeft != null) && canDamage)
        {
            if ((ScratchColliderRight.playerCollision || ScratchColliderLeft.playerCollision) && !DamageDealed)
            {
                DamagePlayer();
            }
        }

        if (isEnrage)
        {
            RageAnimation();
        }

        hitCooldown -= Time.deltaTime;

        if (GetComponentInChildren<DissolvingController>().endCoroutine)
        {
            destroyTimer += Time.deltaTime;
        }

        if (destroyTimer > 3)
        {
            QuestManager.Instance.UpdatePlayerQuest(id);
            Destroy(transform.parent.gameObject);
        }

        if (isInCameraField)
        {
            if (!coroutineStarted)
            {
                StartCoroutine("FindAllAlly");
                coroutineStarted = true;
            }
        }
        else
        {
            if (coroutineStarted)
            {
                StopCoroutine("FindAllAlly");
                coroutineStarted = false;
            }
        }
        
        if (state == STATE.IDLE)
        {
            soundPlayer.StartDigging();
        }
        else
        {
            soundPlayer.StopDigging();
        }
    }

    public override void Idle()
    {
        if (isInCameraField)
        {

            //Time of before change position
            idleTime -= Time.deltaTime;

            //Change position at the end of timer
            if (idleTime <= 0)
            {
                agent.speed = maxSpeed / 2;
                agent.stoppingDistance = 2;
                //Change position if the previous change is finished

                rb.velocity = Vector3.zero;
                //Calculate new position
                Vector3 position = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
                NavMeshHit hit;
                if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.SetDestination(position);
                    idleTime = Random.Range(5, 10);

                    int random = Random.Range(1, 3);
                    if (random == 1)
                    {
                        animator.SetBool("Sniff", true);
                    }
                    else
                    {
                        animator.SetBool("Sniff", false);
                    }

                }
            }



            //Reduce fearMeter until 50
            if (fearMeter > 50)
            {
                fearMeter -= 0.005f;
            }
            //Available when fear is below fearMax
            if (fearMeter < fearMax && !isAvailable)
            {
                isAvailable = true;
            }
            if (!stopAggro)
            {
                //Try to find the player
                FindTarget();
            }
        }
    }

    private void Rage()
    {
        //Boost speed and damage
        if (!isEnrage)
        {
            agent.angularSpeed *= rageMultiplier;
            damage *= rageMultiplier;
            GetComponentInChildren<SkinnedMeshRenderer>().materials[1].SetColor("_RageColor", new Color(255, 0, 0, 1));
            isEnrage = true;
        }
    }

    private void stopRage()
    {
        if (isEnrage)
        {
            agent.angularSpeed /= rageMultiplier;
            damage /= rageMultiplier;
            GetComponentInChildren<SkinnedMeshRenderer>().materials[1].SetColor("_RageColor", new Color(0, 0, 0, 1));
            isEnrage = false;
        }
    }

    private void RageAnimation()
    {
        Ragetimer += Time.deltaTime * 2;

        materials[1].SetColor("_RageColor", RageColor * RageCurve.Evaluate(Ragetimer));

        if (Ragetimer / 2 > 10)//Reset rage
        {
            isEnrage = false;
            Ragetimer = 0;
            agent.angularSpeed /= rageMultiplier;
            damage /= rageMultiplier;
            fearMeter = 30;
            materials[1].SetColor("_RageColor", new Color(0, 0, 0, 1));
        }
    }

    private void GoToPlayer()
    {
        if (isLeader)
        {
            gotoplayer = true;
            List<Gerboise> allyAvailable = new List<Gerboise>();
            allyAvailable.Add(this);  // Add leader to the list

            foreach (Gerboise mob in allyArray)
            {
                if (mob != this && mob.state != STATE.RUN && mob.fearMeter < fearMax && !allyAvailable.Contains(mob))
                {
                    allyAvailable.Add(mob);
                }
            }

            foreach (Gerboise mob in allyAvailable)
            {
                if (!mob.isAttacking && !mob.isDead)
                {
                    mob.agent.stoppingDistance = 5 + mob.stoppingDistanceOffset;

                    LookAtPlayer(mob);

                    if (mob.target != null)
                    {
                        Vector3 distanceToPlayer = mob.transform.position - mob.target.transform.position;

                        if (distanceToPlayer.magnitude < 3 + mob.stoppingDistanceOffset)
                        {
                            mob.agent.stoppingDistance = 0;
                            //Go to the opposite destination
                            mob.agent.speed = mob.maxSpeed / 2;
                            Vector3 destination = new Vector3(mob.target.transform.position.x - mob.transform.position.x, mob.transform.position.y, mob.target.transform.position.z - mob.transform.position.z);

                            Vector3 distanceDestination = destination - mob.agent.destination;

                            if (distanceDestination.magnitude > 0.5f)
                            {
                                destination *= 2;
                                mob.agent.SetDestination(mob.transform.position - destination);
                            }
                        }
                        if (distanceToPlayer.magnitude < 8 + mob.stoppingDistanceOffset)
                        {
                            Encircle();
                        }
                        else
                        {
                            mob.agent.SetDestination(target.transform.position);
                        }
                    }
                }

            }
        }
        else
        {
            if (Leader != null)
            {
                if (!Leader.gotoplayer)
                {
                    Leader.GoToPlayer();
                }
            }
            else
            {
                isLeader = true;
            }
        }
    }

    private void Encircle()
    {
        #region FindAvailableAlly
        List<Gerboise> allyAvailable = new List<Gerboise>();
        allyAvailable.Add(this);  // Add leader to the list

        foreach (Gerboise mob in allyArray)
        {
            if (mob != this && mob.state != STATE.RUN && mob.fearMeter < fearMax && !allyAvailable.Contains(mob))
            {
                allyAvailable.Add(mob);
            }
        }
        #endregion

        //Unfreeze all ally
        foreach (Gerboise mob in allyArray)
        {
            if (!mob.isDead)
            {
                if (mob.agent.isStopped)
                {
                    mob.agent.isStopped = false;
                }
            }
        }

        // Find position around target
        List<Vector3> positions = CalculateCirclePositions(target.gameObject.transform.position, EncircleRadius, allyAvailable.Count);
        List<Vector3> takenPositions = new List<Vector3>();
        int nmbMobEncircle = 0;

        for (int i = 0; i < allyAvailable.Count; i++)
        {
            Gerboise mob = allyAvailable[i];

            if (positions.Count == 0)
            {
                break;  // No position available
            }

            if (mob.isDead)
            {
                continue;
            }

            // Find nearest position for each mob
            Vector3 closestPosition = positions[0];
            float closestDistance = Vector3.Distance(mob.transform.position, closestPosition);

            foreach (Vector3 position in positions)
            {
                // Check if position is already taken
                if (!takenPositions.Contains(position))
                {
                    float distanceToPosition = Vector3.Distance(mob.transform.position, position);

                    if (distanceToPosition < closestDistance)
                    {
                        closestPosition = position;
                        closestDistance = distanceToPosition;
                    }
                }
            }


            Vector3 distancePosition = mob.transform.position - closestPosition;
            if (distancePosition.magnitude <= 1)
            {
                LookAtPlayer(mob);
            }


            if (!mob.isAttacking)
            {
                Vector3 mobToDestination = closestPosition - mob.transform.position;

                float scalaire = Vector3.Dot(mobToDestination, mob.transform.forward);

                if (scalaire > 0)
                {
                    mob.agent.speed = mob.maxSpeed;
                }
                else
                {
                    mob.agent.speed = mob.maxSpeed / 2;
                }
                mob.agent.stoppingDistance = 1 + Mathf.Max(mob.stoppingDistanceOffset, 4);

                Vector3 distanceNewDestination = closestPosition - mob.agent.destination;
                
                if(distanceNewDestination.magnitude >= 5)
                {
                    mob.agent.SetDestination(closestPosition);
                }

            }


            Vector3 distance = closestPosition - mob.transform.position;

            if (distance.magnitude <= mob.agent.stoppingDistance && mob != this && !mob.isDead)
            {
                mob.state = STATE.ATTACK;
            }

            if (mob.state == STATE.ENCIRCLE)
            {
                nmbMobEncircle++;
            }

            // Remove position if already taken
            takenPositions.Add(closestPosition);
            positions.Remove(closestPosition);
        }
   
        if (nmbMobEncircle <= 1)
        {
            state = STATE.ATTACK;
        }
    }

    public void LookAtPlayer(Gerboise mob)
    {
        if (target != null)
        {
            Vector3 lookPosAlly = target.transform.position - mob.transform.position;
            lookPosAlly.y = 0;
            Quaternion rotationAlly = Quaternion.LookRotation(lookPosAlly);
            mob.transform.rotation = Quaternion.Slerp(mob.transform.rotation, rotationAlly, Time.deltaTime * 4);
        }
    }

    public override void Target(Vector3 position)
    {
        //Target != null when player is near
        if (target != null)
        {
            CheckFear();
        }
        else//If player isnt near return to idle
        {
            state = STATE.IDLE;
            return;
        }
        if (allyArray.Count > 0)
        {
            if (agent.remainingDistance <= 3f)//Attack if near of player
            {
                cooldownAttack = 0;
                state = STATE.ATTACK;
            }
            cooldownAttack = 0;
            state = STATE.ATTACK;
        }
        else
        {
            if (agent.remainingDistance <= 3f)//Attack if near of player
            {
                state = STATE.ATTACK;
            }
            state = STATE.ATTACK;
        }

    }

    public override void Run()
    {
        //Calculate the opposite of the player
        Vector3 destination = new Vector3(target.transform.position.x - transform.position.x, transform.position.y, target.transform.position.z - transform.position.z);
        Vector3 playerDistance = target.transform.position - transform.position;

        agent.stoppingDistance = 0;

        if (playerDistance.magnitude < 2)
        {
            destination *= 5;
        }

        //Go to the opposite destination
        agent.SetDestination(transform.position - destination);
        agent.speed = maxSpeed;

        //Stop running if far enought
        if (playerDistance.magnitude > checkAllyRange && fearMeter > 50)
        {
            agent.ResetPath();
            state = STATE.IDLE;
        }
    }

    public override void FindTarget()
    {
        //Sphere of collision 
        Collider[] hits = Physics.OverlapSphere(transform.position, targetingRange);

        //Player is in cone of vision
        if (fov.canSeePlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            CheckFear();
        }

        //Check if player is near
        foreach (Collider hit in hits)
        {
            if (hit.tag == ("Player"))
            {
                target = hit.gameObject;
                CheckFear();
            }
        }

        if (target != null)
        {
            foreach (Gerboise mob in allyArray)
            {
                mob.target = target;
                mob.CheckFear();
            }
        }
    }

    private void FindAlly()
    {
        if (state != STATE.ENCIRCLE && allyArray.Count > 0)
        {
            state = STATE.ENCIRCLE;
        }
        else if (target != null && state != STATE.TARGET)
        {
            state = STATE.TARGET;
        }
    }

    private IEnumerator FindAllAlly()
    {
        while (true && state != STATE.DEATH)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, checkAllyRange);
            int numberAlly = 0;

            //Detect all mob to find near ally
            foreach (Collider hit in hits)
            {
                if (hit.GetComponent<Gerboise>() != null && hit.GetComponent<Gerboise>() != this)
                {
                    numberAlly++;
                    if (!allyArray.Contains(hit.GetComponent<Gerboise>()) && hit.GetComponent<Gerboise>().isAvailable && hit.GetComponent<Gerboise>().state != STATE.DEATH && !hit.GetComponent<Gerboise>().isDead)
                    {
                        allyArray.Add(hit.GetComponent<Gerboise>());
                    }
                }
            }

            if (allyArray.Count > 0 && numberAlly == 0)
            {
                foreach (Gerboise mob in allyArray)
                {
                    mob.allyArray.Remove(this);
                }
                allyArray.Clear();
            }

            List<Gerboise> mobToRemove = new List<Gerboise>();


            if (isAvailable)
            {
                foreach (Gerboise mob in allyArray)
                {
                    if (!mob.isAvailable)
                    {
                        mobToRemove.Add(mob);
                    }
                }
            }
            else
            {
                foreach (Gerboise mob in allyArray)
                {
                    mobToRemove.Add(mob);

                }
            }

            foreach (Gerboise mob in mobToRemove)
            {
                allyArray.Remove(mob);
            }


            checkAllAlly();
            checkLeader();

            foreach (Gerboise mob in allyArray)
            {
                if (mob.target != null && target == null)
                {
                    target = mob.target;
                }
            }


            yield return new WaitForSeconds(1f);

        }

        if (state == STATE.DEATH)
        {
            foreach (Gerboise mob in allyArray)
            {
                mob.allyArray.Remove(this);
            }
        }

    }

    private void CheckFear()
    {
        //Check fear level
        //Run if too high
        if (target == null)
        {
            state = STATE.IDLE;
            return;
        }
        if (fearMeter >= fearMax && target != null)
        {
            isLeader = false;
            isAvailable = false;
            state = STATE.RUN;
            stopRage();
            return;
        }//Rage if too low
        else if (fearMeter <= 10 && target != null && RageCooldown <= 0)
        {
            Rage();
        }
        else
        {
            stopRage();
            if (!isAvailable)
            {
                isAvailable = true;
            }
        }
        FindAlly();

    }

    private void checkLeader()
    {

        if (Leader != null && !Leader.isLeader)
        {
            Leader = null;
        }

        foreach (Gerboise mob in allyArray)
        {
            if (mob.isLeader)
            {
                Leader = mob;
                isLeader = false;
                break;
            }
        }


        if (Leader == null && fearMeter < fearMax)
        {
            isLeader = true;
        }
    }

    private void checkAllAlly()
    {
        //List of mob to add in ally
        List<Gerboise> allyToAdd = new List<Gerboise>();

        //Check if mob aren't allready in ally
        foreach (Gerboise mob in allyArray)
        {
            foreach (Gerboise mobTested in mob.allyArray)
            {
                if (!allyArray.Contains(mobTested) && !allyToAdd.Contains(mobTested) && mobTested != this && mobTested.isAvailable)
                {
                    allyToAdd.Add(mobTested);
                }
            }
        }
        //Add mob in ally
        foreach (Gerboise mob in allyToAdd)
        {
            allyArray.Add(mob);
        }
    }

    List<Vector3> CalculateCirclePositions(Vector3 center, float radius, int count)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            float angle = (180f / count) * i;
            float x = center.x + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = center.z + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            positions.Add(new Vector3(x, center.y, z));
        }

        return positions;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkAllyRange);
    }

    public override void Death()
    {
        if (!isDead)
        {
            foreach (Item item in reward)
            {

                if (item.itemName == "peau de gerboise")
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Inventory.instance.AddItem(item);
                    }
                }
                else
                {
                    int randomNumber = Random.Range(1, 3);
                    for (int i = 0; i < randomNumber; i++)
                    {
                        Inventory.instance.AddItem(item);
                    }
                }


            }

            Destroy(GetComponent<NavMeshAgent>());
            rb.velocity = Vector3.zero;
            if (target != null)
            {
                rb.velocity = (transform.position - target.transform.position) * 15 + Vector3.up;
                rb.useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
            }

            animator.SetTrigger("Death");
            animator.speed = 1;
            isDead = true;
            isEnrage = true;
            stopRage();
        }
    }

    private void PlayStepSound()
    {
        soundPlayer.PlayStep();
    }

    private void PlayTailSound()
    {
        soundPlayer.PlayWhooshSound();
    }

    public override void Hit(float _damage)
    {
        if (!isDead)
        {
            if (hitCooldown <= 0 && !isAttacking)
            {
                animator.SetTrigger("hit");
                hitCooldown = 3f;
                agent.isStopped = true;
                soundPlayer.PlayHurtSound();
            }
            if (fearMeter < 100)
            {
                fearMeter += (int)(_damage / 10);
            }
            hp -= _damage;

            if (target != null)
            {
                target.GetComponent<ThirdPersonPlayer>().AddRage(_damage / 10);
                target.GetComponent<ThirdPersonPlayer>().rageTimer = 0;
            }
        }
    }

    public void ActivateAgent()
    {
        agent.isStopped = false;
        StopAttack();
    }

    public void DestroyMob()
    {
        GetComponentInChildren<DissolvingController>().StartCoroutine("DissolveCo");
    }

    private void DamagePlayer()
    {
        soundPlayer.PlayHitSound();

        target.GetComponent<PlayerStatScript>().ElemResist(damage, element);
        target.GetComponent<ThirdPersonPlayer>().TakeDamage(damage);
        DamageDealed = true;


        foreach (Gerboise gerboise in allyArray)
        {
            gerboise.fearMeter -= 15;
        }
        fearMeter -= 15;

    }

    public void StopAggro()
    {
        if (!stopAggro)
        {
            StopCoroutine("FindAllAlly");
            state = STATE.IDLE;
            stopAggro = true;
        }
    }

}