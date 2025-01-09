using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Button primaryButton;
    private bool isController;

    // Start is called before the first frame update
    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        var devices = InputSystem.devices;

        foreach (var device in devices)
        {
            if (device is Gamepad)
            {
                primaryButton.Select();
                isController = true;
                GameManager.instance.IsUsingController = isController;
                UpdateCursorState();
                return;
            }
        }

        UpdateCursorState();

    }

    
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        Debug.Log("test");
        var devices = InputSystem.devices;

        foreach (var device in devices)
        {
            if (device is Gamepad)
            {
                primaryButton.Select();
                isController = true;
                GameManager.instance.IsUsingController = isController;
                UpdateCursorState();
                return;
            }
        }

        UpdateCursorState();
    }
    

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void InputSystem_onDeviceChange(InputDevice arg1, InputDeviceChange arg2)
    {
        throw new System.NotImplementedException();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Disconnected && device is Gamepad)
        {
            isController = false;
            GameManager.instance.IsUsingController = isController;
            UpdateCursorState();
        }
    }

    private void Update()
    {
        if (!isController)
        {
            var devices = InputSystem.devices;

            foreach (var device in devices)
            {
                if (device is Gamepad)
                {
                    primaryButton.Select();
                    isController = true;
                    GameManager.instance.IsUsingController = isController;
                    UpdateCursorState();
                    return;
                }
            }
        }
    }

    private void UpdateCursorState()
    {
        Cursor.visible = !isController;
        Cursor.lockState = isController ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
