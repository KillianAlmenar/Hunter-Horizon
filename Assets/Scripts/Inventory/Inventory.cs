using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public delegate void OnItemChange();
    public OnItemChange onItemChange = delegate { }; //Delegate Update setup

    public List<Item> 
        ItemList = new List<Item>();
    public List<Item> items = new List<Item>();
    public List<Quest> activeQuest = new List<Quest>();

    [SerializeField] GameObject UiPrefab;
    private GameObject refUi;

    public int golds = 0;

    public HotBar hotbar; //Player's quick Inventory

    private bool InventoryIsOpen;

    PlayerAction actions = null;

    private void Awake()//Singleton Pattern for inventory system
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            actions = new PlayerAction();

            actions.Player.Inventory.Enable();

            actions.Player.Inventory.performed += OnInteractionPerformed;

            InventoryIsOpen = false;

            DontDestroyOnLoad(this);
        }

    }

    private void Start()
    {
        if (hotbar != null)
        {
            //Setup Placeholder until we have a system or UI to put items in hotbar
            for (int i = 0; i < hotbar.HotBarSize; i++)
            {
                if (items.Count > i)
                {
                    hotbar.AddItemToSlot(items[i], i);
                }
            }
        }
        foreach (EquipmentData itemsAdded in Resources.Load<EquipmentDataBase>("EquipmentDataBase").equipments)
        {
            items.Add(itemsAdded);
        }

    }

    public void OnInteractionPerformed(InputAction.CallbackContext ctx)
    {

        if (GameManager.instance.currentGameState != GameManager.GameState.MENU && GameManager.instance.currentGameState != GameManager.GameState.PAUSE)
        {
            //for (int i = 0; i < hotbar.slotsBar.Count; i++)
            //{
            //   //hotBarItemList[i] = hotbar.slotsBar[i].item; // ça bug
            //}
            if (!InventoryIsOpen)
            {
                InventoryIsOpen = true;
                refUi = Instantiate(UiPrefab, transform);

                actions.UI.Quit.Enable();
                actions.UI.Quit.performed += OnInteractionPerformed;
                //GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayer>().SetPlayerInput(false);

                GameManager.instance.UpdateCursor();
            }
            else
            {
                refUi.GetComponent<Animator>().SetBool("Close", true);
                Destroy(refUi, 0.5f);
                refUi = null;
                actions.UI.Quit.Disable();
                actions.UI.Quit.performed -= OnInteractionPerformed;
                InventoryIsOpen = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }


        }
    }

    /// <summary>
    /// Ajoute un Item dans l'inventaire du joueur
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        items.Add(item);
        onItemChange.Invoke();
    }

    /// <summary>
    /// Verifie si l'inventaire du joueur contient les Items requis
    /// </summary>
    /// <param name="requiredItems"></param>
    /// <returns></returns>
    public bool HasItems(List<Item> requiredItems)
    {
        foreach (Item item in requiredItems)
        {
            if (!ContainsId(item))
                return false;
        }
        return true;
    }

    private bool ContainsId(Item item)
    {
        foreach (Item invItem in items)
        {
            if (item.id == invItem.id)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Enleve une liste d'Item de l'inventaire du joueur
    /// </summary>
    /// <param name="requiredItems"></param>
    public void RemoveItems(List<Item> requiredItems)
    {
        foreach (Item requiredItem in requiredItems)
        {
            int removeIndex = 0;
            foreach (Item invItem in items)
            {
                if (invItem.id == requiredItem.id)
                {
                    break;
                }
                removeIndex++;
            }

            items.RemoveAt(removeIndex);
        }
    }

    public void CanActiveInventory(bool _bool)
    {

        if (_bool)
        {
            actions.Player.Inventory.Enable();
        }
        else
        {
            actions.Player.Inventory.Disable();
        }
    }

    public bool GetInventoryIsOpen()
    {
        return InventoryIsOpen;
    }

    public Elements.Element GetQuestElements()
    {
        foreach (Quest q in activeQuest)
        {
            if (q.element != Elements.Element.NONE)
            {
                return q.element;
            }
        }

        return Elements.Element.NONE;
    }
}
