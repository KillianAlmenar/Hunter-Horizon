using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDataBase")]
public class EquipmentDataBase : ScriptableObject
{
    public List<EquipmentData> equipments = new List<EquipmentData>();
}
