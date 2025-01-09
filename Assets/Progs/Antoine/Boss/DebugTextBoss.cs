using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class DebugTextBoss : MonoBehaviour
{
    [SerializeField] tempBoss boss;
    private TextMeshProUGUI textMeshPro;
    public enum typeToShow
    {
        STATE,
        TIMER,
        DISTANCE,
    };
    public typeToShow type;
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        switch (type)
        {
            case typeToShow.STATE:
                textMeshPro.text = boss.CurrentState.ToString();
                break;
            case typeToShow.TIMER:
                textMeshPro.text = new string(Math.Round(boss.timer, 2) +" : " + Math.Round(boss.idleTimer, 2));
                break;
            case typeToShow.DISTANCE:
                textMeshPro.text = new string(boss.getDistance(boss.player.transform).ToString());
                break;
            default:
                break;
        }
    }
}
