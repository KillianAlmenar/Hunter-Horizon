using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;
using Cinemachine;
using TMPro;
using System.Linq;

public class AttackScript : MonoBehaviour
{

    public PlayerAction inputActions;
    public GameObject smokeCloud;
    public GameObject bullet;
    public GameObject blade;
    public GameObject playerObj;
    public GameObject[] bigShot;
    public ManyShotsInstantiator manyShots;
    StuffScript stuffData;
    bool isCrit;
    bool canAttack;
    public bool isAttacking;
    int critStat;
    public int equipedWeapon;
    float comboTime;
    float comboShotsTimer2;
    float recoveryTimer;
    public float recoverTime;
    public float comboTimeLimit;
    GetMember gm;
    bool comboStarted;
    int comboNumber;
    public int comboNumberLimit;
    public float comboValue;
    // Start is called before the first frame update
    float attacktime;

    public HotBar hotbar;
    public TargetLock targetLock;

    public Vector3 lockTrajectory;

    public List<WeaponsData> weaponData2 = new List<WeaponsData>();//recuperer dans ressources
    public List<EquipmentData> weaponData = new List<EquipmentData>().ToList();//recuperer dans ressources
    public EquipmentData equipedData = new EquipmentData();//recuperer dans ressources

    [SerializeField] BladeScript bladeScript;
    //Debug a commenter
    // public TextMeshProUGUI equippedText;

    [SerializeField] PlayerSoundPlayer soundPlayer;

    void Start()
    {
        equipedWeapon = 0;
        weaponData2 = Resources.Load<WeaponDataBase>("WeaponDatabase").weapons.ToList();
        weaponData = Resources.Load<EquipmentDataBase>("EquipmentDataBase").equipments.ToList();

        stuffData = playerObj.GetComponent<StuffScript>();
        inputActions = new PlayerAction();
        inputActions.Player.Slash.Enable();
        inputActions.Player.Fire.Enable();
        inputActions.Player.ChangeWeapon.Enable();
        inputActions.Player.RangeFinisher.Enable();
        critStat=1;
        canAttack=true;
        comboStarted = false;
        recoveryTimer = 0f;
        comboTime = 0f;
        comboNumber = 0;
        attacktime = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        equipedData = weaponData[equipedWeapon];
        if (targetLock.isTargeting) {

            lockTrajectory = targetLock.currentTarget.position - gameObject.transform.position;

        }
        else
        {
            lockTrajectory =Camera.main.transform.forward;

        }
        if (comboTime < 0f)
        {
            comboTime = 0f;
            comboNumber = 0;
        }
        else
        {
            comboTime -= Time.deltaTime;
        }
        if(comboShotsTimer2>0f)
        {
            comboShotsTimer2 -= Time.deltaTime; 
        }
       
 
        foreach (EquipmentData objet in weaponData.ToList())
        {
            if(objet.equipmentType != EquipmentType.WEAPON)
            {
                weaponData.Remove(objet);
            }
        }

            blade.GetComponent<BladeScript>().weaponData = stuffData.equippedStuffData[4];


        if (inputActions.Player.ChangeWeapon.WasPressedThisFrame())
        {
            

            if (equipedWeapon != 0)
            {
                equipedWeapon = 0;
            }
            else
            {
                equipedWeapon = 1;

            }
            if (hotbar.SlotSelected != 0)
            {
                hotbar.SlotSelected = 0;
            }
            else
            {
                hotbar.SlotSelected = 1;

            }
        }
        
        if(equipedWeapon<0)
        {
            equipedWeapon = 0;
        }
        else if(equipedWeapon>1)
        {
            equipedWeapon = 1;
        }

        if (((inputActions.Player.Slash.IsPressed() || inputActions.Player.Fire.IsPressed()) && canAttack)&&GameManager.instance.currentGameState != GameManager.GameState.LOBBY)
        {
            canAttack=false;
            // Créer un rayon depuis le centre de l'écran
            Ray ray = new Ray(transform.position,transform.forward);

            // Déclarer une variable RaycastHit pour stocker les informations sur la collision
  
            float attackDamage = Random.Range(weaponData[equipedWeapon].attack - weaponData[equipedWeapon].attackRange, weaponData[equipedWeapon].attack + weaponData[equipedWeapon].attackRange);
            Debug.Log(weaponData.Count - 1);

            switch (weaponData[equipedWeapon].weaponType)
            {
                //check si l'arme est mêlée ou distance
                case WeaponType.Range:
                    if (inputActions.Player.Fire.IsPressed())
                    {
                        Debug.Log("shooting");
                        
                        Shoot();
                    }
                    break;
                case WeaponType.Close:
                    if (inputActions.Player.Slash.IsPressed())
                    {

                        Slash();
                    }
                    break;
            }
            
            
        }
        if (inputActions.Player.RangeFinisher.WasPressedThisFrame())
        {
            Debug.Log("touche");

            if (weaponData[equipedWeapon].weaponType == WeaponType.Range)
            {
                if (comboTime > 0f && comboNumber > 0)
                {


                    if (comboNumber == 2)
                    {
                        manyShots.attScript = this;
                        manyShots.LaunchAttack(weaponData[equipedWeapon].element);

                    }
                    else
                    {
                        switch (weaponData[equipedWeapon].element)
                        {
                            case Elements.Element.FIRE:
                                Debug.Log(Camera.main.transform.rotation.y);

                                GameObject fireBigShot = Instantiate(bigShot[0], gameObject.transform.position, Quaternion.Euler(new Vector3(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 180f, Camera.main.transform.eulerAngles.z)));
                                fireBigShot.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10f;
                                fireBigShot.GetComponent<BulletScript>().attackScript = this;

                                fireBigShot.SetActive(true);
                                Destroy( fireBigShot,5f );
                                break;
                            case Elements.Element.ICE:
                                GameObject iceBigShot = Instantiate(bigShot[1], gameObject.transform.position, Quaternion.Euler(new Vector3(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 180f, Camera.main.transform.eulerAngles.z)));
                                iceBigShot.GetComponent<BulletScript>().attackScript = this;
                                iceBigShot.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10f;
                                iceBigShot.SetActive(true);
                                Destroy(iceBigShot, 5f);

                                break;
                            case Elements.Element.ELECTRICITY:
                                GameObject elecBigShot = Instantiate(bigShot[2], gameObject.transform.position, Quaternion.Euler(new Vector3(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 180f, Camera.main.transform.eulerAngles.z)));
                                elecBigShot.GetComponent<BulletScript>().attackScript = this;
                                elecBigShot.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10f;
                                elecBigShot.SetActive(true);
                                Destroy(elecBigShot, 5f);

                                break;
                            case Elements.Element.WIND:
                                GameObject windBigShot = Instantiate(bigShot[3], gameObject.transform.position, Quaternion.Euler(new Vector3(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y + 180f, Camera.main.transform.eulerAngles.z)));
                                windBigShot.GetComponent<BulletScript>().attackScript = this;
                                windBigShot.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 10f;
                                windBigShot.SetActive(true);
                                Destroy(windBigShot, 5f);

                                break;
                        }
                    }
                    comboShotsTimer2 = 5f;
                }
            }
        }
        if (comboStarted)
        {
            comboTime -= Time.deltaTime;
        }

        if (!canAttack)
        {
            recoveryTimer += Time.deltaTime;
        }
        if (recoveryTimer > recoverTime)
        {
            canAttack = true;
            recoveryTimer = 0;
        }

        attacktime -= Time.deltaTime;
        if(attacktime<=0f)
        {
            
            isAttacking = false;
            attacktime = 0f;
        }
        

    }
    
    void Shoot()
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.LOBBY)
        {
            soundPlayer.PlayGunShot();

            GameObject cloneCloud =Instantiate(smokeCloud, gameObject.transform.position, Quaternion.Euler(new Vector3(Camera.main.transform.rotation.x + 90, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z)));
            Destroy(cloneCloud, 3f);
            GameObject clone = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(new Vector3(Camera.main.transform.rotation.x + 90, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z)));
            clone.GetComponent<Rigidbody>().velocity = lockTrajectory.normalized * 25f;
            clone.GetComponent<BulletScript>().weaponData = weaponData[equipedWeapon];
            clone.GetComponent<BulletScript>().attackScript = this;
            Destroy(clone, 2f);
            isAttacking = true;
            
        }
        
        comboNumber++;

        if ( comboNumber > 2)
            {
                comboNumber= 2;
            }

        comboTime = 3f;
  
    }
    void Slash()
    {
        if (weaponData[equipedWeapon].weaponType != WeaponType.Range)
        {
            Debug.Log("SLASHING!");

            bladeScript.NormalSlash();
            isAttacking = true;
            attacktime = 1f;
        }

    }

    void Attack()
    {

        if (weaponData[equipedWeapon].weaponType == WeaponType.Range)
        {
            GameObject clone = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(new Vector3(Camera.main.transform.rotation.x + 90, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z)));
            clone.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * 50f;
            clone.GetComponent<BulletScript>().weaponData = weaponData[equipedWeapon];
            Debug.Log("PAN!");
        }
        else if (weaponData[equipedWeapon].weaponType == WeaponType.Range)
        {
            isAttacking = true;
            attacktime = 1f;
        }
    }


    float ComboManager(RaycastHit hit, float attackDamage)
    {
        attackDamage= attackDamage * (comboValue*comboNumber);
        comboNumber++;
        comboTime = 0;
        DamagePopUp.current.CreatePopUp(hit.transform.position, "COMBO x" + comboNumber.ToString(), Color.white, false);

        return attackDamage;

    }

    public float TypeTable(Collider target, EquipmentData weapon, float damage)
    {

        switch (weapon.element)
        {
            case (Elements.Element.FIRE):
                if(target.GetComponent<Enemy>().element == Elements.Element.ICE)
                {
                    damage *= 1.25f;
                }
                if (target.GetComponent<Enemy>().element == Elements.Element.WIND)
                {
                    damage *= 0.25f;
                }
                break;
            case (Elements.Element.ICE):
                if (target.GetComponent<Enemy>().element == Elements.Element.ELECTRICITY)
                {
                    damage *= 1.25f;
                }
                if (target.GetComponent<Enemy>().element == Elements.Element.FIRE)
                {
                    damage *= 0.25f;
                }
                break;
            case (Elements.Element.ELECTRICITY):
                if (target.GetComponent<Enemy>().element == Elements.Element.WIND)
                {
                    damage *= 1.25f;
                }
                if (target.GetComponent<Enemy>().element == Elements.Element.ICE)
                {
                    damage *= 0.25f;
                }
                break;
            case (Elements.Element.WIND):
                if (target.GetComponent<Enemy>().element == Elements.Element.FIRE)
                {
                    damage *= 1.25f;
                }
                if (target.GetComponent<Enemy>().element == Elements.Element.ELECTRICITY)
                {
                    damage *= 0.25f;
                }
                break;
        }
        return damage;
    }
}



