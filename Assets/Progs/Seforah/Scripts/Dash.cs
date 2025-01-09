using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerOrientationBody;
    private Rigidbody rb;
    private ThirdPersonPlayer pm;

    [Header("Dashing")]
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;

    [Header("Settings")]
    [SerializeField] bool allowAllDirections = true;
    [SerializeField] bool disableGravity = false;
    [SerializeField] bool resetVel = true;

    [Header("Cooldown")]
    [SerializeField] float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    [SerializeField] PlayerAction m_playerAction;
    private InputAction m_dashAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<ThirdPersonPlayer>();
    }

    private void Update()
    {
        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    public void Dashing(InputAction.CallbackContext context)
    {
        if (pm.isAlive)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }
            if (dashCdTimer > 0)
            {
                return;
            }
            else
            {
                dashCdTimer = dashCd;
            }

            pm.dashing = true;

            Transform forwardT;

            forwardT = playerOrientationBody;

            Vector3 direction = GetDirection(forwardT);

            Vector3 forceToApply = direction * dashForce + orientation.up;

            if (disableGravity)
            {
                rb.useGravity = false;
            }

            delayedForceToApply = forceToApply;
            Invoke(nameof(DelayedDashForce), 0.0025f);
            Invoke(nameof(ResetDash), dashDuration);
        }
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        if (resetVel)
        {
            //rb.velocity = Vector3.zero;
        }

        rb.AddForce(delayedForceToApply, ForceMode.VelocityChange);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        if (disableGravity)
        {
            rb.useGravity = true;
        }
    }

    private Vector3 GetDirection(Transform _forwardT)
    {
        Vector3 direction = _forwardT.forward;

        return direction.normalized;
    }
}
