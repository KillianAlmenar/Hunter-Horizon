using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class QuestDataWindow : EditorWindow
{
    QuestDataBase database;

    [MenuItem("Tools/Quest Database")]
    public static void OpenWindow()
    {
        GetWindow<QuestDataWindow>("Quest Database");
    }

    ItemDataBase databaseItem;
    int selectedResultIndex = 0;
    int selectedNeedIndex = 0;

    private void OnEnable()
    {
        database = Resources.Load<QuestDataBase>("QuestDatabase");
        databaseItem = Resources.Load<ItemDataBase>("ItemsDatabase");
    }

    Vector2 scrollPosition = Vector2.zero;

    string newQuestId = "default_id";
    string search = "";

    List<Quest> searchQuest = new List<Quest>();

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
        if (GUILayout.Button("New Quest"))
        {
            database.quests.Add(new Quest() { id = newQuestId });
        }

        GUI.color = baseColor;

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID", GUILayout.Width(100));
        EditorGUILayout.LabelField("Name", GUILayout.Width(200));
        EditorGUILayout.LabelField("Description", GUILayout.Width(400));
        EditorGUILayout.LabelField("Type", GUILayout.Width(100));
        EditorGUILayout.LabelField("Element", GUILayout.Width(100));
        EditorGUILayout.LabelField("ObjectiveID", GUILayout.Width(100));
        EditorGUILayout.LabelField("Quantity needed", GUILayout.Width(100));
        EditorGUILayout.LabelField("GoldReward", GUILayout.Width(100));
        EditorGUILayout.LabelField("ItemsReward", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < searchQuest.Count; i++)
        {
            Quest quest = searchQuest[i];

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(quest.id, GUILayout.Width(100));

            quest.questName = EditorGUILayout.TextField(quest.questName, GUILayout.Width(200));

            quest.questDescription = EditorGUILayout.TextArea(quest.questDescription, GUILayout.Width(400)) ;

            quest.type = (Quest.Type)EditorGUILayout.EnumPopup(quest.type, GUILayout.Width(100));

            quest.element = (Elements.Element)EditorGUILayout.EnumPopup(quest.element, GUILayout.Width(100));

            quest.objectiveId = EditorGUILayout.TextArea(quest.objectiveId, GUILayout.Width(100));

            quest.quantity = EditorGUILayout.IntField(quest.quantity, GUILayout.Width(100));

            //quest.itemReward = 

            quest.goldReward = EditorGUILayout.IntField(quest.goldReward, GUILayout.Width(100));

            // Afficher le bouton déroulant avec les options
            if (EditorGUILayout.DropdownButton(new GUIContent(databaseItem.items[selectedResultIndex].itemName), FocusType.Passive, GUILayout.Width(100)))
            {
                // Afficher le menu déroulant
                GenericMenu menu = new GenericMenu();
                for (int k = 0; k < databaseItem.items.Count; k++)
                {
                    int index = k;
                    menu.AddItem(new GUIContent(databaseItem.items[k].itemName), false, () => { selectedResultIndex = index; });
                }
                menu.ShowAsContext();
            }

            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                quest.itemReward.Add(databaseItem.items[selectedResultIndex]);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(50)))
            {
                quest.itemReward.Remove(quest.itemReward.Last());
            }

            
            GUILayout.Label("List :");
            foreach (Item item in quest.itemReward)
            {
                GUILayout.Label(item.itemName);
            }


            // bouton suppression
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                database.quests.Remove(quest);
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
        
        searchQuest = database.quests.
            Where(quest => quest.questName.ToLower().Contains(search.ToLower()) || quest.id.ToLower().Contains(search.ToLower()))
            .ToList();
        
    }
}
