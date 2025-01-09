using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#region enum
public enum EquipmentType
{
    WEAPON,
    HEAD,
    TORSO,
    ARM,
    LEG,
}
public enum Talent //Ideas
{
    NONE,
    //WeaponTalent
    DragonHeart,
    FireBorn,
    WindGod,
    LightningEdge,
    IceMan,
    //ArmorTalent
    MAX_HP, //Add X amount of HP to player's stat
    MOVEMENT_SPEED,//Add X amount of movement speed to player's stat
    LOOTING, //+ X amount loot from ennemies
    REFLEXION,//Reflect X amount of damage whenever player take damage (cooldown)
    BARRIER, //Ignore damage from a lethal attack (cooldown)
    AGILITY, //Multiply by X amount of temporary movement speed after swapping weapons (cooldown)
}
public enum WeaponType
{
    Close,
    Range
}
#endregion
[System.Serializable]
public class EquipmentData : Item
{
    public string equipmentName;
    public EquipmentType equipmentType;
    public TalentsScript.WeaponTalents talent;

    //Stat that can be granted by the equipment
    public int attack;
    public int defense;
    public int criticalRate;
    public int maxHealth;

    public float knockbackForce;
    public float movementSpeed;
    public float attackRange;

    public WeaponType weaponType; //Null if equipment is not a weapon
    public Elements.Element element;
    public Color color;

    public bool isEquipped;
}
