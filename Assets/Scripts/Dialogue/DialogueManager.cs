using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public event Action OnEndDialogue;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    private Queue<string> sentences;
    GameObject player;

    PlayerAction actions = null;

    private bool blockDialogue = false;

    [SerializeField] GameObject DialogueBox;

    private void Awake()
    {
        actions = new PlayerAction();
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        blockDialogue = false;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueBox.SetActive(true);

        blockDialogue = false;
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        actions.Lobby.Enable();
        actions.Lobby.Interaction.performed += OnInteractionPerformed;
        DisplayNextSentence();

        LockPlayer();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            OnEndDialogue?.Invoke();
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void OnInteractionPerformed(InputAction.CallbackContext ctx)
    {

        if (!blockDialogue)
        {
            DisplayNextSentence();
        }
        
    }


    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = " ";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(0.01f);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

        actions.Lobby.Interaction.performed -= OnInteractionPerformed;
        actions.Lobby.Disable();

        DialogueBox.SetActive(false);
        blockDialogue = true;
    }

    public void LockPlayer()
    {
        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(false);
    }

    public void UnLockPlayer()
    {
        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(true);
    }
}
