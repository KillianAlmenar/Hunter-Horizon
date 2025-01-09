using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float fadingSpeed = 1f;

    [Header("References")]
    [SerializeField] private GameObject questDisplayPanel;
    [SerializeField] private Image questType;
    [SerializeField] TMP_Text questNameText;
    [SerializeField] TMP_Text questDescriptionText;
    [SerializeField] TMP_Text questProgressText; //Need to update it

    #region Local Var
    private Inventory playerQuestInventory;
    private CanvasGroup canvaGroup;
    //Fade
    bool startDisplayFade = false;
    bool fadeType = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        canvaGroup = GetComponentInChildren<CanvasGroup>();
        playerQuestInventory = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        if (startDisplayFade)
        {
            CallFading(fadeType);
        }
    }

    public void UpdateUI()
    {
        if (playerQuestInventory.activeQuest.Count > 0) //Only if player have quest
        {
            Fade(true);

            questNameText.text = playerQuestInventory.activeQuest[0].questName;
            questDescriptionText.text = playerQuestInventory.activeQuest[0].questDescription;

            questProgressText.text = "Remaining : " + playerQuestInventory.activeQuest[0].quantity.ToString();
            switch (playerQuestInventory.activeQuest[0].type)
            {
                case Quest.Type.Hunt:
                    questType.color = new Color(255f, 0, 0,50f);
                    break;
                case Quest.Type.Collect:
                    questType.color = new Color(0, 255f, 0, 50f);
                    break;
                default:
                    questType.color = Color.grey;
                    break;
            }
        }
        else
        {
            Fade(false);
        }
    }

    #region Fading UI Functions
    private void CallFading(bool _in)
    {
        if (_in == true)
        {
            if (canvaGroup.alpha < 1)
            {
                canvaGroup.alpha += fadingSpeed * Time.deltaTime;
            }
            else
            {
                startDisplayFade = false;
            }
        }
        else
        {
            if (canvaGroup.alpha > 0)
            {
                canvaGroup.alpha -= fadingSpeed * Time.deltaTime;
            }
            else
            {
                startDisplayFade = false;
            }
        }
    }
    void Fade(bool _in)
    {
        startDisplayFade = true;
        fadeType = _in;
    }
    #endregion
}
