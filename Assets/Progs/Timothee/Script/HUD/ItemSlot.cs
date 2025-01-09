using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("Item Reference")]
    [SerializeField] private Item item;

    [Header("Slot Image References")]
    [SerializeField] private Image iconItem;
    [SerializeField] private Image icon;

    #region Local Var
    private bool isSelected = false;
    #endregion

    #region GetterSetter
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public Item Item { get => item; set { item = value; } }
    public Image IconItem { get => iconItem; set { iconItem = value; } }
    public Image Icon { get => icon; set { icon = value; } }
    #endregion

    public void AddItemToSlot(Item newItem)
    {
        item = newItem;
        iconItem.sprite = newItem.itemIcon;
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
            //ClearSlot(); Call this function whenever you have a consumable to remove it
        }
        else
        {
            Debug.Log("There's no item to use!");
        }
    }

}
