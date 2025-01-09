using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;

public class WeaponDataWindow : EditorWindow
{
    WeaponDataBase database;

    [MenuItem("Tools/Weapon Database")]
    public static void OpenWindow()
    {
        GetWindow<WeaponDataWindow>("Weapon Database");
    }

    private void OnEnable()
    {
        database = Resources.Load<WeaponDataBase>("WeaponDatabase");
    }

    Vector2 scrollPosition = Vector2.zero;

    string newQuestId ;
    string search = "";

    List<WeaponsData> searchWeapon = new List<WeaponsData>();

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
        newQuestId = GUILayout.TextField(newQuestId);
        GUI.color = Color.green;
        newQuestId = "weapon_"+database.weapons.Count.ToString();
        if (GUILayout.Button("New Weapon"))
        {
            database.weapons.Add(new WeaponsData() { id = newQuestId });
        }

        GUI.color = baseColor;

        EditorGUILayout.EndHorizontal();


        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


        for (int i = 0; i < searchWeapon.Count; i++)
        {
            WeaponsData weapon = searchWeapon[i];

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(weapon.id, GUILayout.Width(100));

            weapon.itemName = EditorGUILayout.TextField(weapon.itemName);

            weapon.description = EditorGUILayout.TextField(weapon.description);

            weapon.rarity = (Item.Rarity)EditorGUILayout.EnumPopup(weapon.rarity);

            weapon.weaponType = (WeaponType)EditorGUILayout.EnumPopup(weapon.weaponType);

            weapon.attack = EditorGUILayout.IntField(weapon.attack);

            weapon.attackRange = EditorGUILayout.IntField(weapon.attackRange);

            weapon.criticalRate = EditorGUILayout.IntField(weapon.criticalRate);

            weapon.knockbackForce = EditorGUILayout.IntField(weapon.knockbackForce);

            weapon.color = EditorGUILayout.ColorField(weapon.color);

            weapon.element = (Elements.Element)EditorGUILayout.EnumPopup(weapon.element);

            weapon.talents = (TalentsScript.WeaponTalents)EditorGUILayout.EnumPopup(weapon.talents);





            // bouton suppression
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                database.weapons.Remove(weapon);
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

        searchWeapon = database.weapons.
            Where(weapons => weapons.id.ToLower().Contains(search.ToLower()))
            .ToList();

    }
}
