using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanOpenInventory : MonoBehaviour
{
    public void CanOpen()
    {
        Inventory.instance.CanActiveInventory(true);
    }
}
