using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryQuest : MonoBehaviour
{
    [SerializeField] GameObject prefabQuest;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Quest q in Inventory.instance.activeQuest)
        {
           GameObject temp = Instantiate(prefabQuest, transform);
           temp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = q.questName;
           temp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Remaining : " + q.quantity;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
