using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchDamageCollider : MonoBehaviour
{
    public bool playerCollision = false;
    public bool CoCollision = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ThirdPersonPlayer>() != null)
        {
            playerCollision = true;
        }

        if (other.GetComponentInParent<PalmTree>() != null)
        {
            CoCollision = true;

            if(GetComponentInParent<Gerboise>().isAttacking)
            {
                other.GetComponentInParent<PalmTree>().Hit();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ThirdPersonPlayer>() != null)
        {
            playerCollision = false;
        }

        if (other.GetComponentInParent<PalmTree>() != null)
        {
            CoCollision = false;

        }
    }

}
