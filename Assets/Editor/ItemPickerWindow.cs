using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.IonicZip;
using UnityEditor;
using UnityEngine;

public class ItemPickerWindow : EditorWindow
{
    ItemDataBase database;

    public delegate void ItemPickerDelegate(Item item);
    public ItemPickerDelegate callback;

    public static ItemPickerWindow OpenWindow()
    {
        return GetWindow<ItemPickerWindow>("Item Picker");
    }

    public void SetCallback(ItemPickerDelegate _callback)
    {
        callback = _callback;
    }

    private void OnEnable()
    {
        database = Resources.Load<ItemDataBase>("ItemsDatabase");
    }

    Vector2 scrollPosition = Vector2.zero;
    string search = "";
    List<Item> searchItems = new List<Item>();

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Search :", GUILayout.Width(60));
        search = GUILayout.TextField(search);
        EditorGUILayout.EndHorizontal();

        SearchInDatabase();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < searchItems.Count; i++)
        {
            Item item = searchItems[i];

            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button(item.itemName))
            {
                callback?.Invoke(item);
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

    }

    void SearchInDatabase()
    {
        searchItems = database.items.
            Where(item => item.itemName.ToLower().Contains(search.ToLower()) || item.id.ToLower().Contains(search.ToLower()))
            .ToList();
    }
}
