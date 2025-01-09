using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "WeaponData", menuName = "MHLike/WeaponData")]
[System.Serializable]
public class WeaponsData : Item
{
    public WeaponType weaponType;
    public int attack;
    public int attackRange;
    public int criticalRate;
    public int knockbackForce;
    public Color color;
    public Elements.Element element;
    public TalentsScript.WeaponTalents talents;
}



