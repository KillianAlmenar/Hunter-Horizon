using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempEnemies : smallMonster
{
    private void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < detectionRange)
        {
            Move(player.transform);
        }
    }

    private void Update()
    {
        DetectPlayer();
    }
}
