using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;

public class ItemsDataWindow : EditorWindow
{
    ItemDataBase database;

    [MenuItem("Tools/Item Database")]
    public static void OpenWindow()
    {
        GetWindow<ItemsDataWindow>("Item Database");
    }

    private void OnEnable()
    {
        database = Resources.Load<ItemDataBase>("ItemsDatabase");
    }

    Vector2 scrollPosition = Vector2.zero;

    string newItemId = "default_id";
    string search = "";

    List<Item> searchItem = new List<Item>();

    private void OnGUI()
    {
        Color baseColor = GUI.color;

        // Champs de recherche
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search :", GUILayout.Width(60));
        search = GUILayout.TextField(search);
        EditorGUILayout.EndHorizontal();

        SearchInDatabase();

        // Ajout d'un element dans la liste
        EditorGUILayout.BeginHorizontal();
        newItemId = GUILayout.TextField(newItemId);
        GUI.color = Color.green;
        if (GUILayout.Button("New Item"))
        {
            database.items.Add(new Item() { id = newItemId });
        }

        GUI.color = baseColor;

        EditorGUILayout.EndHorizontal();


        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


        for (int i = 0; i < searchItem.Count; i++)
        {
            Item item = searchItem[i];

            EditorGUILayout.BeginHorizontal();

            
            EditorGUILayout.LabelField(item.id, GUILayout.Width(100));

            item.itemName = EditorGUILayout.TextField(item.itemName, GUILayout.Width(100));
            item.rarity = (Item.Rarity)EditorGUILayout.EnumPopup(item.rarity, GUILayout.Width(100));
            item.description = EditorGUILayout.TextField(item.description);


            // bouton suppression
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                database.items.Remove(item);
            }
            GUI.color = baseColor;

            
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save"))
        {
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }
    }

    void SearchInDatabase()
    {

       searchItem = database.items.
            Where(quest => quest.itemName.ToLower().Contains(search.ToLower()) || quest.id.ToLower().Contains(search.ToLower()))
            .ToList();

    }
}
