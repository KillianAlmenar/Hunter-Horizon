using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private bool isInInteractRange = false;
    private string interactTextContent = "Interact Text Default Content";

    [SerializeField] private ThirdPersonPlayer player;
    private HUD hud;
    IInteractable interactable;
    PlayerAction actions = null;

    public bool IsInInteractRange { get; set; }
    public string InteractTextContent { get; set; }


    private void Awake()
    {
        actions = new PlayerAction();
        hud = GameObject.Find("InGameHUD").GetComponent<HUD>();
    }

    private void OnEnable()
    {
        actions.Player.Interaction.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Interaction.Disable();
    }

    public void OnInteractionPerformed(InputAction.CallbackContext ctx)
    {
        if (IsInInteractRange && GameManager.instance.currentGameState != GameManager.GameState.PAUSE && player.isAlive)//We need to keep it as the stack calls asks for its verification
        {
            Inventory.instance.CanActiveInventory(false);
            GameManager.instance.IsInAnInteraction = true;
            interactable.Interact();
            hud.PlayTextOutAnim();

        }
    }

    private void Update()
    {
        IsInInteractRange = isInInteractRange;
        InteractTextContent = interactTextContent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            interactTextContent = "Press " + actions.Player.Interaction.GetBindingDisplayString()+ " to Interact";
            hud.PlayTextInAnim();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null) //We detect an interactable object
        {
            actions.Player.Interaction.performed += OnInteractionPerformed;//Inputs

            isInInteractRange = true;
            interactable = other.GetComponent<IInteractable>();

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            actions.Player.Interaction.performed -= OnInteractionPerformed;//Inputs

            isInInteractRange = false;
            interactable = null;

            Inventory.instance.CanActiveInventory(true);
            hud.PlayTextOutAnim();
        }
    }

}
