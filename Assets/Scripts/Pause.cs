using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("Canva")]
    [SerializeField] GameObject canva;
    [SerializeField] Button resumeButton;
    [SerializeField] Button boatButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button settingsButton;
    [SerializeField] GameObject settingsPrefab;

    [Header("BlurBackground Function")]
    [SerializeField] GameObject volumePrefab;
    private GameObject _volume;
    [SerializeField] float blurDuration = 0.5f;
    private float weightMin = 0.7f;
    private float weightMax = 0.9f;

    [Header("Player relative")]
    PlayerAction actions = null;//pour le nouveau system d'input
    GameObject player;

    [Header("Animation relative")]
    [SerializeField] protected Animator animator;

    private GameManager.GameState previousState;
    private void Awake()
    {
        actions = new PlayerAction();
    }

    private void OnEnable()
    {
        actions.Player.Pause.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Pause.Disable();
        actions.Player.Pause.performed -= OnInteractionPerformed;
    }

    // Start is called before the first frame update
    void Start()
    {
        _volume = Instantiate(volumePrefab); //Instantiate the blur volume process
        _volume.GetComponent<Volume>().weight = 0f; //No focus on blur when instantiated

        actions.Player.Pause.performed += OnInteractionPerformed;//Inputs
        player = GameObject.FindGameObjectWithTag("Player");

        resumeButton.Select();

        resumeButton.onClick.AddListener(() => PauseTest());

        boatButton.onClick.AddListener(() => ResetBlur());
        boatButton.onClick.AddListener(() => LevelLoader.instance.LoadLevel("Lobby"));
        boatButton.onClick.AddListener(() => GameManager.instance.currentGameState = GameManager.GameState.LOBBY);
        boatButton.onClick.AddListener(() => Destroy(_volume));

        menuButton.onClick.AddListener(() => ResetBlur());
        menuButton.onClick.AddListener(() => LevelLoader.instance.LoadLevel("Lobby"));
        menuButton.onClick.AddListener(() => GameManager.instance.SwitchToMenu());
        menuButton.onClick.AddListener(() => Destroy(_volume));

        settingsButton.onClick.AddListener(() => settingsPrefab.SetActive(true));

    }

    public void OnInteractionPerformed(InputAction.CallbackContext ctx) //When Pressing Pause
    {
        PauseTest();
    }

    public void PauseTest()
    {
        //We can't pause in Menu state or when the player is in an interaction
        //or when the inventory is already open or when we're in a bossfight
        if (GameManager.instance.currentGameState != GameManager.GameState.MENU &&
            !GameManager.instance.IsInAnInteraction && !Inventory.instance.GetInventoryIsOpen() && !GameManager.instance.IsInABossFight)
        {
            if (GameManager.instance.currentGameState == GameManager.GameState.PAUSE) //If already in pause
            {              
                actions.UI.Quit.Disable();
                actions.UI.Quit.performed -= OnInteractionPerformed;
                Resume();
            }
            else //If not in pause
            {
                previousState = GameManager.instance.currentGameState;
                    
                player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(false);
                if (GameManager.instance.currentGameState == GameManager.GameState.LOBBY)
                {
                    boatButton.gameObject.SetActive(false);
                }
                else
                {
                    boatButton.gameObject.SetActive(true);
                }
                canva.SetActive(true);

                GameManager.instance.InGameHUD.GetComponent<HUD>().Fade(false);
                BlurStart(true);//GameManager.instance.currentGameState = GameManager.GameState.PAUSE;
                animator.SetTrigger("Open");

                actions.UI.Quit.Enable();
                actions.UI.Quit.performed += OnInteractionPerformed;
            }
        }
    }

    void Resume()
    {
        BlurStart(false); //GameManager.instance.currentGameState = previousState;
        GameManager.instance.InGameHUD.GetComponent<HUD>().Fade(true);

        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(true);
        canva.SetActive(false);
        animator.SetTrigger("Close");

        GameManager.instance.HideCursor();
    }

    #region Blur
    public void BlurStart(bool _method)
    {
        StopAllCoroutines();
        if (_method)
        {
            StartCoroutine(Blur());
        }
        else
        {
            StartCoroutine(UnBlur());
        }
    }

    IEnumerator Blur()
    {
        float _elapsedTime = 0f;

        while (_elapsedTime < blurDuration)
        {
            _elapsedTime += Time.deltaTime;
            _volume.GetComponent<Volume>().weight = Mathf.Lerp(weightMin, weightMax, _elapsedTime / blurDuration);

            yield return null;
            GameManager.instance.currentGameState = GameManager.GameState.PAUSE;
        }
    }
    IEnumerator UnBlur()
    {
        float _elapsedTime = 0f;

        while (_elapsedTime < blurDuration)
        {
            _elapsedTime += Time.deltaTime;
            _volume.GetComponent<Volume>().weight = Mathf.Lerp(weightMax, 0, _elapsedTime / blurDuration);

            yield return null;
            GameManager.instance.currentGameState = previousState;
        }
    }

    void ResetBlur()
    {
        _volume.GetComponent<Volume>().weight = weightMin;
    }
    #endregion

}
