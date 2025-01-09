using log4net.Appender;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gerboise))]
public class BasicMobRewardEditor : Editor
{
    SerializedProperty propertyItems;

    private void OnEnable()
    {
        propertyItems = serializedObject.FindProperty("reward");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Reward Item"))
        {
            ItemPickerWindow.OpenWindow().SetCallback(AddItem);
        }
    }

    public void AddItem(Item item)
    {
        Gerboise basicMob = (Gerboise)target; // Obtenez l'instance de BasicMob
        List<Item> itemList = basicMob.reward; // Accédez à la liste d'items depuis BasicMob

        // Ajoutez l'objet à la liste d'items de BasicMob
        itemList.Add(item);
    }
}
