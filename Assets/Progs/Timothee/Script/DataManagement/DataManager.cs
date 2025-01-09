using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    /* Data to save and put in a JSON FILE
    * 
    *     public List<Item> items = new List<Item>();
    *     public List<Item> hotBarItemList = new List<Item>();
    *     public List<Quest> activeQuest = new List<Quest>();
    *     public int golds;
    */

    //[SerializeField] string path = @"C:\Users\Etudiant2\Desktop";
    [SerializeField] string fileName = @"HunterHorizonSave";
    string extension = @".json";

    //[Header("Test buttons")]
    //[SerializeField] bool save_bool = false;
    //[SerializeField] bool load_bool = false;

    private Inventory inventoryData;

    Dictionary<string, int> itemInInventory = new(); //key ID value Quantity
    Dictionary<string, int> itemInHotBar = new(); //key ID value Quantity
    Dictionary<string, int> activeQuests = new(); //key ID value progressQuantity

    ItemDataBase itemDataBase;
    QuestDataBase questDataBase;

    private void Start()
    {

        //itemDataBase.GetItemById()
    }
    public void SaveData() //Set all data listed above into our containers, and then into our JSON file
    {
        inventoryData = Inventory.instance;

        //Step 1 : Reset every container values
        itemInInventory.Clear();
        //itemInHotBar.Clear();
        activeQuests.Clear();

        //Step 2 : stock everything in these containers
        foreach (var item in inventoryData.items)
        {
            if (!itemInInventory.ContainsKey(item.id))
            {
                itemInInventory.Add(item.id, 1);
            }
            else
            {
                itemInInventory[item.id]++;
            }
        }
        itemInInventory.Add("golds", inventoryData.golds);


        //foreach (var item in inventoryData.hotBarItemList)
        //{
        //    if (!itemInHotBar.ContainsKey(item.id))
        //    {
        //        itemInHotBar.Add(item.id, 1);
        //    }
        //    else
        //    {
        //        itemInHotBar[item.id]++;
        //    }
        //}

        foreach (var quest in inventoryData.activeQuest)
        {
            if (!activeQuests.ContainsKey(quest.id))
            {
                activeQuests.Add(quest.id, 1);
            }
            else
            {
                activeQuests[quest.id]++;
            }
        }

        //Step 3 : Serialize everything in JSON Files
        string inventoryJSON = JsonConvert.SerializeObject(itemInInventory);
        //string hotbarJSON = JsonConvert.SerializeObject(itemInHotBar);
        string questJSON = JsonConvert.SerializeObject(activeQuests);
        File.WriteAllText(Application.dataPath + @"\" + fileName + "Inventory" + extension, inventoryJSON);
        //File.WriteAllText(path + @"\" + fileName + "Hotbar" + extension, hotbarJSON);
        File.WriteAllText(Application.dataPath + @"\" + fileName + "Quest" + extension, questJSON);
        Debug.Log("Files Saved in the following file" + Application.dataPath);
    }

    public bool IsThereProgress() //Do we have a save ? 
    {
        if (File.Exists(Application.dataPath+@"\"+fileName + "Inventory"+extension)&&File.Exists(Application.dataPath+@"\"+fileName + "Quest"+extension))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetProgress()
    {
        /*Inventory.instance.items.Clear();
        Inventory.instance.golds = 0;
        //Inventory.instance.hotBarItemList.Clear();
        Inventory.instance.activeQuest.Clear();*/
    }
    public void LoadData() //Inventory.instance = JsonConvert.DeserializeObject<Inventory>(json);
    {
        itemDataBase = Resources.Load<ItemDataBase>("ItemsDatabase");
        questDataBase = Resources.Load<QuestDataBase>("QuestDatabase");

        ResetProgress();

        if (!IsThereProgress()) //Do we have a save ? If not
        {
            return; //Don't need to Load anything, just start a new game
        }

        //Checking each file after the new game code is simply a security, you can remove it if you feel like it 
        if (File.Exists(Application.dataPath + @"\"+fileName + "Inventory"+extension))
        {
            using (StreamReader r = new StreamReader(Application.dataPath +@"\"+fileName+"Inventory"+extension))
            {
                string json = r.ReadToEnd();
                Dictionary<string, int> tempDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

                foreach (string key in tempDictionary.Keys)
                {
                    if (key != "golds")
                    {
                        Item tempItem = new Item();
                        int quantity = tempDictionary[key];

                        //tempItem.id = key;
                        tempItem = ItemDataBase.GetItemById(key);

                        for (int i = 0; i < quantity; i++)
                        {
                            Inventory.instance.items.Add(tempItem);
                        }
                    }
                    else
                    {
                        Inventory.instance.golds = tempDictionary[key];
                    }
                }
            }
            Debug.Log("Inventory Loaded");
        }
        //Need to fix hotbar before
        //if (File.Exists(path+@"\"+fileName + "HotBar"+extension))
        //{
        //    using (StreamReader r = new StreamReader(path+@"\"+fileName+"HotBar"+extension))
        //    {
        //        string json = r.ReadToEnd();
        //        Dictionary<string, int> tempDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

        //        foreach(string key in tempDictionary.Keys) 
        //        {
        //            Item tempItem = new Item();
        //            int quantity = tempDictionary[key];

        //            //tempItem.id = key;
        //            tempItem = itemDataBase.GetItemById(key);

        //            for (int i = 0; i < quantity; i++)
        //            {
        //                Inventory.instance.items.Add(tempItem);
        //            }           
        //        }
        //    }
        //    Debug.Log("HotBar Loaded");
        //}

        if (File.Exists(Application.dataPath+@"\"+fileName + "Quest"+extension))
        {
            using (StreamReader r = new StreamReader(Application.dataPath+@"\"+fileName+"Quest"+extension))
            {
                string json = r.ReadToEnd();
                Dictionary<string, int> tempDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json); //Recover the dictionary data from json file

                foreach (string key in tempDictionary.Keys)
                {
                    Quest tempQuest = new Quest();

                    //tempQuest.id = key;
                    tempQuest = questDataBase.GetQuestsById(key);
                    tempQuest.quantity = tempDictionary[key];

                    Inventory.instance.activeQuest.Add(tempQuest);
                }
            }
            Debug.Log("Quests Loaded");
        }
    }
}