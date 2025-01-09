using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    static public List<Item> rewardData = new List<Item>();
    static public bool questEnd = false;
    [SerializeField] GameObject rewardCanvas;

    public void showReward()
    {
        if (questEnd)
        {
            rewardCanvas.SetActive(true);
        }
    }

    public void Update()
    {
        showReward();
    }
}
