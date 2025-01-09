using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    DialogueTrigger dialogueTrigger;
    [SerializeField] DialogueManager dialogueManager;

    [Header("Interface")]
    [SerializeField] protected GameObject questCanvas;

    private bool isOpen = false;

    public void Interact()
    {
        if (!isOpen)
        {
            //Interaction code here
            dialogueTrigger = GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerDialogue();

            isOpen = true;

            dialogueManager.OnEndDialogue+= OpenQuestInterface;
        }
    }
    public void OpenQuestInterface()
    {
        questCanvas.SetActive(true);

        dialogueManager.OnEndDialogue-= OpenQuestInterface;
    }

    public void CanReInteract()
    {
        isOpen = false;
    }
}
