using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "QuestDatabase")]
public class QuestDataBase : ScriptableObject
{
    public List<Quest> quests = new List<Quest>();

    public Quest GetQuestsById(string id)
    {
        QuestDataBase database = Resources.Load<QuestDataBase>("QuestDatabase");
        List<Quest> searchQuest = new List<Quest>();

        searchQuest = database.quests.
    Where(quest => quest.questName.ToLower().Contains(id.ToLower()) || quest.id.ToLower().Contains(id.ToLower()))
    .ToList();

        return searchQuest.First();
    }
}
