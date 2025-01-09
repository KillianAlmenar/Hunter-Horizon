using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase")]
public class ItemDataBase : ScriptableObject
{
    public List<Item> items = new List<Item>();

    static public Item GetItemById(string id)
    {
        ItemDataBase database = Resources.Load<ItemDataBase>("ItemsDatabase");
        List<Item> searchItem = new List<Item>();

        searchItem = database.items.
    Where(item => item.itemName.ToLower().Contains(id.ToLower()) || item.id.ToLower().Contains(id.ToLower()))
    .ToList();

        Item item = new Item();
        item = searchItem.First();
        return item ;
    }
}
