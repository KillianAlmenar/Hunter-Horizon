using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;

    public event Action OnQuestUpdate;

    [SerializeField] GameObject[] questPrefabChasse;
    [SerializeField] GameObject[] questPrefabCueillette;

    [SerializeField] LayoutGroup parentLayout;
    [SerializeField] ShowQuest showQuest;

    [SerializeField]
    public List<Quest> quests = new List<Quest>();//recuperer dans ressources
    static private List<Quest> finishQuest = new();

    [SerializeField] Button closeButton;

    static private List<Button> QuestButtons = new();

    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
                instance = new QuestManager();

            return instance;
        }
        set => instance = value;
    }

    private void OnEnable()
    {
        //Debug.Log(QuestButtons.Count);

        if (QuestButtons[0] != null)
        {
            QuestButtons[0].Select();
        }

        /*
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        */

        GameManager.instance.UpdateCursor();
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()//Singleton Pattern for inventory system
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        QuestManagerInit();
    }

    public void SelectFirstQuest()
    {
        if (parentLayout.transform.childCount != 0)
        {
            parentLayout.transform.GetChild(0).GetComponent<Button>().Select();
            return;
        }
           
        closeButton.Select();
    }

    void QuestManagerInit()
    {
        QuestButtons.Clear();
        int maxQuest = 0;
        quests = Resources.Load<QuestDataBase>("QuestDatabase").quests;

        foreach (Quest quest in quests)
        {
            if (maxQuest < 6 && !AlreadyInPlayerInventory(quest))
            {
                GameObject temp;
                if (quest.type == Quest.Type.Hunt)
                {
                    temp = Instantiate(questPrefabChasse[Random.Range(0, 2)], parentLayout.transform);
                }
                else
                {
                    temp = Instantiate(questPrefabCueillette[Random.Range(0, 2)], parentLayout.transform);
                }

                temp.GetComponent<Button>().onClick.AddListener(() => showQuest.ActiveShowQuest(temp.transform.position, quest.id, temp));

                //stock les buttons
                QuestButtons.Add(temp.GetComponent<Button>());

                maxQuest++;
            }

        }

        SelectFirstQuest();
    }

    bool AlreadyInPlayerInventory(Quest quest)
    {
        foreach (Quest q in finishQuest)
        {
            if (q.id == quest.id)
            {
                return true;
            }
        }

        foreach (Quest i in Inventory.instance.activeQuest)
        {
            if (i.id == quest.id)
            {
                return true;
            }
        }

 
        return false;
    }

    public void UpdatePlayerQuest(string _id)
    {
        foreach (Quest quest in Inventory.instance.activeQuest)
        {
            quest.UpdateQuest(_id);
        }

        for (int i = Inventory.instance.activeQuest.Count - 1; i >= 0; i--)
        {
            Quest quest = Inventory.instance.activeQuest[i];
            if (quest.isCompleted)
            {
                Quest temp = quest.Clone();
                temp.isCompleted = false;
                finishQuest.Add(temp);

                Inventory.instance.activeQuest.RemoveAt(i);
            }
        }

        OnQuestUpdate?.Invoke();
    }

    public void CompleteQuest(string questName)
    {
        Quest quest = quests.Find(x => x.questName == questName);
      
        if (quest != null)
        {
           
           
            quest.CompleteQuest();
        }
    }

    private void Update()
    {
        if (parentLayout.transform.childCount <= 0)
        {
            closeButton.Select();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);  
    }

    public void UnlockPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayer>().SetPlayerInput(true);
    }
}
