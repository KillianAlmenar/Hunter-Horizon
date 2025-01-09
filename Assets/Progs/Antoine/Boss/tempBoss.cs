using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class tempBoss : Boss
{
    private float distanceWithPlayer;
    private bool detected;
    private bool isAlive = true;
    public float rotationSpeed = 5f;
    public float detect = 5f;

    public List<GameObject> HitBoxBossList;
    [SerializeField] BossDoor[] boosDoor;
    [SerializeField] GameObject behindTrigger;
    [SerializeField] GameObject OrbPrefab;
    [SerializeField] Transform OrbPos;
    [SerializeField] BossSoundPlayer soundPlayer;

    private Quaternion targetRotation;
    private bool isRotating = false;
    Vector3 targetRush;
    GameObject orb;
    protected override void Start()
    {
        base.Start();
        Cursor.visible = false;
        detected = false;
        detectionRange = detect;
        CurrentState = State.IDLE;
        timer = 0;
        CollectHitBoxes(transform);
        setStopingDistance(1f);
    }

    void CollectHitBoxes(Transform parent)
    {
        foreach (Transform child in parent)
        {
            HitBoxBoss hitBox = child.GetComponent<HitBoxBoss>();

            if (hitBox != null)
            {
                HitBoxBossList.Add(hitBox.transform.gameObject);
            }

            CollectHitBoxes(child);
        }
    }
    void DisableHitBox()
    {
        foreach (GameObject child in HitBoxBossList)
        {
            child.SetActive(false);
        }
    }
    public float getDistance(Transform entityPos)
    {
        float distance = Vector3.Distance(transform.position, entityPos.position);
        return distance;
    }
    Vector3 GetRandomPointOnNavMesh(float maxDist, float minDist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * maxDist;

        randomDirection += transform.position;

        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(randomDirection, out navMeshHit, maxDist, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }

        return transform.position;
    }
    private void EndDestination()
    {
        if (CurrentState != State.WALKING && CurrentState != State.RUNNING && CurrentState != State.RUSH)
        {
            //  Move(transform.position, 3.5f, 8f);
        }
    }
    private void Waiting()
    {
        if (CurrentState == State.IDLE)
        {
            timer += Time.deltaTime;
            if (timer >= idleTimer)
            {
                OrientBoss(GetRandomPointOnNavMesh(25f, 5f), true);
            }
        }
    }
    private void Walking()
    {
        if (CurrentState == State.WALKING)
        {
            if (FinishedPath())
            {
                SetTrigger("Idle", State.IDLE);
            }
        }
    }
    private void BattleIdle()
    {
        if (CurrentState == State.BATTLEIDLE)
        {           
            DisableHitBox();
            OrientBoss(player.transform.position, false);
            timer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.H))
            {
                timer = 0;
                SetTrigger("BigTail", State.BIGTAIL);
            }
            if (timer >= idleTimer)
            {
                 if (getDistance(player.transform) > 15f)
                 {
                    int rand = Random.Range(0, 5);
                    if(rand == 0)
                    {
                        timer = 0;
                        animator.SetBool("Rush", true);
                        CurrentState = State.RUSH;
                    }
                    else if (rand != 1)
                    {
                        animator.SetBool("Orbs", true);
                        CurrentState = State.ORB;
                        idleTimer = 2f;
                        timer = 0;
                        orb = Instantiate(OrbPrefab, OrbPos.position, Quaternion.identity);
                        soundPlayer.PlayBreathingSound();
                    }
                    else if (rand != 2)
                    {
                        timer = 0;
                        SetTrigger("BigTail", State.BIGTAIL);
                    }
                    else if(rand != 0 && rand != 1 && rand != 2)
                    {
                        timer = 0;
                        animator.SetBool("Running", true);
                        CurrentState = State.RUNNING;
                    }
                 }
                 else
                 {
                     SetTrigger("Running", State.RUNNING);
                     OrientBoss(player.transform.position, true);
                 }
                if (getDistance(player.transform) < 8.5f)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        SetTrigger("Head", State.HEAD);
                    }
                    else if (rand == 1)
                    {
                        SetTrigger("BigTail", State.BIGTAIL);
                    }

                }
            }

        }
    }
    private void Running()
    {
        if (CurrentState == State.RUNNING)
        {
            OrientBoss(player.transform.position, true);
            Move(player.transform.position, 25f, 50f);
            timer += Time.deltaTime;
            if (getDistance(player.transform) < 8.5f)
            {
                int rand = Random.Range(0, 1);
                if (rand == 0)
                {
                    SetTrigger("Head", State.HEAD);
                }
                else if (rand == 1)
                {
                    SetTrigger("BigTail", State.BIGTAIL);
                }
            }
            else if (timer >= idleTimer)
            {
                SetTrigger("BattleIdle", State.BATTLEIDLE);
            }
        }
    }
    private void Head()
    {
            
        if (CurrentState == State.HEAD)
        {
            HitBoxBossList[0].SetActive(true);
            if (AnimationEnded())
            {
                SetTrigger("BattleIdle", State.BATTLEIDLE);
            }
        }
    }
    private void Tail()
    {
        if (CurrentState == State.TAIL)
        {
            HitBoxBossList[1].SetActive(true);
            if (AnimationEnded())
            {
                StartCoroutine(DelayedCheckAnimation());
            }
        }
    }
    private void BigTail()
    {
        if (CurrentState == State.BIGTAIL)
        {
            HitBoxBossList[1].SetActive(true);
            StartCoroutine(DelayedCheckAnimation());
        }
    }
    private void Orb()
    {
        if (CurrentState == State.ORB)
        {
            timer += Time.deltaTime;
            if (timer >= idleTimer)
            {
                animator.SetBool("Orbs", false);
                orb.GetComponent<OrbTravel>().loaded = true;
                if (AnimationEnded())
                {
                    soundPlayer.PlayOrbSound();
                    SetTrigger("BattleIdle", State.BATTLEIDLE); 
                }
            }
            else
            {
                OrientBoss(player.transform.position, false);
                Vector3 target = player.transform.position;
                orb.GetComponent<OrbTravel>().target = target;
                orb.transform.position = OrbPos.position;
            }
        }
    }
    private void OrientBoss(Vector3 _target, bool moving)
    {
        Vector3 target = _target;
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            direction.Normalize();

            targetRotation = Quaternion.LookRotation(direction);

            if (!isRotating || Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                isRotating = true;
            }
        }

        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                isRotating = false;
            }
            if (moving)
            {
                if (CurrentState == State.IDLE)
                {
                    Move(target, 3.5f, 8f);
                    SetTrigger("Walking", State.WALKING);
                }
                else if (CurrentState == State.BATTLEIDLE)
                {
                    if (getDistance(player.transform) > 8.5f)
                    {
                        Move(target, 25f, 50f);
                        SetTrigger("Running", State.RUNNING);
                    }
                }
            }
        }

    }
    public void enableTail()
    {
        if (CurrentState != State.TAIL)
        {
            SetTrigger("Tail", State.TAIL);
        }
    }
    public void Rush()
    {
        if (CurrentState == State.RUSH)
        {
            HitBoxBossList[2].SetActive(true);
            if (getDistance(player.transform) < 20f)
            {
                if (getDistance(player.transform) < 10f)
                {
                    animator.SetBool("Rush", false);
                    StartCoroutine(DelayedCheckAnimation());
                }
            }
            else
            {
                OrientBoss(player.transform.position, true);
                Move(player.transform.position, 50f, 75f);
            }
        }
    }
    IEnumerator DelayedCheckAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        if (AnimationEnded() && !animator.GetBool("Rush"))
        {
            SetTrigger("BattleIdle", State.BATTLEIDLE);
        }

        yield return null;
    }
    private void StateManager()
    {
        if (getDistance(player.transform) < 9)
        {
            agent.ResetPath();
        }
        Waiting();
        Walking();
        BattleIdle();
        Running();
        Head();
        Tail();
        Rush();
        BigTail();
        Orb();
    }
    protected override void Update()
    {
        if (boosDoor[0].isEnter || boosDoor[1].isEnter)
        {
            base.Update();
            distanceWithPlayer = getDistance(player.transform);
            StateManager();
            EndDestination();
            if (hitCooldown > 0)
            {
                hitCooldown -= Time.deltaTime;
            }
            if (getDistance(player.transform) < 35f && CurrentState != State.BATTLEIDLE && (CurrentState == State.IDLE || CurrentState == State.WALKING))
            {
                agent.ResetPath();
                SetTrigger("BattleIdle", State.BATTLEIDLE);
                idleTimer = Random.Range(1f, 2f);
            }
            if (CurrentState != State.IDLE && CurrentState != State.WALKING)
            {
                MusicManager.instance.BeginBossFight();
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            transform.position = new Vector3(-154.14f, -1.907349f, 172.74f);
        }

        soundPlayer.speed = currentSpeed / maxSpeed;
    }
    public override void Hit(float _damage)
    {
        if (hp > 0)
        {
            if (hitCooldown <= 0)
            {
                animator.SetTrigger("hit");
                hitCooldown = 2f;
                agent.isStopped = true;
            }
            hp -= _damage;


        }

        if (hp <= 0)
        {
            animator.SetTrigger("Death");
            Death();
        }
    }
    public override void Death()
    {
        if (isAlive)
        {
            QuestManager.Instance.UpdatePlayerQuest(id);
            GetComponentInChildren<DissolvingController>().StartCoroutine("DissolveCo");
            Destroy(gameObject, 5);
            isAlive = false;
            GameManager.instance.IsInABossFight = false;
        }

    }
    public void ActivateAgent()
    {
        agent.isStopped = false;
    }
    public void setStopingDistance(float distance)
    {
        agent.stoppingDistance = distance;
    }

    public void PlayStepSound()
    {
        soundPlayer.PlayStep();
    }

    public void PlayHeadAttackSound()
    {
        soundPlayer.PlayWhooshSound();
    }
}
