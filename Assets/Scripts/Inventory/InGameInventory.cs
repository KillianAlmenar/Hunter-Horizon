using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameInventory : MonoBehaviour
{
    [SerializeField] private GameObject prefabItemUi;
    [SerializeField] private Transform inventoryGrid;
    [SerializeField] TextMeshProUGUI golds;

    private Dictionary<string, Item> itemLookup = new Dictionary<string, Item>();

    private GameObject player;

    private void Start()
    {
        PopulateInventory();
        player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(false);

        golds.text = "Golds : " + Inventory.instance.golds;
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

    private void OnDestroy()
    {
        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(true);
    }
}
