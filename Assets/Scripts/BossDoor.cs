using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField]
    GameObject gerboise;
    public bool isEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            isEnter = true;
            GameManager.instance.IsInABossFight = true; //Used for tracking in other scenes(Pause for example) 
                                                        //I switch it to false when the bossfight has ended
        }

        if (other.GetComponentInChildren<Gerboise>() != null)
        {
            other.GetComponentInChildren<Gerboise>().Death();
        }
    }
}
