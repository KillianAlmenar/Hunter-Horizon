using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BladeScript : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject terrain;
    public GameObject swordTip;
    float terrainTimer;
    public EquipmentData weaponData;
    bool isCrit;
    int critStat;
    [SerializeField] ThirdPersonPlayer player;
    public GameObject[] simpleSlash;
    public GameObject[] simpleSlash2;
    public GameObject[] tripleSlash;
    public GameObject[] circularSlash;

    public GameObject[] tornadoesTab;
    public GameObject[] cracksTab;
    bool hasAttacked = false;
    [SerializeField] AttackScript attackScript;
    float attackTime;
    float slashTime;
    float slashTime2;
    int previousNbAttack = 0;
    int lastNbAttack = 0;
    [SerializeField] PlayerSoundPlayer soundPlayer;
    int nombreattack;
    Collider target;

    bool fullCombo;
    void Start()
    {
        terrainTimer = 6f;
        //player = GetComponentInParent<ThirdPersonPlayer>();
        hasAttacked = false;
        fullCombo = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.nbAttack >= 2)
        {
            fullCombo = true;
        }

        if (terrainTimer >= 0)
        {
            terrainTimer -= Time.deltaTime;
            terrain.layer = 7;
        }

        if (terrainTimer <= 0)
        {
            terrain.layer = 0;
        }

        if (player.attackComboPlayer == false)
        if (attackTime > 0)
        {
            hasAttacked = false;
        }
        {
            attackTime -= Time.deltaTime;
        }
        if (attackTime <= 0)
        {

        }
        if (slashTime > 0f)
        {
            slashTime -= Time.deltaTime;

        }
        if (slashTime <= 0f)
        {

            simpleSlash[0].SetActive(false);
            simpleSlash[1].SetActive(false);
            simpleSlash[2].SetActive(false);
            simpleSlash[3].SetActive(false);




        }
        if (slashTime2 > 0f)
        {

            slashTime2 -= Time.deltaTime;
        }
        if (slashTime2 <= 0f)
        {

            simpleSlash2[0].SetActive(false);
            simpleSlash2[1].SetActive(false);
            simpleSlash2[2].SetActive(false);
            simpleSlash2[3].SetActive(false);
        }


        nombreattack = player.GetNbAttack();

    }

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
                    if (previousNbAttack == 1)
                    {
                        Vector3 vec = new Vector3(collider.transform.position.x, 0, collider.transform.position.z);

                        Debug.Log("CRACK");
                        GameObject temp = Instantiate(cracksTab[0], vec, Quaternion.identity);
                        terrain.layer = 7;

                        Destroy(temp, 6f);
                    }

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
                    Debug.Log("wind");


                    if (player.GetNbAttack() == 3 && previousNbAttack == 1)
                    {
                        Vector3 vec = new Vector3(collider.transform.position.x, 0, collider.transform.position.z);

                        Debug.Log("windcrack");

                        GameObject temp = Instantiate(cracksTab[2], vec, Quaternion.identity);
                        terrain.layer = 7;

                        Destroy(temp, 6f);
                    }

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

                    /*if (player.GetNbAttack() == 3 && previousNbAttack == 2)
                    {
                        GameObject temp = Instantiate(tornadoesTab[3], vec, Quaternion.identity);
                        Destroy(temp, 6f);
                    }*/
                    if (player.GetNbAttack() == 3 && previousNbAttack == 1)
                    {
                        Vector3 vec = new Vector3(collider.transform.position.x, 1f, collider.transform.position.z);

                        GameObject temp = Instantiate(cracksTab[3], vec, Quaternion.identity);
                        terrain.layer = 7;
                        Destroy(temp, 6f);
                    }

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


                    if (player.GetNbAttack() == 3 && previousNbAttack == 1)
                    {
                        Vector3 vec = new Vector3(collider.transform.position.x, 0, collider.transform.position.z);

                        GameObject temp = Instantiate(cracksTab[1], vec, Quaternion.identity);
                        terrain.layer = 7;

                        Destroy(temp, 6f);
                    }


                }
                break;


        }
    }

    float AppliedDamage(Collider collider, float damageStat)
    {
        //récupère la def de l'ennemi pour déterminer les dégats
        float damage = (float)(damageStat * damageStat / collider.GetComponent<EnemyStatScript>().defStat);
        if (UnityEngine.Random.Range(0, 100) < weaponData.criticalRate)
        {

            damage *= 1.5f;
        }
        damage = attackScript.TypeTable(collider, weaponData, damage);
        damage = Mathf.Max(damage, 1);
        return damage;
    }

    void LaunchAttack(Collider collider, float attackDamage)
    {
        if (UnityEngine.Random.Range(0, 100) <= weaponData.criticalRate)
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
                    if (collider.GetComponent<Enemy>().element != weaponData.element || weaponData.talent == TalentsScript.WeaponTalents.ElementalSurge)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);

                }
                else
                {
                    Debug.Log("Leg Damage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, false);
                    if (collider.GetComponent<Enemy>().element != weaponData.element || weaponData.talent == TalentsScript.WeaponTalents.ElementalSurge)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);


                }

                break;
            case "Body":
                attackDamage = AppliedDamage(collider, attackDamage);


                if (isCrit == true)
                {
                    Debug.Log("CRITICAL!\nDamage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, true);
                    if (collider.GetComponent<Enemy>().element != weaponData.element || weaponData.talent == TalentsScript.WeaponTalents.ElementalSurge)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);

                }
                else
                {
                    Debug.Log("Body Damage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, false);
                    if (collider.GetComponent<Enemy>().element != weaponData.element || weaponData.talent == TalentsScript.WeaponTalents.ElementalSurge)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);

                }

                break;
            case "Head":
                attackDamage = AppliedDamage(collider, attackDamage);

                attackDamage = (int)(attackDamage * 1.25);

                if (isCrit == true)
                {
                    //Debug.Log("CRITICAL!\nDamage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, true);
                    if (collider.GetComponent<Enemy>().element != weaponData.element || weaponData.talent == TalentsScript.WeaponTalents.ElementalSurge)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);

                }
                else
                {
                    //Debug.Log("Head Damage : " + attackDamage);
                    DamagePopUp.current.CreatePopUp(collider.transform.position, ((int)attackDamage).ToString(), weaponData.color, false);
                    if (collider.GetComponent<Enemy>().element != weaponData.element || weaponData.talent == TalentsScript.WeaponTalents.ElementalSurge)
                    {
                        ApplyEffect(weaponData.element, collider);
                    }
                    collider.GetComponent<Enemy>().Hit(attackDamage);
                }
                break;
        }
        collider.GetComponent<Rigidbody>().AddForce(((collider.transform.position - transform.position).normalized + (Vector3.up * 0.5f)) * weaponData.knockbackForce, ForceMode.Impulse);


    }
    private void OnTriggerEnter(Collider other)
    {
        target = other;

        if (other.tag == "Head" || other.tag == "Leg" || other.tag == "Body" || other.tag == "Enemy")
        {

            previousNbAttack = lastNbAttack;

            if (player.nbAttack < 3)
            {
                soundPlayer.PlayAttack();
            }

            lastNbAttack = player.GetNbAttack();
            float attackDamage = UnityEngine.Random.Range((FindAnyObjectByType<PlayerStatScript>().playerAtk + weaponData.attack) - weaponData.attackRange, (FindAnyObjectByType<PlayerStatScript>().playerAtk + weaponData.attack) + weaponData.attackRange);
            hasAttacked = true;
            attackTime = 5f;
            LaunchAttack(other, attackDamage);
            //gameObject.SetActive(false);
        }

        if (other.GetComponentInParent<PalmTree>() != null)
        {

            hasAttacked = true;

            other.GetComponentInParent<PalmTree>().Hit();
        }

    }


    public void ActiveTornado()
    {
        soundPlayer.PlayHeavyAttack();
        //Debug.Log("NombreAttack : " + nombreattack);
        if (nombreattack == 1)
        {
            slashTime2 = 1f;

            simpleSlash2[0].SetActive(false);
            simpleSlash2[0].SetActive(true);
            float3 temp = new float3(swordTip.transform.position.x, -1f, swordTip.transform.position.z);

            terrainTimer = 6f;
            terrain.layer = 7;

            //Debug.Log("attack1");

            switch (weaponData.element)
            {
                case Elements.Element.FIRE:
                    GameObject fireTor = Instantiate(cracksTab[0], temp, Quaternion.identity);
                    Destroy(fireTor, 6f);
                    break;
                case Elements.Element.ICE:
                    GameObject iceTor = Instantiate(cracksTab[1], temp, Quaternion.identity);
                    Destroy(iceTor, 6f);
                    break;
                case Elements.Element.WIND:
                    GameObject windTor = Instantiate(cracksTab[2], temp, Quaternion.identity);
                    Destroy(windTor, 6f);
                    break;
                case Elements.Element.ELECTRICITY:
                    GameObject elecTor = Instantiate(cracksTab[3], temp, Quaternion.identity);
                    Destroy(elecTor, 6f);
                    break;
            }

        }
        else if (nombreattack == 2)
        {
            slashTime = 1f;

            simpleSlash[0].SetActive(false);
            simpleSlash[0].SetActive(true);
            float3 temp = new float3(swordTip.transform.position.x, 0f, swordTip.transform.position.z);
            terrain.layer = 7;
            Debug.Log("attack2");

            switch (weaponData.element)
            {

                case Elements.Element.FIRE:
                    GameObject fireTor = Instantiate(tornadoesTab[0], temp, Quaternion.identity);
                    Destroy(fireTor, 6f);
                    break;
                case Elements.Element.ICE:
                    GameObject iceTor = Instantiate(tornadoesTab[1], temp, Quaternion.identity);
                    Destroy(iceTor, 6f);
                    break;
                case Elements.Element.WIND:
                    GameObject windTor = Instantiate(tornadoesTab[2], temp, Quaternion.identity);
                    Destroy(windTor, 6f);
                    break;
                case Elements.Element.ELECTRICITY:
                    GameObject elecTor = Instantiate(tornadoesTab[3], temp, Quaternion.identity);
                    Destroy(elecTor, 6f);
                    break;
            }
        }
        fullCombo = false;

        /*Quaternion temp = Quaternion.Euler(-90, 0, 0);
          float3 tempPos = new float3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
          switch (weaponData.element)
          {
              case Elements.Element.FIRE:
                  GameObject fireTor = Instantiate(circularSlash[0],tempPos, temp);
                  Destroy(fireTor, 6f);
                  break;
              case Elements.Element.ICE:
                  GameObject iceTor = Instantiate(circularSlash[1], tempPos, temp);
                  Destroy(iceTor, 6f);
                  break;
              case Elements.Element.WIND:
                  GameObject windTor = Instantiate(circularSlash[2],tempPos, temp);
                  Destroy(windTor, 6f);
                  break;
              case Elements.Element.ELECTRICITY:
                  GameObject elecTor = Instantiate(circularSlash[3], tempPos, temp);
                  Destroy(elecTor, 6f);
                  break;
          }

          float3 tempPos = new float3(swordTip.transform.position.x, swordTip.transform.position.y + 1f, swordTip.transform.position.z);

          switch (weaponData.element)
          {
              case Elements.Element.FIRE:
                  GameObject fireTor = Instantiate(tripleSlash[0], tempPos, Quaternion.identity);
                  Destroy(fireTor, 6f);
                  break;
              case Elements.Element.ICE:
                  GameObject iceTor = Instantiate(tripleSlash[1], tempPos, Quaternion.identity);
                  Destroy(iceTor, 6f);
                  break;
              case Elements.Element.WIND:
                  GameObject windTor = Instantiate(tripleSlash[2], tempPos, Quaternion.identity);
                  Destroy(windTor, 6f);
                  break;
              case Elements.Element.ELECTRICITY:
                  GameObject elecTor = Instantiate(tripleSlash[3], tempPos, Quaternion.identity);
                  Destroy(elecTor, 6f);
                  break;
          }
        */
    }

    public void NormalSlash()
    {
        if (player.nbAttack != 0)
        {

            previousNbAttack = lastNbAttack;
            lastNbAttack = player.nbAttack;

        }
        switch (weaponData.element)
        {
            case Elements.Element.FIRE:
                if (player.nbAttack == 1)
                {
                    nombreattack = 1;


                }
                if (player.nbAttack == 2)
                {
                    slashTime2 = 1f;

                    simpleSlash2[0].SetActive(true);
                    nombreattack = 2;
                }
                break;
            case Elements.Element.ICE:
                if (player.nbAttack == 1)
                {
                    nombreattack = 1;

                }
                if (player.nbAttack == 2)
                {
                    slashTime2 = 1f;
                    nombreattack = 2;

                    simpleSlash2[1].SetActive(true);
                }
                break;
            case Elements.Element.WIND:
                if (player.nbAttack == 1)
                {
                    nombreattack = 1;

                }
                if (player.nbAttack == 2)
                {
                    slashTime2 = 1f;
                    nombreattack = 2;

                    simpleSlash2[3].SetActive(true);
                }
                break;
            case Elements.Element.ELECTRICITY:
                if (player.nbAttack == 1)
                {
                    nombreattack = 1;

                }
                if (player.nbAttack == 2)
                {
                    slashTime2 = 1f;
                    nombreattack = 2;

                    simpleSlash2[2].SetActive(true);
                }
                break;

        }



    }
}

