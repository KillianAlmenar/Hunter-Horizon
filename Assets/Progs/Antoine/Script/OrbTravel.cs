using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static Unity.VisualScripting.Member;

public class OrbTravel : MonoBehaviour
{
    public Vector3 target;
    private float speed = 5f;
    public bool loaded = false;
    public GameObject explosionEffect;
    [SerializeField] SoundPlayer player;

    private bool hasPlayed = false;

    private ParticleSystem ps;
    private void Start()
    {
        ps = explosionEffect.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (loaded)
        {
            if (!hasPlayed)
            {
                Vector3 dir = target - transform.position;
                dir.y = target.y - transform.position.y + 0.5f;
                transform.position += dir * speed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Head")
        {
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<VisualEffect>().enabled = false;
            hasPlayed = true;
            if (ps != null && !ps.isPlaying)
            {
                explosionEffect.SetActive(true);
                ps.Play();
                StartCoroutine(WaitForParticleSystemCompletion());
                player.Play();
            }

            Collider[] hits = Physics.OverlapSphere(transform.position, 4);

            foreach (Collider hit in hits)
            {
                if (hit.GetComponent<ThirdPersonPlayer>() != null)
                {
                    
                    hit.GetComponent<ThirdPersonPlayer>().TakeDamage(15);
                }
            }
        }
    }

    private IEnumerator WaitForParticleSystemCompletion()
    {
        yield return new WaitUntil(() => !ps.isPlaying);

            Destroy(gameObject);
    }
}
