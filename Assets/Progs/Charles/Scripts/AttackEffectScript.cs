using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ElementalAttackScript;
using UnityEngine.AI;

public class AttackEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Color elemColor;

    public Status actualStatus = Status.NONE;
    public ComboStatus actualCombosStatus = ComboStatus.NONE;
    public float timer;
    float effectTime = 5f;

    bool isFrozen;
    bool isWindy;
    bool isBurnt;
    bool isElectrified;
    bool isStunned;

    public EquipmentData weaponData;

    float burnRate = 1f;
    int burnDamage = 1;


    float stunTime = 5f;
    float elecRate = 2f;

    float sparkCountRate = 1f;

    float stormRate = 2f;

    int blizzardDamage = 10;
    float blizzardRate = 1f;

    int fusionDamage = 1;
    float fusionRate = 1f;

    int fireStormDamage = 10;
    float fireStormRate = 1f;

    float supraconductRate = 1f;
 
    float elapsed = 0f;
    float elapsed2 = 0f; //used for elements ticking


    bool textDisplay = false;
    bool textComboDisplay = false;

    public GameObject target;
    float tempSpeed;
    public float tempDef;
    float tempAttack;
    Enemy enemy;
    tempBoss boss;
    public void SetTarget(GameObject newtarget)
    {
        target = newtarget;
        enemy = newtarget.GetComponent<Enemy>();
        boss = newtarget.GetComponent<tempBoss>();

    }

    public void EffectsUpdate()
    {
        if (weaponData.talent == TalentsScript.WeaponTalents.InfernalFire)
        {
            burnDamage = 2;
        }
        if (actualStatus==Status.NONE&&actualCombosStatus==ComboStatus.NONE)
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (weaponData.talent == TalentsScript.WeaponTalents.Overcharged)
        {
            elecRate = 1f;
            burnRate = 0.5f;
            blizzardRate = 0.5f;
            fireStormRate = 0.5f;
            fusionRate = 0.5f;
            sparkCountRate = 0.5f;
            supraconductRate = 0.5f;
            stormRate = 1f;
        }

        elapsed += Time.deltaTime;
        elapsed2 += Time.deltaTime;

        switch (actualStatus)
        {

            case (Status.FROZEN):
                elemColor = new Color(155f / 255f, 248f / 255f, 255f / 255f);


                if (!textDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "FROZEN", elemColor, false);
                    
                    textDisplay = true;
                }
                target.GetComponent<EnemyStatScript>().speedStat = 0;
                break;
            case (Status.BURNT):

                
                if (elapsed >= burnRate)
                {
                    elapsed = elapsed % burnRate;
                    elemColor = new Color(241f / 255f, 47f / 255f, 47f / 255f);
                    
                    DamagePopUp.current.CreatePopUp(target.transform.position, burnDamage.ToString(), elemColor, false);
                    enemy.Hit(burnDamage);
                }
                break;

            case (Status.WINDED):

                elemColor = new Color(114f / 255f, 238f / 255f, 107f / 255f);

                if (!textDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "WINDY", elemColor, false);
                    textDisplay = true;
                    if (weaponData.talent == TalentsScript.WeaponTalents.MercilessStorm)
                    {
                        target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 80f;

                    }
                    else
                    {
                        target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 90f;
                    }

                }

                break;

            case (Status.SHOCKED):

                

                if (elapsed >= elecRate && !isStunned)
                {
                    elemColor = new Color(254f / 255f, 255f / 255f, 85f / 255f);

                    DamagePopUp.current.CreatePopUp(target.transform.position, "STUNNED", elemColor, false);

                    elapsed = elapsed % elecRate;

                    isStunned = true;
                }
                if (isStunned && elapsed >= stunTime)
                {


                    isStunned = false;
                    elapsed = elapsed % stunTime;

                }
                if (isStunned)
                {
                    target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    target.GetComponent<NavMeshAgent>().isStopped = true;

                }
                else
                {
                    target.GetComponent<NavMeshAgent>().isStopped = false;
                }
                break;

            case (Status.NONE):
                break;

        }

        switch (actualCombosStatus)
        {
            case (ComboStatus.SPARKLING):
                elemColor = new Color(247f / 255f, 230f / 255f, 126f / 255f);

                if (!textComboDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "SPARKLING!", elemColor, false);
                    textComboDisplay = true;

                }
                if (elapsed >= 1f && !isStunned)
                {

                    DamagePopUp.current.CreatePopUp(target.transform.position, "STUNNED", elemColor, false);

                    elapsed = elapsed % elecRate;

                    isStunned = true;
                }
                if (isStunned && elapsed >= 1f)
                {


                    isStunned = false;
                    elapsed = elapsed % stunTime;

                }
                if (isStunned)
                {
                    target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    target.GetComponent<NavMeshAgent>().isStopped = true;

                }
                else
                {
                    target.GetComponent<NavMeshAgent>().isStopped = false;
                }
                if (elapsed2 >= burnRate)
                {
                    elapsed2 = elapsed2 % burnRate;

                    DamagePopUp.current.CreatePopUp(target.transform.position, burnDamage.ToString(), elemColor, false);
                    enemy.Hit(burnDamage);
                }
                break;
            case (ComboStatus.STORM):
                elemColor = new Color(113f / 255f, 125f / 255f, 126f / 255f);



                if (!textComboDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "STORM!", elemColor, false);
                    textComboDisplay = true;

                }
                if (elapsed >= elecRate && !isStunned)
                {

                    DamagePopUp.current.CreatePopUp(target.transform.position, "STUNNED", elemColor, false);

                    elapsed = elapsed % elecRate;

                    isStunned = true;
                }
                if (isStunned && elapsed >= stunTime)
                {


                    isStunned = false;
                    elapsed = elapsed % stunTime;

                }
                if (isStunned)
                {
                    target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    target.GetComponent<NavMeshAgent>().isStopped = true;

                }
                else
                {
                    target.GetComponent<NavMeshAgent>().isStopped = false;
                }
                if (weaponData.talent == TalentsScript.WeaponTalents.MercilessStorm)
                {
                    target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 80f;

                }
                else
                {
                    target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 90f;
                }
                break;
            case (ComboStatus.BLIZZARD):
                elemColor = new Color(174f / 255f, 214f / 255f, 241f / 255f);


                if (!textComboDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "BLIZZARD!", elemColor, false);
                    textComboDisplay = true;

                }
                if (elapsed >= 1f)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, blizzardDamage.ToString(), elemColor, false);
                    enemy.Hit(blizzardDamage);
                    boss.Hit(blizzardDamage);

                    elapsed = 0f;

                    GetComponent<EnemyStatScript>().speedStat = tempSpeed - ((tempSpeed / 100) * 80);
                }
                if (weaponData.talent == TalentsScript.WeaponTalents.MercilessStorm)
                {
                    target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 80f;

                }
                else
                {
                    target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 90f;
                }
                break;
            case (ComboStatus.FUSION):
                elemColor = new Color(247f / 255f, 126f / 255f, 126f / 255f);


                if (!textComboDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "FUSION!", elemColor, false);
                    textComboDisplay = true;

                }
                target.GetComponent<EnemyStatScript>().defStat = tempDef - ((tempDef / 100) * 40);

                if (elapsed >= 1f)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, fusionDamage.ToString(), elemColor, false);
                    enemy.Hit(fusionDamage);
                    boss.Hit(fusionDamage);

                    elapsed = 0f;
                }
                break;

            case (ComboStatus.FIRESTORM):
                elemColor = new Color(123f / 255f, 36f / 255f, 28f / 255f);


                if (!textComboDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "FIRESTORM!", elemColor, false);
                    textComboDisplay = true;

                }
                if (elapsed >= fireStormRate)
                {
                    elapsed = 0f; 
                    DamagePopUp.current.CreatePopUp(target.transform.position, fireStormDamage.ToString(), elemColor, false);
                    enemy.Hit(fireStormDamage);
                    boss.Hit(fireStormDamage);
                }
                if (weaponData.talent == TalentsScript.WeaponTalents.MercilessStorm)
                {
                    target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 80f;

                }
                else
                {
                    target.GetComponent<EnemyStatScript>().defStat = (tempDef / 100f) * 90f;
                }
                break;
            case (ComboStatus.SUPRACONDUCTION):
                elemColor = new Color(200f / 255f, 74f / 255f, 251f / 255f);



                if (!textComboDisplay)
                {
                    DamagePopUp.current.CreatePopUp(target.transform.position, "SUPRACONDUCT!", elemColor, false);
                    textComboDisplay = true;
                }
                if (elapsed >= elecRate && !isStunned)
                {
                    elemColor = new Color(254f / 255f, 255f / 255f, 85f / 255f);

                    DamagePopUp.current.CreatePopUp(target.transform.position, "STUNNED", elemColor, false);

                    elapsed = elapsed % elecRate;

                    isStunned = true;
                }
                if (isStunned && elapsed >= stunTime)
                {


                    isStunned = false;
                    elapsed = elapsed % stunTime;

                }
                if (isStunned)
                {
                    target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    target.GetComponent<NavMeshAgent>().isStopped = true;

                }
                else
                {
                    target.GetComponent<NavMeshAgent>().isStopped = false;
                }
                break;
        }
    }
}
