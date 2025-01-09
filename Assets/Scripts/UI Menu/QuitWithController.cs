using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuitWithController : MonoBehaviour
{
    [SerializeField] Button quitButton;
    PlayerAction action;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        action = new PlayerAction();
        action.UI.Quit.Enable();
        action.UI.Quit.performed += OnActionPerform;
    }

    private void OnDisable()
    {
        action.UI.Quit.Disable();
        action.UI.Quit.performed -= OnActionPerform;
    }

    void OnActionPerform(InputAction.CallbackContext ctx)
    {
        quitButton.Select();
        quitButton.onClick.Invoke();
    }
}
