using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCraftUi : MonoBehaviour
{
    [SerializeField] private GameObject prefabItemUi;
    [SerializeField] private Transform inventoryGrid;

    private Dictionary<string, Item> itemLookup = new Dictionary<string, Item>();

    private void Start()
    {
        //PopulateInventory();
        
    }

    private void OnEnable()
    {
        RefreshInventoryUI();
        CraftingSystem.CraftSuccessEvent += RefreshInventoryUI;
    }

    /// <summary>
    /// Crée et Instancie les case de l'inventaire en fonction des Items du joueur
    /// </summary>
    private void PopulateInventory()
    {
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        foreach (Item item in Inventory.instance.items)
        {
            string itemName = item.itemName;
            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName]++;
            }
            else
            {
                itemCounts[itemName] = 1;
            }

            if (!itemLookup.ContainsKey(itemName))
            {
                itemLookup[itemName] = item;
            }
        }

        foreach (var pair in itemCounts)
        {
            if (itemLookup.TryGetValue(pair.Key, out Item item))
            {
                GameObject itemInstance = Instantiate(prefabItemUi, inventoryGrid);
                itemInstance.transform.Find("ItemImage").GetComponent<Image>().sprite = item.itemIcon;
                itemInstance.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = pair.Value.ToString();
            }
        }
    }

    private void RefreshInventoryUI()
    {
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }

        PopulateInventory();
    }

    private void OnDestroy()
    {
        CraftingSystem.CraftSuccessEvent -= RefreshInventoryUI;
    }
}

