using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform oriantation;
    [SerializeField] Transform player;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform rb;

    [SerializeField] float rotationSpeed;
    [SerializeField] float movementThreshold = 0.2f;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        oriantation.forward = viewDir.normalized;

        Vector3 inputDir = player.GetComponent<Rigidbody>().velocity;
        inputDir.y = 0;

        if (inputDir.magnitude > movementThreshold)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

            Vector3 playerObjRotation = playerObj.rotation.eulerAngles;
            playerObjRotation.x = 0f;
            playerObj.rotation = Quaternion.Euler(playerObjRotation);
        }
    }

}
