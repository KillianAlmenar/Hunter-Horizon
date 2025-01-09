using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCollider : MonoBehaviour
{

    [SerializeField] GameObject CurrentZone;
    [SerializeField] GameObject ZoneToLoad;
    [SerializeField] GameObject Player;
    [SerializeField] Transform SpawnPlayer;
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CurrentZone.SetActive(false);
        ZoneToLoad.SetActive(true);
        Player.transform.position = SpawnPlayer.position;
    }
}
