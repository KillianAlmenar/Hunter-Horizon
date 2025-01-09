using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int hotBarSize = 2;
    [SerializeField] private int slotSelected = 0;

    [Header("Content")]
    [SerializeField] private List<ItemSlot> slotsBar = new List<ItemSlot>();

    [Header("Sprite Relative")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite selectedSprite;

    #region Local Var
    private PlayerAction inputActions;
    private Vector2 defaultSize = new Vector2(1, 1);
    private Vector2 selectionSize = new Vector2(1.25f, 1.25f);
    #endregion

    #region GetterSetter
    public int SlotSelected
    {
        get => slotSelected; 
        set
        {
            if (SlotSelected >=0 && SlotSelected <= hotBarSize -1)
            {
                slotSelected = value;
            }
        }
    }

    public int HotBarSize{ get => hotBarSize; set { hotBarSize = value; } }



    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        InitBar();
        inputActions = new PlayerAction();
    }

    // Update is called once per frame
    void Update()
    {
        
            
            UpdateSelection();
        

    }
    public void InitBar()
    {
        for (int i = 0; i < hotBarSize; i++)
        {
            ItemSlot slot = gameObject.transform.GetChild(i).GetComponent<ItemSlot>();
            slotsBar.Add(slot);

            UpdateSelection();
        }
    }

    #region SlotFunctions

    public void AddItemToSlot(Item item, int index)
    {
        slotsBar[index].AddItemToSlot(item);
    }
    public void RemoveItemFromSlot(int index)
    {
        slotsBar.RemoveAt(index);
    }
    public void ReplaceItemFromSlot(Item newItem, int index)
    {
        RemoveItemFromSlot(index);
        AddItemToSlot(newItem, index);
    }

    #endregion

    #region SelectionFunctions

    public void SelectNextSlot()
    {
        if (slotSelected < slotsBar.Count - 1)
        {
            slotSelected++;
        }
        else
        {
            slotSelected = 0;
        }
        UpdateSelection();
    }
    public void SelectPreviousSlot()
    {
        if (slotSelected > 0)
        {
            slotSelected--;
        }
        else
        {
            slotSelected = slotsBar.Count - 1;
        }
        UpdateSelection();
    }

    public void UpdateSelection()
    {
        for (int i = 0; i < slotsBar.Count; i++)
        {
            slotsBar[i].IsSelected = false;
        }
        slotsBar[slotSelected].IsSelected = true;

        for (int i = 0; i < slotsBar.Count; i++)
        {
            if (slotsBar[i].IsSelected)
            {
                slotsBar[i].gameObject.transform.localScale = selectionSize;
                slotsBar[i].Icon.sprite = selectedSprite;
            }
            else
            {
                slotsBar[i].gameObject.transform.localScale = defaultSize;
                slotsBar[i].Icon.sprite = defaultSprite;
            }
        }
    }
    #endregion

}
