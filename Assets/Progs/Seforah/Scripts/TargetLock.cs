using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Threading;

public class TargetLock : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera AimCamera;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    [Space]
    [Header("UI")]
    [SerializeField] private Image aimIcon;
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string enemyTag;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [Space]
    [SerializeField] Transform lookAtTarget;
    [SerializeField] ThirdPersonPlayer player;
    public bool isTargeting;

    private float maxAngle;
    public Transform currentTarget;

    void Start()
    {
        maxAngle = 90f;
    }

    void Update()
    {
        if (!player.isAlive)
        {
            isTargeting = false;
        }
        if (isTargeting)
        {
            if (currentTarget != null)
            {
                lookAtTarget.LookAt(currentTarget);

                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                if (distanceToTarget > maxDistance)
                {
                    CancelLock();
                }
                else
                {
                    UpdateTarget(currentTarget);
                }
            }
            else
            {
                CancelLock();
            }
        }

        if (aimIcon)
        {
            aimIcon.gameObject.SetActive(isTargeting);
        }
    }

    private void CancelLock()
    {
        AimCamera.gameObject.SetActive(false);
        cinemachineFreeLook.gameObject.SetActive(true);
        isTargeting = false;
        currentTarget = null;
    }


    public void LockEnemy(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        AssignTarget();
    }

    private void AssignTarget()
    {
        if (isTargeting)
        {
            AimCamera.gameObject.SetActive(false);
            cinemachineFreeLook.gameObject.SetActive(true);
            isTargeting = false;
            currentTarget = null;
            return;
        }

        if (ClosestTarget())
        {
            currentTarget = ClosestTarget().transform;
            cinemachineFreeLook.gameObject.SetActive(false);
            AimCamera.gameObject.SetActive(true);
            
            isTargeting = true;

            AimCamera.LookAt = currentTarget;
        }
    }

    private void UpdateTarget(Transform target)
    {
        if (!currentTarget)
        {
            return;
        }

        Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);

        if (aimIcon)
        {
            aimIcon.transform.position = mainCamera.WorldToScreenPoint(target.position);
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget < minDistance)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            transform.position = target.position - directionToTarget * minDistance;
        }
    }


    private GameObject ClosestTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        return closest;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}