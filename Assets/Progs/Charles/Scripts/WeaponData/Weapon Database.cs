using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase")]
public class WeaponDataBase : ScriptableObject
{
    public List<WeaponsData> weapons = new List<WeaponsData>();
}
