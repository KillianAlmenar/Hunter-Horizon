using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayer : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float dashSpeed;

    [Header("Input")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Slop Handleing")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    private bool grounded;

    [Header("Animation")]
    public Animator animPlayer;

    [Header("Audio")]
    public PlayerSoundPlayer soundPlayer;

    [Header("Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float rage = 0f;
    [SerializeField] float maxRage = 100f;

    [Space]
    private bool sprintState;
    [SerializeField] Transform oriantation;
    [SerializeField] float groundDrag;
    private Rigidbody rb;
    private float sprintAction;

    Vector3 moveDirection;

    public float MoveHorizontal;
    public float MoveVertical;

    [SerializeField] MovementState state;
    [SerializeField] PlayerInput inputPlayer;
    [SerializeField] CinemachineInputProvider inputCamera;

    [Header("fight and combos")]
    private int weapon = 0;
    public bool attackComboPlayer = false;

    [Space]
    [Header("Bool")]
    private bool DecreaseRage = true;
    public bool isAlive = true;
    public bool dashing;
    private bool invincible = false;
    private bool attackTimerActive = false;
    public bool actionPlayer = true;

    public int nbAttack = 0;

    [Space]
    [Header("Timer")]
    private float attackTimerDuration = 0.5f;
    private float attackTimer = 0.0f;
    public float rageTimerLimit = 2.5f;
    public float rageTimer = 0;
    [SerializeField] float invincibleTimerDuration = 1.0f;
    private float invincibleTimer = 0.0f;

    private bool InvincibleDebug = false;

    private enum MovementState
    {
        Walking,
        sprinting,
        dash,
        Attack
    }

    private void Awake()
    {
        //GameManager.instance.player = this.gameObject;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sprintState = false;
    }

    void invincibleDebugTkt()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!InvincibleDebug)
            {
                InvincibleDebug = true;
            }
            else
            {
                InvincibleDebug = false;
            }
        }
    }

    void Update()
    {
        invincibleDebugTkt();

        if (Input.GetKeyDown(KeyCode.X))
        {
            health = 0;
        }
        if (health > 0)
        {
            isAlive = true;
            if (!isSheatheAnimation())
            {
                MovePlayer();
            }
            grounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, whatIsGround);

            if (state == MovementState.Walking || state == MovementState.sprinting || state == MovementState.dash)
            {
                rb.drag = groundDrag;
                rb.velocity += Physics.gravity * Time.deltaTime * 15.0f;
            }
            else if (grounded)
            {
                rb.drag = 0;
            }
            checkRageTimer();
            if (rage > 0 && DecreaseRage)
                rage -= Time.deltaTime * 1.5f;
            UpdateAttackTimer();
            Invincible();
        }
        else
        {
            if (isAlive)
            {
                animPlayer.SetTrigger("Dead");
            }
            isAlive = false;
            animPlayer.SetBool("Attack", false);
            animPlayer.SetBool("SworkWalk", false);
            animPlayer.SetBool("Dash", false);
            animPlayer.SetBool("Finish 1", false);
        }
    }

    private void FixedUpdate()
    {
        StateHandler(sprintAction);
        EndSheathAnim();
    }

    public void checkRageTimer()
    {
        rageTimer += Time.deltaTime;
        if (rageTimer > rageTimerLimit)
        {
            DecreaseRage = true;
        }
        else
        {
            DecreaseRage = false;
        }
    }
    public void Sprinting(InputAction.CallbackContext context)
    {
        sprintAction = context.ReadValue<float>();
    }
    private void StateHandler(float sprintAction)
    {
        if (dashing)
        {
            animPlayer.SetBool("Dash", true);
            state = MovementState.dash;
            moveSpeed = dashSpeed + sprintSpeed;
        }
        else if (sprintAction > 0.5f)
        {
            state = MovementState.sprinting;
            sprintState = true;
            moveSpeed = sprintSpeed;
        }
        else
        {
            state = MovementState.Walking;
            moveSpeed = walkSpeed;
        }

    }

    public void SwordUnsheathe(InputAction.CallbackContext context)
    {
        animPlayer.SetFloat("Velocity", 0);
        if (GameManager.instance.currentGameState != GameManager.GameState.LOBBY)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }
            if (!isSheatheAnimation())
            {
                state = MovementState.Attack;

                if (weapon != 0)
                {
                    weapon = 0;
                }
                else
                {
                    weapon = 1;
                }

                animPlayer.SetFloat("Weapon", weapon);


                if (weapon != 0)
                {
                    animPlayer.SetBool("Unsheath", true);
                    soundPlayer.PlayUnsheathe();
                }
                else
                {
                    animPlayer.SetBool("Sheath", true);
                    soundPlayer.PlaySheathe();
                }
            }
        }
    }

    public void NextWeapon(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        if (weapon == 1)
        {
            weapon = 2;
        }
        else if (weapon == 2)
        {
            weapon = 1;
            Debug.Log("weapon : " + weapon);
        }

        animPlayer.SetFloat("Weapon", weapon);
    }

    private bool isSheatheAnimation()
    {
        if (animPlayer.GetBool("Unsheath"))
        {
            return animPlayer.GetBool("Unsheath");
        }
        if (animPlayer.GetBool("Sheath"))
        {
            return animPlayer.GetBool("Sheath");
        }
        else
            return false;
    }
    private void EndSheathAnim()
    {
        if (animPlayer.GetBool("Unsheath") || animPlayer.GetBool("Sheath"))
        {
            if (AnimationEnded())
            {
                animPlayer.SetBool("Unsheath", false);
                animPlayer.SetBool("Sheath", false);
            }
        }
    }
    public void SwordAttack(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            if (weapon == 1 || weapon == 2)
            {
                if (context.phase != InputActionPhase.Performed)
                {
                    return;
                }

                //if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                //{
                //    if (context.control.IsPressed())
                //    {
                //        nbAttack = 2;
                //        animPlayer.SetBool("Combos 1", true);
                //        animPlayer.SetBool("Attack", false);
                //    }
                //}
                //else if (animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
                //{
                //}
                //else
                //{
                if (context.control.IsPressed())
                {
                    animPlayer.SetTrigger("Attack 0");
                    animPlayer.SetBool("Attack", true);

                    nbAttack++;
                    nbAttack = Mathf.Clamp(nbAttack, 0, 2);
                    animPlayer.SetInteger("Combo", nbAttack);
                    StartAttackTimer();

                    soundPlayer.PlaySword();
                }
            }
        }

    }

    private void StartAttackTimer()
    {
        attackTimerActive = true;
        //attackTimer = attackTimerDuration;
        attackTimer = 1f;
    }

    private void UpdateAttackTimer()
    {
        if (attackTimerActive)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                attackTimerActive = false;
                nbAttack = 0;
                Debug.Log("caca");
                animPlayer.SetBool("Finish", false);
                animPlayer.SetBool("Attack", false);
                animPlayer.SetInteger("Combo", nbAttack);
            }
        }
    }

    public void FinishAttack(InputAction.CallbackContext context)
    {
        if (weapon == 1 || weapon == 2)
        {

            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }
            if (context.control.IsPressed())
            {
                moveSpeed = 0f;
                animPlayer.SetBool("Finish", true);

                StartAttackTimer();
            }
        }
    }

    void MovePlayer()
    {
        if (weapon == 1 || weapon == 2)
        {
            if (moveSpeed != 0)
            {
                moveSpeed /= 1.5f;
            }
        }
        //{
        Vector3 directionCamera = Camera.main.transform.forward;
        directionCamera.y = 0f;
        directionCamera.Normalize();

        Shader.SetGlobalVector("Player", transform.position);

        Vector3 Move;

        if (state == MovementState.dash || attackComboPlayer)
        {
            soundPlayer.PlayDash();
            return;
        }

        if (OnSlope())
        {
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();
            rb.MovePosition(rb.position + slopeMoveDirection * moveSpeed * Time.deltaTime);
        }

        if (state != MovementState.dash && !isSheatheAnimation())
        {
            if (Mathf.Abs(MoveHorizontal) > 0 || Mathf.Abs(MoveVertical) > 0)
            {
                Move = MoveVertical * directionCamera + MoveHorizontal * Camera.main.transform.right;
                Move.y -= 0.0f;
                Move.Normalize();
                Move *= moveSpeed;
                Move.y += rb.velocity.y;
                rb.velocity = Move;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        soundPlayer.speed = rb.velocity.magnitude / sprintSpeed;
        animPlayer.SetFloat("Velocity", rb.velocity.magnitude);
        animPlayer.SetBool("Dash", false);
        // }
    }

    private void Invincible()
    {
        if (invincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= invincibleTimerDuration)
            {
                invincible = false;
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        MoveHorizontal = context.ReadValue<Vector2>().x;
        MoveVertical = context.ReadValue<Vector2>().y;
    }

    #region Getters
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }
    public float GetRage() { return rage; }
    public float GetMaxRage() { return maxRage; }

    public int GetNbAttack() { return nbAttack; }

    public bool GetInvincible() { return invincible; }
    private bool AnimationEnded()
    {
        return (animPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    #endregion
    #region Setters

    public void TakeDamage(float damageAmount)
    {
        if (InvincibleDebug)
        {
            if (!invincible)
            {
                //  damageAmount = ((damageAmount * damageAmount) / GetComponent<PlayerStatScript>().playerDef);
                //  Debug.Log(GetComponent<PlayerStatScript>().playerDef);
                invincible = true;
                health -= damageAmount;
            }
        }


        if (health < 0)
        {
            health = 0;
        }
    }

    public void AddRage(float Amount)
    {
        rage += Amount;
        if (rage >= maxRage)
            rage = maxRage;
    }
    public void SetPlayerInput(bool ActiveInput)
    {
        inputPlayer.enabled = ActiveInput;
        inputCamera.enabled = ActiveInput;
    }


    #endregion
}
