using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoDamages : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.Hit(50);
            Debug.Log("pomme");
        }
    }
}
