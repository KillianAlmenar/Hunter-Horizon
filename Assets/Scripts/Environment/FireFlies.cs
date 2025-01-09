using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour
{
       private ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }

    private void Update()
    {
        if (DayNightManager.instance.isNight && _particleSystem.isStopped)
        {
            _particleSystem.Play();
        }
        else if (!DayNightManager.instance.isNight && !_particleSystem.isStopped)
        {
            _particleSystem.Stop();
        }
    }
}
