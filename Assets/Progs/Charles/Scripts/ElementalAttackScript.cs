using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class ElementalAttackScript : MonoBehaviour
{

    public enum Status{
        NONE,
        BURNT,
        WINDED,
        FROZEN,
        SHOCKED

    }
    public enum ComboStatus
    {
        NONE,
        BLIZZARD,
        FIRESTORM,
        FUSION,
        SPARKLING,
        STORM,
        SUPRACONDUCTION

    }
    public Color elemColor;

    public Status actualStatus = Status.NONE;
    public ComboStatus actualCombosStatus = ComboStatus.NONE;
    public float timer;

    public EquipmentData weaponData;


    float effectTime =5f;

     bool isFrozen;
     bool isWindy;
     bool isBurnt;
     bool isElectrified;
    bool isStunned;
    


    public float elapsed = 0f;





    AttackEffectScript newStack;
    List<AttackEffectScript> elementalStacks;
    
    // Start is called before the first frame update
    void Start()
    {

        newStack = new AttackEffectScript();
        elementalStacks = new List<AttackEffectScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
        EffectRunning();
        if(elapsed > 0f)
        {
            elapsed -= Time.deltaTime;
        }
        if(elapsed<=0f)
        {
            if (actualStatus == Status.WINDED)
            {
                GetComponent<EnemyStatScript>().defStat = GetComponent<EnemyStatScript>().defStatMax;
            }

            elapsed = 0f;
            actualStatus = Status.NONE;
            actualCombosStatus = ComboStatus.NONE;
        }
        if(actualStatus!=Status.NONE)
        {
            //Debug.Log("affected");

        }

    }



    public void ElementalHit(Collider collider, Elements.Element elements)
    {
        effectTime = 5f;
        newStack.weaponData = weaponData;
        switch (elements)
        {
            case Elements.Element.ICE:
                actualStatus = Status.FROZEN;
                newStack.actualStatus = Status.FROZEN;
                newStack.actualCombosStatus = ComboStatus.NONE;
                newStack.target = collider.gameObject;

                newStack.SetTarget(collider.gameObject);
                newStack.timer= 0f;
                elementalStacks.Add(newStack);
                timer = 4;
                break;
            case Elements.Element.FIRE:
                actualStatus = Status.BURNT;
                newStack.actualStatus = Status.BURNT;
                newStack.actualCombosStatus = ComboStatus.NONE;
                newStack.target = collider.gameObject;
                newStack.SetTarget(collider.gameObject);
                newStack.timer = 0f;

                elementalStacks.Add(newStack);
                timer = 0;
                break;
            case Elements.Element.ELECTRICITY:
                actualStatus = Status.SHOCKED;
                newStack.actualStatus = Status.SHOCKED;
                newStack.actualCombosStatus = ComboStatus.NONE;
                newStack.target = collider.gameObject;

                newStack.SetTarget(collider.gameObject);
                newStack.timer = 0f;

                elementalStacks.Add(newStack);
                timer = 0;
                break;
            case Elements.Element.WIND:
                actualStatus = Status.WINDED;
                newStack.actualStatus = Status.WINDED;
                newStack.actualCombosStatus = ComboStatus.NONE;
                newStack.tempDef = collider.GetComponent<EnemyStatScript>().defStatMax;
                newStack.target = collider.gameObject;

                newStack.SetTarget(collider.gameObject);
                newStack.timer = 0f;

                elementalStacks.Add(newStack);
                timer = 0; ;
                break;

        }
        
    }
    public void ComboHit(Collider collider, ElementalAttackScript.ComboStatus combo)
    {
        effectTime = 5f;

        switch (combo)
        {
            case ComboStatus.BLIZZARD:
                actualStatus = Status.NONE;
                newStack.actualStatus = Status.NONE;

                newStack.actualCombosStatus = ComboStatus.BLIZZARD;
                newStack.SetTarget(collider.gameObject);
                newStack.timer = 0f;
                elementalStacks.Add(newStack);

                timer = 0;
                break;
            case ComboStatus.FIRESTORM:
                actualStatus = Status.NONE;
                newStack.actualStatus = Status.NONE;
                newStack.actualCombosStatus = ComboStatus.FIRESTORM;
                newStack.SetTarget(collider.gameObject);
                newStack.timer = 0f;
                elementalStacks.Add(newStack);

                timer = 0;
                break;
            case ComboStatus.FUSION:
                actualStatus = Status.NONE;
                newStack.actualStatus = Status.NONE;
                newStack.actualCombosStatus = ComboStatus.FUSION;
                newStack.SetTarget(collider.gameObject);
                newStack.timer= 0f;
                elementalStacks.Add(newStack);

                timer = 0;
                break;
            case ComboStatus.SUPRACONDUCTION:
                actualStatus = Status.NONE;
                newStack.actualStatus = Status.NONE; 
                newStack.actualCombosStatus = ComboStatus.SUPRACONDUCTION;
                newStack.SetTarget(collider.gameObject);
                newStack.timer= 0f;
                elementalStacks.Add(newStack);

                timer = 0;
                break;
            case ComboStatus.STORM:
                actualStatus = Status.NONE;
                newStack.actualStatus = Status.NONE; 
                newStack.actualCombosStatus = ComboStatus.STORM;
                newStack.SetTarget(collider.gameObject);
                newStack.timer= 0f;
                elementalStacks.Add(newStack);

                timer = 0;
                break;
            case ComboStatus.SPARKLING:
                actualStatus = Status.NONE;
                newStack.actualStatus = Status.NONE;
                newStack.actualCombosStatus = ComboStatus.SPARKLING;
                newStack.SetTarget(collider.gameObject);
                newStack.timer= 0f;

                elementalStacks.Add(newStack);

                timer = 0;
                break;


        }

    }


    public void EffectRunning()
    {
        if (elementalStacks != null)
        {
            foreach (AttackEffectScript objet in elementalStacks.ToList())
            {
                
                    objet.EffectsUpdate();
                    if ((objet.timer > 5f && objet.weaponData.talent != TalentsScript.WeaponTalents.EternalSuffering)|| (objet.timer > 10f && objet.weaponData.talent == TalentsScript.WeaponTalents.EternalSuffering))
                    {
                        elementalStacks.Remove(objet);

                    }
                
            }
        }
    }


    
}
