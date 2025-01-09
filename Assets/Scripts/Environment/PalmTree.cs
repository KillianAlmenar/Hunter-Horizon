using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class PalmTree : MonoBehaviour
{
    [SerializeField]
    GameObject coconut;
    [SerializeField]
    Transform[] cocoSpawnPoint;
    private float spawnCocoCooldown = 1f;


    private void Update()
    {

        spawnCocoCooldown -= Time.deltaTime;
    }

    public void Hit()
    {
        if (spawnCocoCooldown <= 0)
        {
            int randomPoint = Random.Range(0, 2);
            Instantiate(coconut, cocoSpawnPoint[randomPoint].position, Quaternion.identity);
            spawnCocoCooldown = 1f;
        }

    }

}