using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Compteur : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unite;
    [SerializeField] TextMeshProUGUI dizaine;
    [SerializeField] TextMeshProUGUI centaine;

    [SerializeField] BoatController boat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        float boatSpeed = boat.GetSpeed();
        boatSpeed = Mathf.Abs(boatSpeed);

       
        UpdateText(unite, 1+ (boatSpeed % 10));
        UpdateText(dizaine,1+ ((boatSpeed / 10) % 10));
        UpdateText(centaine,1+ ((int)boatSpeed / 100));
    }

    void UpdateText(TextMeshProUGUI text, float value)
    {
        Vector3 temp = text.transform.localPosition;
        temp.y = value * text.fontSize * -1;
        text.transform.localPosition = temp;
    }
}
