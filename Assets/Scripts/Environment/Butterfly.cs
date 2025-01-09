using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Butterfly : MonoBehaviour
{
    private VisualEffect VFX;
    private bool isChanged = false;
    [SerializeField]
    private Texture[] WingTexture;

    private void Start()
    {
        VFX = GetComponent<VisualEffect>();
        if (!DayNightManager.instance.isNight)
        {
            VFX.SetTexture("Base Color Map", WingTexture[0]);

        }
        else if (DayNightManager.instance.isNight)
        {
            VFX.SetTexture("Base Color Map", WingTexture[1]);
        }
    }

    private void Update()
    {

        if (!DayNightManager.instance.isNight && isChanged)
        {
            VFX.SetTexture("Base Color Map", WingTexture[0]);

            isChanged = false;
        }
        else if (DayNightManager.instance.isNight && !isChanged)
        {
            VFX.SetTexture("Base Color Map", WingTexture[1]);
            isChanged = true;
        }
    }
}
