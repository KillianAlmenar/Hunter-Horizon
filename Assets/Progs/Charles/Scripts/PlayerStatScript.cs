using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerAtk;
    public int playerDef;
    StuffScript stuff;
    public List<int> elemResistance;
    void Start()
    {
        playerAtk = 1;
        playerDef = 1;
        stuff = GetComponent<StuffScript>();
        for(int i =0;i<5;i++)
        {
            int debut = 0;
            elemResistance.Add(debut);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDef = stuff.equippedStuffData[0].defense + stuff.equippedStuffData[1].defense + stuff.equippedStuffData[2].defense + stuff.equippedStuffData[3].defense;
        playerAtk = stuff.equippedStuffData[0].attack + stuff.equippedStuffData[1].attack + stuff.equippedStuffData[2].attack + stuff.equippedStuffData[3].attack;
        for (int i = 0; i < 5; i++)
        {
            elemResistance[i] = 0;
        }
        for (int j = 0; j < 4; j++)
            {
                switch (stuff.equippedStuffData[j].element)
                {
                    case (Elements.Element.FIRE):
                        elemResistance[(int)Elements.Element.WIND] -= 20;
                        elemResistance[(int)Elements.Element.ICE] += 20;
                        break;
                    case (Elements.Element.ICE):
                        elemResistance[(int)Elements.Element.FIRE] -= 20;
                        elemResistance[(int)Elements.Element.ELECTRICITY] += 20;
                        break;
                    case (Elements.Element.ELECTRICITY):
                        elemResistance[(int)Elements.Element.ICE] -= 20;
                        elemResistance[(int)Elements.Element.WIND] += 20;
                        break;
                    case (Elements.Element.WIND):
                        elemResistance[(int)Elements.Element.ELECTRICITY] -= 20;
                        elemResistance[(int)Elements.Element.FIRE] += 20;
                        break;
                }
            }
        
        
    }
    public float ElemResist(float damage, Elements.Element elem)
    {
        float temp = 0f;

        switch (elem)
        {
            case (Elements.Element.FIRE):
                damage -= (damage/100) * elemResistance[(int)Elements.Element.FIRE];

                break;
            case (Elements.Element.ICE):
                damage -= (damage / 100) * elemResistance[(int)Elements.Element.ICE];

                break;
            case (Elements.Element.ELECTRICITY):
                damage -= (damage / 100) * elemResistance[(int)Elements.Element.ELECTRICITY];

                break;
            case (Elements.Element.WIND):
                damage -= (damage / 100) * elemResistance[(int)Elements.Element.WIND];

                break;
        }

        return temp;
    }

}
