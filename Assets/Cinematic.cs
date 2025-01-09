using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera CameraCinematic;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
    private float currentTimer;
    private float minTimer;
    private ThirdPersonPlayer player;


    private void Start()
    {
        CameraCinematic.gameObject.SetActive(false);
        currentTimer = 4.0f;
        minTimer = 0.0f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayer>();
    }

    private void Update()
    {
        if (cinemachineFreeLook.isActiveAndEnabled == false)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer <= minTimer)
            {
                CameraCinematic.gameObject.SetActive(false);
                cinemachineFreeLook.gameObject.SetActive(true);
                player.SetPlayerInput(true);
                gameObject.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            CameraCinematic.gameObject.SetActive(true);
            cinemachineFreeLook.gameObject.SetActive(false);
            player.SetPlayerInput(false);
        }
    }
}
