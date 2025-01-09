using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CollectPoint))]
public class CollectPointEditor : Editor
{
    SerializedProperty propertyItems;

    private void OnEnable()
    {
        propertyItems = serializedObject.FindProperty("Items");
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
        CollectPoint collect = (CollectPoint)target; // Obtenez l'instance de BasicMob
        List<Item> itemList = collect.Items; // Accédez à la liste d'items depuis BasicMob

        // Ajoutez l'objet à la liste d'items de BasicMob
        itemList.Add(item);
    }
}
