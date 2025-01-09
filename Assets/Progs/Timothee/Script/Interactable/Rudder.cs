using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rudder : MonoBehaviour, IInteractable
{
    Collider rudderTrigger;
    [SerializeField] protected Animator animator;

    DialogueTrigger dialogueTrigger;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] BoatController boatController;

    [SerializeField] string sceneToLoad;
    [SerializeField] string musicToLoad;

    [Header("Menu")]
    [SerializeField] protected GameObject choiceCanvas;
    [SerializeField] protected GameObject choices;
    //[SerializeField] TMP_FontAsset rudderFont;

    private bool isOpen = false;

    private void Start()
    {
        rudderTrigger = GetComponent<Collider>();
        isOpen = false;

        boatController.OnEndDriving += DisableBoatDriving;
    }
    public void Interact()
    {
        if (!isOpen)
        {
            dialogueTrigger = GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerDialogue();
            
            dialogueManager.OnEndDialogue+=OpenChoiceMenu;
            isOpen = true;
        }

    }
    public void OpenChoiceMenu()
    {
        choiceCanvas.SetActive(true);
        choices.SetActive(true);
        dialogueManager.OnEndDialogue-= OpenChoiceMenu;

        animator.SetTrigger("Open");
        foreach (Button button in choices.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }

    public void PlayCloseChoiceAnim()
    {
        animator.SetTrigger("Close");
        foreach (Button button in choices.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
    }
    public void CloseChoiceMenu()
    {
        choices.SetActive(false);
        choiceCanvas.SetActive(false);
    }

    public void EnableBoatDriving()
    {
        rudderTrigger.enabled = false;
        boatController.EnableBoatDriving(true, transform);
        PlayCloseChoiceAnim();
    }
     
    public void DisableBoatDriving()
    {   
        GameManager.instance.EndInteraction();
        rudderTrigger.enabled = true;
        isOpen = false;
    }
    public void TravelToNextArea()
    {
        GameManager.instance.EndInteraction(); //To be able to pause again
        //Destroy(GameObject.Find("Volume"))
        LevelLoader.instance.LoadLevel(sceneToLoad);
        GameManager.instance.currentGameState = GameManager.GameState.INGAME;
        dialogueManager.OnEndDialogue-=TravelToNextArea;

        Inventory.instance.CanActiveInventory(true);
    }
}
