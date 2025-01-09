using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentsScript : MonoBehaviour
{

    public enum WeaponTalents
    {
        None,
        ElementalSurge,//Permet a une arme d'infliger les effets d'un �l�ment a un monstre du meme �l�ment
        InfernalFire,//Double la puissance des DoTs
        BoostedAttack,//Augmente l'attaque
        Overcharged,//Double la fr�quence des DoTs et des effets
        EternalSuffering, // double la dur�e pendant laquelle les effets restent actifs
        MercilessStorm //Double la baisse de def sur l'effet vent
    }

    public int weaponLevel;
}
