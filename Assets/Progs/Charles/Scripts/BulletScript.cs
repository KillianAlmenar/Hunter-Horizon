using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public EquipmentData weaponData;
    bool isCrit;
    int critStat;

    public AttackScript attackScript;
    void ApplyEffect(Elements.Element type, Collider collider)
    {
        switch (type)
        {
            case Elements.Element.FIRE:
                if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.FROZEN)
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.FUSION);

                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.WINDED)
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.FIRESTORM);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.SHOCKED)

                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.SPARKLING);

                }
                else
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().weaponData = weaponData;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ElementalHit(collider, weaponData.element);

                }
                break;
            case Elements.Element.WIND:
                
                if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.FROZEN)
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.BLIZZARD);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.BURNT)
                {
                    Debug.Log("BURNED");
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.FIRESTORM);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.SHOCKED)

                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.STORM);

                }
                else
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().weaponData = weaponData;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ElementalHit(collider, weaponData.element);

                }

                break;
            case Elements.Element.ELECTRICITY:
                
                if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.FROZEN)
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.SUPRACONDUCTION);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.BURNT)

                {
                    Debug.Log("BURNED");
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.SPARKLING);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.WINDED)

                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.STORM);

                }
                else
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().weaponData = weaponData;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ElementalHit(collider, weaponData.element);

                }
                break;
            case Elements.Element.ICE:
               
                if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.SHOCKED)
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.SUPRACONDUCTION);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.BURNT)
                {
                    Debug.Log("BURNED");

                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.FUSION);


                }
                else if (collider.gameObject.GetComponent<ElementalAttackScript>().actualStatus == ElementalAttackScript.Status.WINDED)

                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ComboHit(collider, ElementalAttackScript.ComboStatus.BLIZZARD);


                }
                else
                {
                    collider.gameObject.GetComponent<ElementalAttackScript>().elapsed = 5f;
                    collider.gameObject.GetComponent<ElementalAttackScript>().weaponData = weaponData;

                    collider.gameObject.GetComponent<ElementalAttackScript>().ElementalHit(collider, weaponData.element);

                }
                break;
        }
    }

    float AppliedDamage(Collider collider, float damageStat)
    {
        //récupère la def de l'ennemi pour déterminer les dégats
        float damage = (float)(damageStat * damageStat / collider.GetComponent<EnemyStatScript>().defStat);

        if (Random.Range(0, 100) < weaponData.criticalRate)
        {

            damage *= 1.5f;
        }
        damage = attackScript.TypeTable(collider, weaponData, damageStat);

        damage = Mathf.Max(damage, 1);
        return damage;
    }

    void LaunchAttack(Collider collider, float attackDamage)
    {
        if (Random.Range(0, 100) <= weaponData.criticalRate)
        {
            isCrit = true;
            critStat = 2;
        }
        else
        {
            isCrit = false;
            critStat = 1;
        }

        switch (collider.tag)
        {

            case "Leg":
                attackDamage = AppliedDamage(collider, attackDamage);


                attackDamage = attackDamage * 0.75f;

                if (isCrit == true)
                {
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, true);
                    if (collider.GetComponent<Enemy>().element != weaponData.element)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    
                   

                }
                else
                {
                    Debug.Log("Leg Damage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, false);
                    if (collider.GetComponent<Enemy>().element != weaponData.element)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);
                    collider.GetComponent<tempBoss>().Hit(attackDamage);

                }

                break;
            case "Body":
                attackDamage = AppliedDamage(collider, attackDamage);


                if (isCrit == true)
                {
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, true);
                    if (collider.GetComponent<Enemy>().element != weaponData.element)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);
                    collider.GetComponent<tempBoss>().Hit(attackDamage);

                }
                else
                {
                    Debug.Log("Body Damage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, false);
                    if (collider.GetComponent<Enemy>().element != weaponData.element)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);
                    collider.GetComponent<tempBoss>().Hit(attackDamage);

                }

                break;
            case "Head":
                attackDamage = AppliedDamage(collider, attackDamage);

                attackDamage = (int)(attackDamage * 1.25);

                if (isCrit == true)
                {
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, true);
                    if (collider.GetComponent<Enemy>().element != weaponData.element)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);
                    collider.GetComponent<tempBoss>().Hit(attackDamage);

                }
                else
                {
                    Debug.Log("Head Damage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, false);
                    if (collider.GetComponent<Enemy>().element != weaponData.element)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);
                    collider.GetComponent<tempBoss>().Hit(attackDamage);

                }
                break;
        }
        collider.GetComponent<Rigidbody>().AddForce(((collider.transform.position - transform.position).normalized + (Vector3.up * 0.5f)) * weaponData.knockbackForce, ForceMode.Impulse);



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Head" || other.tag == "Leg" || other.tag == "Body" || other.tag == "Enemy")
        {
            float attackDamage = Random.Range((FindAnyObjectByType<PlayerStatScript>().playerAtk + weaponData.attack) - weaponData.attackRange, (FindAnyObjectByType<PlayerStatScript>().playerAtk + weaponData.attack) + weaponData.attackRange);

            LaunchAttack(other, attackDamage);
            Destroy(gameObject);



        }
        else if (other != null)
        {
        }
        if (other.GetComponentInParent<PalmTree>() != null)
        {
            other.GetComponentInParent<PalmTree>().Hit();
        }

    }


}
