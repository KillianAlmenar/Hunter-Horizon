using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Forge : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    DialogueTrigger dialogueTrigger;
    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] protected GameObject forgeCanvas;
    [SerializeField] protected GameObject choices;

    [SerializeField] protected Animator animator;

    private bool isOpen = false;

    public void Interact()
    {
        if (!isOpen)
        {
            //Interaction code here
            dialogueTrigger = GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerDialogue();

            isOpen = true;

            dialogueManager.OnEndDialogue += OpenForgeInterface;
        }
    }

    public void OpenForgeInterface()
    {
        forgeCanvas.SetActive(true);
        choices.SetActive(true);
        dialogueManager.OnEndDialogue-= OpenForgeInterface;

        animator.SetTrigger("Open");
        foreach (Button button in choices.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }

    public void PlayCloseForgeAnim()
    {
        animator.SetTrigger("Close");
        foreach(Button button in choices.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
    }

    public void CloseForgeInterface()
    {
        choices.SetActive(false);
        forgeCanvas.SetActive(false);
    }
    public void CanReInteract()
    {
        isOpen = false;
    }


}
