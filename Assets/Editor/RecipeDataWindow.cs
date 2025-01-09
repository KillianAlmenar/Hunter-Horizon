using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;

public class RecipeDataWindow : EditorWindow
{
    RecipeDataBase database;
    ItemDataBase databaseItem;
    WeaponDataBase databaseWeapon;

    [MenuItem("Tools/Recipe Database")]
    public static void OpenWindow()
    {
        GetWindow<RecipeDataWindow>("Recipe Database");
    }

    private void OnEnable()
    {
        database = Resources.Load<RecipeDataBase>("RecipeDatabase");
        databaseItem = Resources.Load<ItemDataBase>("ItemsDatabase");
        databaseWeapon = Resources.Load<WeaponDataBase>("WeaponDatabase");
    }

    Vector2 scrollPosition = Vector2.zero;

    string newRecipeId = "default_id";
    string search = "";

    List<Recipe> searchRecipe = new List<Recipe>();

    int selectedResultIndex = 0;
    int selectedNeedIndex = 0;

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
        newRecipeId = GUILayout.TextField(newRecipeId);
        GUI.color = Color.green;
        if (GUILayout.Button("New Recipe"))
        {
            database.recipes.Add(new Recipe()
            {
                id = newRecipeId
                , resultItem = databaseItem.items[selectedResultIndex]
            }) ;
        }

        GUI.color = baseColor;

        EditorGUILayout.EndHorizontal();


        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


        for (int i = 0; i < searchRecipe.Count; i++)
        {
            Recipe recipe = searchRecipe[i];

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(recipe.id, GUILayout.Width(100));

            // Afficher le bouton déroulant avec les options
            if (EditorGUILayout.DropdownButton(new GUIContent(databaseWeapon.weapons[selectedResultIndex].itemName), FocusType.Passive, GUILayout.Width(100)))
            {
                // Afficher le menu déroulant
                GenericMenu menu = new GenericMenu();
                for (int k = 0; k < databaseWeapon.weapons.Count; k++)
                {
                    int index = k;
                    menu.AddItem(new GUIContent(databaseWeapon.weapons[k].itemName), false, () => { selectedResultIndex = index; });
                }
                menu.ShowAsContext();
            }
            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                recipe.resultItem = databaseWeapon.weapons[selectedResultIndex];
            }

            // Afficher le bouton déroulant avec les options
            if (EditorGUILayout.DropdownButton(new GUIContent(databaseItem.items[selectedNeedIndex].itemName), FocusType.Passive, GUILayout.Width(100)))
            {
                // Afficher le menu déroulant
                GenericMenu menu = new GenericMenu();
                for (int k = 0; k < databaseItem.items.Count; k++)
                {
                    int index = k;
                    menu.AddItem(new GUIContent(databaseItem.items[k].itemName), false, () => { selectedNeedIndex = index; });
                }
                menu.ShowAsContext();
            }

            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                recipe.requiredItems.Add(databaseItem.items[selectedNeedIndex]);
            }



            GUILayout.Label("List :");
            foreach (Item item in recipe.requiredItems)
            {
                GUILayout.Label(item.itemName);
            }

            GUILayout.Label("Result :");
            GUILayout.Label(recipe.resultItem.itemName);

            // bouton suppression
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                database.recipes.Remove(recipe);
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
        searchRecipe = database.recipes;/*.
                 Where(recipe => recipe.resultItem.itemName.ToLower().Contains(search.ToLower()) || recipe.id.ToLower().Contains(search.ToLower()))
                 .ToList();*/

    }
}
