using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SecurityOob : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Transform initSpawn;
    private Vector3 spawn;
    // Start is called before the first frame update
    void Start()
    {
        initSpawn = player.transform;
        spawn = initSpawn.position;
    }

    // Update is called once per frame
    void Update()
    {        
        LobbyOobSecurity();
    }

    void LobbyOobSecurity()
    {
        if (player.transform.position.y <= -30)
        {
            player.transform.position = spawn;
        }
    }
}
