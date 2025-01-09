using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public enum Rarity //Move to another script later
    {
        COMMON,
        RARE,
        EPIC,
        LEGENDARY,
    }

    public string itemName = "name";
    public string id = "-1";
    public string description = "description";
    public Sprite itemIcon = null;
    public Image itemImage = null;
    public int itemValue = 0;
    public Rarity rarity = 0;
    
    public virtual void Use()//Required for later - T
    {
        //Debug.Log("Using" + itemName);
    }
}

[System.Serializable]
public class Recipe
{
    public Item resultItem;
    public List<Item> requiredItems = new List<Item>();
    public string id;
}
