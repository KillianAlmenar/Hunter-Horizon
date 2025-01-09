using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StuffScript : MonoBehaviour
{

    //RAPPEL CHARLES ESSAIE DE PLACER DES NOUVELLES IMAGES SUR LES SLOTS ET SOUS LE SELECTED



    public List<EquipmentData> stuffData = new List<EquipmentData>();//recuperer dans ressources
    public List<EquipmentData> equippedStuffData = new List<EquipmentData>();//recuperer dans ressources

    Sprite swordTemp;

    bool noHead;
    bool noArm;
    bool noTorso;
    bool noLeg;
    bool noClose;

    bool noRange;


    public ItemSlot swordSlot;
    public ItemSlot gunSlot;
    // Start is called before the first frame update
    void Start()
    {
        stuffData = Resources.Load<EquipmentDataBase>("EquipmentDataBase").equipments;
        for (int i = 0; i < 6; i++) 
        {
            EquipmentData temp = new EquipmentData();
            equippedStuffData.Add(temp);
        }

    }

    // Update is called once per frame
    void Update()
    {
    

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            stuffData[0].isEquipped = true;
            stuffData[1].isEquipped = true;

            
            stuffData[6].isEquipped=false;
            stuffData[7].isEquipped=false;
            stuffData[8].isEquipped=false;

            stuffData[9].isEquipped=false;
            stuffData[10].isEquipped=false;
            stuffData[11].isEquipped=false;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            stuffData[0].isEquipped = false;
            stuffData[1].isEquipped = false;


            stuffData[6].isEquipped = true;
            stuffData[7].isEquipped = false;
            stuffData[8].isEquipped = false;

            stuffData[9].isEquipped = true;
            stuffData[10].isEquipped = false;
            stuffData[11].isEquipped = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            stuffData[0].isEquipped = false;
            stuffData[1].isEquipped = false;


            stuffData[6].isEquipped = false;
            stuffData[7].isEquipped = true;
            stuffData[8].isEquipped = false;

            stuffData[9].isEquipped = false;
            stuffData[10].isEquipped = true;
            stuffData[11].isEquipped = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            stuffData[0].isEquipped = false;
            stuffData[1].isEquipped = false;


            stuffData[6].isEquipped = false;
            stuffData[7].isEquipped = false;
            stuffData[8].isEquipped = true;

            stuffData[9].isEquipped = false;
            stuffData[10].isEquipped = false;
            stuffData[11].isEquipped = true;

        }
        noHead =!stuffData.Any(stuffData => stuffData.isEquipped && stuffData.equipmentType == EquipmentType.HEAD);
        noArm=!stuffData.Any(stuffData => stuffData.isEquipped && stuffData.equipmentType == EquipmentType.ARM);
        noTorso=!stuffData.Any(stuffData => stuffData.isEquipped && stuffData.equipmentType == EquipmentType.TORSO);
        noLeg=!stuffData.Any(stuffData => stuffData.isEquipped && stuffData.equipmentType == EquipmentType.LEG);

        noClose=!stuffData.Any(stuffData => stuffData.isEquipped && (stuffData.equipmentType == EquipmentType.WEAPON && stuffData.weaponType == WeaponType.Close));



        noRange=!stuffData.Any(stuffData => stuffData.isEquipped && (stuffData.equipmentType == EquipmentType.WEAPON && stuffData.weaponType == WeaponType.Range));

        foreach (EquipmentData equipmentData in stuffData)
        {
            equipmentData.itemName = equipmentData.equipmentName;
            if (equipmentData.equipmentType == EquipmentType.HEAD && equipmentData.isEquipped)
            {
                equippedStuffData[0] = equipmentData;
            }

            if (equipmentData.equipmentType == EquipmentType.ARM && equipmentData.isEquipped)
            {
                equippedStuffData[1] = equipmentData;
            }

            if (equipmentData.equipmentType == EquipmentType.TORSO && equipmentData.isEquipped)
            {
                equippedStuffData[2] = equipmentData;
            }

            if (equipmentData.equipmentType == EquipmentType.LEG && equipmentData.isEquipped)
            {
                equippedStuffData[3] = equipmentData;
            }
            if ((equipmentData.equipmentType == EquipmentType.WEAPON && equipmentData.isEquipped) && equipmentData.weaponType == WeaponType.Close)
            {
                equippedStuffData[4] = equipmentData;
                swordSlot.Item = equipmentData;
                swordSlot.IconItem.sprite = equipmentData.itemIcon;

            }

            if ((equipmentData.equipmentType == EquipmentType.WEAPON && equipmentData.isEquipped)&& equipmentData.weaponType == WeaponType.Range)
            {
                equippedStuffData[5] = equipmentData;
                gunSlot.Item = equipmentData;
                gunSlot.IconItem.sprite = equipmentData.itemIcon;

            }

        }
        if(noHead)
        {
            equippedStuffData[0] = null;
        }
        if (noArm)
        {
            equippedStuffData[1] = null;
        }
        if (noTorso)
        {
            equippedStuffData[2] = null;
        }
        if (noLeg)
        {
            equippedStuffData[3] = null;
        }
        if (noClose)
        {
            equippedStuffData[4] = null;
            swordSlot.Item = null;
            swordSlot.IconItem.sprite = null;

        }
        if (noRange)
        {
            equippedStuffData[5] = null;
            gunSlot.Item = null;
            gunSlot.IconItem.sprite = null;

        }

    }


    public void ReplaceEquipment(EquipmentData newEquipmentData)
    {
        foreach (EquipmentData equipmentData in equippedStuffData)
        {
            if(equipmentData.equipmentType == newEquipmentData.equipmentType)
            {
                equippedStuffData[equippedStuffData.IndexOf(equipmentData)] = newEquipmentData; 
            }
        }
    }
}
