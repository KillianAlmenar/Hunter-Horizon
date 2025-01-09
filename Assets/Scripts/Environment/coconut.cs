using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static Unity.VisualScripting.Member;

public class coconut : MonoBehaviour
{
    [SerializeField]
    private float timerExplosion = 2;
    [SerializeField]
    private float ExplosionRadius = 4;
    [SerializeField]
    private GameObject explosionEffect;
    private bool explosionPlayed = false;
    private Rigidbody rb;
    private float lifeTime = 3f;
    [SerializeField]
    private ParticleSystemRenderer[] particlesRenderer;
    [SerializeField]
    private List<Material> dissolveMaterials;
    public float dissolveRate = 0.05f;
    public float refreshRate = 0.1f;
    [HideInInspector]
    public bool endCoroutine = false;
    [HideInInspector]
    public bool coroutineStarted = false;

    private Camera mainCamera;
    private bool isInCameraField = false;
    [SerializeField]
    private GameObject postProcessingGO;
    private Volume postProcessing;
    private ChromaticAberration aberration;
    private Vignette vignette;
    [SerializeField]
    private float flashBangSpeed = 0.1f;
    AudioSource source;
    private float damage = 50;
    private float exposure = 0.9f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Random.Range(-3, 4), 0, Random.Range(-3, 4));

        particlesRenderer = GetComponentsInChildren<ParticleSystemRenderer>();

        foreach (ParticleSystemRenderer particle in particlesRenderer)
        {
            dissolveMaterials.Add(particle.material);
        }

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        postProcessingGO = GameObject.Find("PostProcessing");
        postProcessing = postProcessingGO.GetComponent<Volume>();
        postProcessing.profile.TryGet(out aberration);
        postProcessing.profile.TryGet(out vignette);
        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        timerExplosion -= Time.deltaTime;
        if (timerExplosion < 0)
        {
            Explode();
        }


        if (lifeTime < 0)
        {
            if (!coroutineStarted)
            {
                StartCoroutine("DissolveCoroutine");
                coroutineStarted = true;
            }
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }

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

    }

    private IEnumerator FlashBang()
    {
        while (vignette.intensity.value < exposure)
        {
            vignette.intensity.value += Time.deltaTime * flashBangSpeed;

            yield return null;
        }
        while (vignette.intensity.value > 0.374f)
        {

            vignette.intensity.value -= Time.deltaTime * flashBangSpeed / 400;
            aberration.intensity.value -= Time.deltaTime * flashBangSpeed / 400;

            yield return null;
        }
        vignette.intensity.value = 0.374f;
        aberration.intensity.value = 0;
        Destroy(gameObject);
        yield return null;

    }

    private void Explode()
    {
        if (!explosionPlayed)
        {
            explosionEffect.GetComponent<ParticleSystem>().Play();
            GetComponent<MeshRenderer>().enabled = false;
            explosionPlayed = true;

            Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);

            foreach (Collider hit in hits)
            {
                if (hit.GetComponent<Gerboise>() != null)
                {
                    hit.GetComponent<Gerboise>().Hit(damage);
                }

                if (hit.GetComponent<ThirdPersonPlayer>() != null)
                {
                    if (isInCameraField)
                    {
                        Vector3 distance = hit.GetComponent<ThirdPersonPlayer>().transform.position - transform.position;

                        aberration.intensity.value = 1;
                        source.Play();
                        StartCoroutine("FlashBang");
                    }
                    hit.GetComponent<ThirdPersonPlayer>().TakeDamage(damage);
                }
            }

        }
        explosionEffect.transform.localRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, -transform.rotation.w);
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }

    IEnumerator DissolveCoroutine()
    {
        float counter = 0;

        if (dissolveMaterials.Count > 0)
        {

            while (dissolveMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < dissolveMaterials.Count; i++)
                {
                    dissolveMaterials[i].SetFloat("_DissolveWidth", 0.1f);
                    dissolveMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
                if (counter >= 1)
                {
                    endCoroutine = true;
                }

            }

        }
        else
        {
            Debug.Log("No Dissolving Material assigned to model.");
            yield break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

}