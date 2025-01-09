using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMerchantDebug : MonoBehaviour
{

    [SerializeField] GameObject QuestsMerchant;
    [SerializeField] KeyCode keyQuest;

    [SerializeField] GameObject CraftMerchant;
    [SerializeField] KeyCode keyCraft;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestsMerchant != null && Input.GetKeyDown(keyQuest))
        {
            if (QuestsMerchant.activeSelf)
            {
                QuestsMerchant.SetActive(false);
            }
            else
            {
                QuestsMerchant.SetActive(true);
            }
        }

        if (CraftMerchant != null && Input.GetKeyDown(keyCraft))
        {
            if (CraftMerchant.activeSelf)
            {
                CraftMerchant.SetActive(false);
            }
            else
            {
                CraftMerchant.SetActive(true);
            }
        }
    }
}
