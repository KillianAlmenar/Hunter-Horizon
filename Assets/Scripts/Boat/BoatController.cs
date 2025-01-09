using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    [SerializeField]HUD inGameHud;
    PlayerAction actions;
    [SerializeField] private float maxRotationSpeed = 0;

    [SerializeField] private float actualRotationSpeed = 0;

    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float actualSpeed = 0;

    float aceleration = 0;
    float sideAceleration = 0;

    private Transform fixPlayerPosition;

    [SerializeField] GameObject player;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    public Action OnEndDriving;
    private Vector2 MoveHorizontal = Vector2.zero;

    [SerializeField] BoatMove boatMove;

    [SerializeField] GameObject canvaNavigation;
    // Start is called before the first frame update
    void Start()
    {
        actions = new PlayerAction();
        actions.Boat.Brake.performed += resetRotationSpeed;
        actions.Boat.Quit.performed += DisableBoatDriving;
       
    }

    private void Update()
    {
        MoveHorizontal = actions.Boat.Brake.ReadValue<Vector2>();

        

        FrontMove();
        SideMove();
    }

    public void SideMove()
    {
        if (fixPlayerPosition != null && actions.Boat.Brake.enabled)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 temp = player.transform.position;
            player.transform.position = fixPlayerPosition.position - 2 * fixPlayerPosition.forward;
        }

        if (MoveHorizontal.x > 0)
        {
            sideAceleration += Time.deltaTime * 2;
            actualRotationSpeed += sideAceleration * Time.deltaTime;
        }
        else if (MoveHorizontal.x < 0)
        {
            sideAceleration += Time.deltaTime * 2;
            actualRotationSpeed -= sideAceleration * Time.deltaTime;
        }
        else
        {
            if (actualRotationSpeed > 0)
            {
                actualRotationSpeed -= 4 * Time.deltaTime;
            }
            else if (actualRotationSpeed < 0)
            {
                actualRotationSpeed += 4 * Time.deltaTime;
            }
            sideAceleration -= Time.deltaTime * 10;
        }

        sideAceleration = Mathf.Clamp(sideAceleration, 0, 10);
        actualRotationSpeed = Mathf.Clamp(actualRotationSpeed, -maxRotationSpeed, maxRotationSpeed);

        boatMove.yRotation += actualRotationSpeed * Time.deltaTime;
        boatMove.AddIncline += actualRotationSpeed * Time.deltaTime;

        if (boatMove.AddIncline > 0)
        {
            boatMove.AddIncline -= Time.deltaTime * 4;
        }
        else if (boatMove.AddIncline < 0)
        {
            boatMove.AddIncline += Time.deltaTime * 4;
        }


        boatMove.AddIncline = Mathf.Clamp(boatMove.AddIncline, -10, 10);


    }

    private void FrontMove()
    {
        if (MoveHorizontal.y > 0)
        {
            aceleration += Time.deltaTime * 2;
            actualSpeed += aceleration * Time.deltaTime;
        }
        else if (MoveHorizontal.y < 0)
        {
            aceleration += Time.deltaTime * 2;
            actualSpeed -= aceleration * Time.deltaTime;
        }
        else
        {
            aceleration -= Time.deltaTime * 10;
        }

        aceleration = Mathf.Clamp(aceleration, 0, 20);
        actualSpeed = Mathf.Clamp(actualSpeed, 0, maxSpeed);

        if (aceleration > 0)
        {
            boatMove.multiplicator = 4;
        }
        else
        {
            boatMove.multiplicator = 1;
        }


        Vector3 temp = -transform.forward * Time.deltaTime * actualSpeed;
        temp.y = 0;
        transform.position += temp;

    }

    public void resetRotationSpeed(InputAction.CallbackContext ctx)
    {
        //actualRotationSpeed = 0;
    }

    public void EnableBoatDriving(bool _bool, Transform _fixPlayerPos)
    {
        virtualCamera.Priority = 100;
        fixPlayerPosition = _fixPlayerPos;
        actions.Boat.Enable();

        canvaNavigation.SetActive(true);
        inGameHud.Fade(false);
    }

    public void DisableBoatDriving(InputAction.CallbackContext ctx)
    {
        virtualCamera.Priority = -100;
        

        canvaNavigation.SetActive(false);

        aceleration = 0;
        actualSpeed = 0;

        player.transform.position += player.transform.up;
        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(true);

        OnEndDriving?.Invoke();

        actions.Boat.Disable();
        inGameHud.Fade(true);
    }

    public float GetSpeed()
    {
        return actualSpeed;
    }
}
