using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeeder : MonoBehaviour
{
    public int playbackSpeed = 1;

    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps.time = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        ps.playbackSpeed = playbackSpeed;
        if(Input.GetKeyDown(KeyCode.I))
        {
            ps.Play();
        }
    }
}
