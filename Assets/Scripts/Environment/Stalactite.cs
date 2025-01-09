using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Stalactite : MonoBehaviour
{
    [SerializeField]
    private GameObject smoke;
    [SerializeField]
    private GameObject smokePoint;
    float destroyTimer = 0;
    bool asCollided = false;
    List<GameObject> objectedHit = new List<GameObject>();
    [SerializeField]
    private float damage = 100f;
    Rigidbody rb;

    bool debugCollision = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (asCollided)
        {
            destroyTimer += Time.deltaTime;

            if (destroyTimer > 1)
            {
                Destroy(smoke);
            }

            if (destroyTimer > 3)
            {
                Destroy(gameObject);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;

        if(debugCollision && collision.gameObject.name == "ground")
        {
            smoke.SetActive(true);
            smoke.GetComponent<VisualEffect>().Reinit();
            smoke.GetComponent<VisualEffect>().Play();

            asCollided = true;
        }

        if (debugCollision && !objectedHit.Contains(collision.gameObject))
        {
            if(collision.gameObject.GetComponent<Gerboise>() != null)
            {
                collision.gameObject.GetComponent<Gerboise>().Hit(damage);
            }

            if (collision.gameObject.GetComponent<ThirdPersonPlayer>() != null)
            {
                collision.gameObject.GetComponent<ThirdPersonPlayer>().TakeDamage(damage);
            }

            objectedHit.Add(collision.gameObject);
        }

        debugCollision = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        rb.useGravity = true;

        if (debugCollision && other.gameObject.name == "ground")
        {
            smoke.SetActive(true);
            smoke.GetComponent<VisualEffect>().Reinit();
            smoke.GetComponent<VisualEffect>().Play();

            asCollided = true;
        }

        if (debugCollision && !objectedHit.Contains(other.gameObject))
        {
            if (other.gameObject.GetComponent<Gerboise>() != null)
            {
                other.gameObject.GetComponent<Gerboise>().Hit(damage);
            }

            if (other.gameObject.GetComponent<ThirdPersonPlayer>() != null)
            {
                other.gameObject.GetComponent<ThirdPersonPlayer>().TakeDamage(damage);
            }

            objectedHit.Add(other.gameObject);
        }

        debugCollision = true;

    }
}
