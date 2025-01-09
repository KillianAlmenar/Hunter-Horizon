using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldsShop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI golds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        golds.text = "Golds : " + Inventory.instance.golds;
    }
}
