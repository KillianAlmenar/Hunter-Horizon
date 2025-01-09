using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerboiseBoat : MonoBehaviour
{

    private Animator anim;
    private bool sniff = false;
    [SerializeField]
    private float changeTimer = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        changeTimer = Random.Range(5, 10);
        int random = Random.Range(0, 2);

        if (random > 0)
        {
            sniff = true;
            anim.SetBool("Sniff", sniff);
        }
    }

    void Update()
    {
        changeTimer -= Time.deltaTime;

        if(changeTimer < 0f)
        {
            sniff = !sniff;
            anim.SetBool("Sniff", sniff);
            changeTimer = Random.Range(5, 10);
        }

    }
}
