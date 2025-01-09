
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public enum Type
    {
        Hunt,
        Collect
    }

    public string id;
    public string questName = " ";
    public string objectiveId = " ";

    public string questDescription;
    public Type type;
    public int quantity=0;

    public int goldReward;
    public List<Item> itemReward = new();

    public bool isCompleted = false;

    public Elements.Element element = Elements.Element.NONE;


    public Quest()
    {
    }

    
    public Quest Clone()
    {
        Quest newQuest = new Quest();

        newQuest.id = id;
        newQuest.questName = questName;
        newQuest.objectiveId = objectiveId;

        newQuest.questDescription = questDescription; 
        newQuest.type = type;
        newQuest.quantity = quantity;
        newQuest.goldReward = goldReward;
        newQuest.itemReward = itemReward;
        newQuest.isCompleted = isCompleted;
        newQuest.element = element;

        return newQuest;
    }

    public void UpdateQuest(string _id)
    {
        if (_id == objectiveId)
        {
            quantity--;
        }

        if (quantity <= 0)
        {
            CompleteQuest();
        }
    }

    
    public void CompleteQuest()
    {
        if (!isCompleted)
        {
            for (int i = 0; i < itemReward.Count; i++)
            {              
                Inventory.instance.AddItem(itemReward[i]);
                RewardManager.rewardData.Add(itemReward[i]);
                RewardManager.questEnd = true;
            }

            Inventory.instance.golds += goldReward;
            GameManager.instance.InstantiateEndQuest(questName);
            isCompleted = true;
        }
    }
}
