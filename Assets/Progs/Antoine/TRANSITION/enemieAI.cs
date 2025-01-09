using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemieAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(player.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if(agent.hasPath == false)
        {
            Debug.Log("tes");
            agent.SetDestination(player.transform.position);
        }

    }
}
