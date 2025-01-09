using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDropScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Item equipmentData;
    public GameObject weaponInfo;
    public TextMeshProUGUI text1;


    // Update is called once per frame
    void Update()
    {
        text1.text = equipmentData.itemName;
        switch (equipmentData.rarity)
        {
            case Item.Rarity.COMMON:
                text1.color = Color.gray;
                break;
            case Item.Rarity.RARE:
                text1.color = Color.blue;
                break;
            case Item.Rarity.EPIC:
                text1.color = new Color(1f,0f,1f);
                break;
            case Item.Rarity.LEGENDARY:
                text1.color =Color.yellow;
                break;
        }
    }
    void DisplayItem()
    {
        weaponInfo.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            DisplayItem();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            weaponInfo.SetActive(false);
        }

    }

}
