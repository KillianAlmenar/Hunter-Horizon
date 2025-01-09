using log4net.Appender;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPC_Shop))]
public class NPC_ShopEditor : Editor
{
    SerializedProperty propertyItems;

    private void OnEnable()
    {
        propertyItems = serializedObject.FindProperty("stockItems");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Item"))
        {
            ItemPickerWindow.OpenWindow().SetCallback(AddItem);
        }
    }

    public void AddItem(Item item)
    {
        NPC_Shop shop = (NPC_Shop)target; // Obtenez l'instance de BasicMob
        shop.ShopArray.Add(item);
        // Ajoutez l'objet à la liste d'items de BasicMob
        //itemList.Add(item);
    }
}
