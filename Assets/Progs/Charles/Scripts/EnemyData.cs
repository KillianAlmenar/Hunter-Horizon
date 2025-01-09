using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "MHLike/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float cooldown;
    public float maxHp;
    public int targetingRange;
    public int checkAllyRange;
    public string enemyName;
    public int attackStat;
    public int attackStatRange;
    public int defStat;
    public int critStat;
    public int speedStat;
    public Elements element;
}
