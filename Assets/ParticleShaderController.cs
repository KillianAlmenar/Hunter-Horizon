using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShaderController : MonoBehaviour
{
    ParticleSystemRenderer updater;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        updater = GetComponent<ParticleSystemRenderer>();
        timer = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= 2*Time.deltaTime;
        updater.material.SetFloat("_Clip", timer);
    }
}
