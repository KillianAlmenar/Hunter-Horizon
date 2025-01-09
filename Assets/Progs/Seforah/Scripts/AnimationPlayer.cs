using Cinemachine;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] BoxCollider colliderSword;
    [SerializeField] BladeScript blade;

    private CinemachineImpulseSource impulseSource;
    [SerializeField] ThirdPersonPlayer player;
    [SerializeField] CinemachineFreeLook cameraFreeLook;

    private bool isTransitioningFOV = false;
    private float targetFOV;
    private float transitionDuration = 0.3f;
    private float transitionStartTime;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (isTransitioningFOV)
        {
            float elapsedTime = Time.time - transitionStartTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            cameraFreeLook.m_Lens.FieldOfView = Mathf.Lerp(cameraFreeLook.m_Lens.FieldOfView, targetFOV, t);

            if (t >= 1.0f)
            {
                isTransitioningFOV = false;
            }
        }
    }

    public void TriggerActive()
    {
        colliderSword.enabled = true;
        if (player.nbAttack >= 2)
        {
            targetFOV = 25f;
            StartFOVTransition();
        }
    }

    public void TriggerDeactivate()
    {
        colliderSword.enabled = false;
        if (player.nbAttack >= 2)
        {
            targetFOV = 30f;
            StartFOVTransition();
        }
    }

    public void ActiveFinal()
    {
        if (blade == null)
        {
            return;
        }
        blade.ActiveTornado();
        CinemachineShake.instance.CameraShake(impulseSource);
    }

    private void StartFOVTransition()
    {
        isTransitioningFOV = true;
        transitionStartTime = Time.time;
    }
}
