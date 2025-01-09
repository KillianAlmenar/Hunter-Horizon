using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;
using UnityEditor.Search;

public class EquipmentDataTool : EditorWindow
{
    EquipmentDataBase database;
    [MenuItem("Tools/Equipment Database")]

    public static void OpenWindow()
    {
        GetWindow<EquipmentDataTool>("Equipment Database");
    }

    private void OnEnable()
    {
        database = Resources.Load<EquipmentDataBase>("EquipmentDataBase");
    }

    Vector2 scrollPosition = Vector2.zero;

    string newQuestId;
    string search = "";
    private static int equipmentCounter = 0;

    List<EquipmentData> searchEquipment = new List<EquipmentData>();

    private void OnGUI()
    {
        Color baseColor = GUI.color;

        int spacingLineValue = 75;
        int spacingLineValue2 = 125;
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

        newQuestId = "equipment_" + database.equipments.Count.ToString();
        GUI.color = Color.green;
        if (GUILayout.Button("New Equipment", GUILayout.Width(spacingLineValue2)))
        {
            // Utiliser l'identifiant généré pour le nouvel équipement
            database.equipments.Add(new EquipmentData() { id = newQuestId });
            // Incrémenter le compteur pour le prochain équipement
            equipmentCounter++;
        }

        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < searchEquipment.Count; i++)
        {
            EquipmentData equipment = searchEquipment[i];

            EditorGUILayout.BeginHorizontal();

            // bouton suppression
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                database.equipments.Remove(equipment);
            }
        GUI.color = baseColor;

            EditorGUILayout.LabelField(equipment.id, GUILayout.Width(spacingLineValue));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Equipped", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Name", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Type", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Rarity", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Color", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Talent", GUILayout.Width(spacingLineValue));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            equipment.isEquipped = EditorGUILayout.Toggle(equipment.isEquipped, GUILayout.Width(spacingLineValue));
            equipment.equipmentName = EditorGUILayout.TextField(equipment.equipmentName, GUILayout.Width(spacingLineValue));
            equipment.equipmentType = (EquipmentType)EditorGUILayout.EnumPopup(equipment.equipmentType, GUILayout.Width(spacingLineValue));
            equipment.rarity = (Item.Rarity)EditorGUILayout.EnumPopup(equipment.rarity, GUILayout.Width(spacingLineValue));
            equipment.color = EditorGUILayout.ColorField(equipment.color, GUILayout.Width(spacingLineValue));
            equipment.talent = (TalentsScript.WeaponTalents)EditorGUILayout.EnumPopup(equipment.talent, GUILayout.Width(spacingLineValue));
            equipment.itemIcon = (Sprite)EditorGUILayout.ObjectField("Icon:", equipment.itemIcon, typeof(Sprite), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Attack", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Defense", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Crit", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Health", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Knockback", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Movespeed", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Range", GUILayout.Width(spacingLineValue));
            EditorGUILayout.LabelField("Element", GUILayout.Width(spacingLineValue));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            equipment.attack = EditorGUILayout.IntField(equipment.attack,GUILayout.Width(spacingLineValue));
            equipment.defense = EditorGUILayout.IntField(equipment.defense,GUILayout.Width(spacingLineValue));
            equipment.criticalRate = EditorGUILayout.IntField(equipment.criticalRate, GUILayout.Width(spacingLineValue));
            equipment.maxHealth = EditorGUILayout.IntField(equipment.maxHealth, GUILayout.Width(spacingLineValue));
            equipment.knockbackForce = EditorGUILayout.FloatField(equipment.knockbackForce, GUILayout.Width(spacingLineValue));
            equipment.movementSpeed = EditorGUILayout.FloatField(equipment.movementSpeed, GUILayout.Width(spacingLineValue));
            equipment.attackRange = EditorGUILayout.FloatField(equipment.attackRange, GUILayout.Width(spacingLineValue));
            equipment.element = (Elements.Element)EditorGUILayout.EnumPopup(equipment.element, GUILayout.Width(spacingLineValue));

            EditorGUILayout.EndHorizontal();
          


            if (equipment.equipmentType == EquipmentType.WEAPON)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("WeaponType", GUILayout.Width(spacingLineValue));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                equipment.weaponType = (WeaponType)EditorGUILayout.EnumPopup(equipment.weaponType, GUILayout.Width(spacingLineValue));
                EditorGUILayout.EndHorizontal();
            }
           
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("- - - - - - - - - - - - - - - - - - - -", GUILayout.Width(200));
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
        searchEquipment = database.equipments.
                 Where(equipment => equipment.id.ToLower().Contains(search.ToLower()))
                 .ToList();
    }
}
