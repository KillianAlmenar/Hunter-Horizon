using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        LOBBY,
        INGAME,
        PAUSE
    }

    public static GameManager instance;

    [Header("General")]
    public GameState currentGameState = GameState.MENU;
    public GameObject player;

    [Header("UI Relative")]
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject EndQuestCanva;

    [Header ("Controls Sensibility")]
    public float mouseSensibility = 0f;
    public float controllerSensibility = 0f;

    #region Local Var
    private bool isUsingController = false;
    private bool isInABossFight = false;
    private bool isInAnInteraction = false;
    private DataManager dataManager;
    #endregion

    #region Getter Setter
    public bool IsUsingController { get => isUsingController; set => isUsingController = value; }
    public bool IsInAnInteraction { get => isInAnInteraction; set => isInAnInteraction = value; }
    public bool IsInABossFight { get => isInABossFight; set => isInABossFight = value; }
    public DataManager GetDataManager { get => GameManager.instance.dataManager; }

    public GameObject InGameHUD { get => GameManager.instance.inGameHUD; set => inGameHUD = value; }

    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        dataManager = GameManager.instance.GetComponent<DataManager>();
    }
    public void EndInteraction() //Since Setter doesn't work for some reason
    {
        GameManager.instance.IsInAnInteraction = false; //Keep the GameManager.instance because otherwise it won't work for some reason... Unity moment lol
        HideCursor();
    }

    #region StateTransition Functions
    public void SwitchToGame()
    {
        GameManager.instance.currentGameState = GameState.INGAME;

        InGameHUD.SetActive(true);
        InGameHUD.GetComponent<HUD>().Fade(true);
    }

    public void SwitchToMenu()
    {
        GameManager.instance.currentGameState = GameState.MENU;

        InGameHUD.GetComponent<HUD>().Fade(false);
        InGameHUD.SetActive(false);

    }

    public void SwitchToLobby()
    {
        GameManager.instance.currentGameState = GameState.LOBBY;
        InGameHUD.SetActive(true);
        InGameHUD.GetComponent<HUD>().Fade(true);
    }
    #endregion

    #region Cursor Functions
    public void UpdateCursor()
    {
        if (instance.IsUsingController)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region DataManager Function
    public bool IsThereAnyProgress()
    {
        if (instance.dataManager.IsThereProgress())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region QuestFunction
    public void InstantiateEndQuest(string _questName)
    {
        GameObject temp = Instantiate(EndQuestCanva);
        temp.GetComponent<EndQuest>().SetEndQuestCanva(_questName);
    }
    #endregion

}
