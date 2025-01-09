using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoadZone : MonoBehaviour
{
    [SerializeField] List<GameObject> CollectiblePoints;
    [SerializeField] GameObject prefabToSpawn;
    [SerializeField] List<int> IDObject;

    [SerializeField] List<GameObject> EnemiePoints;
    [SerializeField] GameObject EnemieToSpawn;
    void Start()
    {
        for (int i = 0; i < CollectiblePoints.Count; i++)
        {
            GameObject spawnedObject = Instantiate(prefabToSpawn, CollectiblePoints[i].transform.position, Quaternion.identity, CollectiblePoints[i].transform);

            CollectScript collectibleItem = spawnedObject.AddComponent<CollectScript>();

            collectibleItem.ID = IDObject[i];

        }

        for (int i = 0; i < EnemiePoints.Count; i++)
        { 
         //   Instantiate(EnemieToSpawn, EnemiePoints[i].transform.position, Quaternion.identity, EnemiePoints[i].transform);
        }
    }
}
