using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ScreenResolution
{
    public int ScreenHeight;
    public int ScreenWidth;
}

public class ScreenSettings : MonoBehaviour
{
    public ScreenResolution[] screenResolution = new ScreenResolution[4];
    bool isFullScreen;
    int actualResolution;

    // Start is called before the first frame update
    void Start()
    {

        screenResolution[0].ScreenWidth = 1280;
        screenResolution[0].ScreenHeight = 720;

        screenResolution[1].ScreenWidth = 1920;
        screenResolution[1].ScreenHeight = 1080;

        screenResolution[2].ScreenWidth = 2560;
        screenResolution[2].ScreenHeight = 1040;

        screenResolution[3].ScreenWidth = 3840;
        screenResolution[3].ScreenHeight = 2160;

        actualResolution = PlayerPrefs.GetInt("ActualResolution", 1);
        isFullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen", 1));

        GetComponentInChildren<TMP_Dropdown>().value = actualResolution;
        GetComponentInChildren<Toggle>().isOn = isFullScreen;

        // Initialisation de la résolution au lancement
        SetScreenResolution();
    }

    void SetScreenResolution()
    {
        Screen.SetResolution(screenResolution[actualResolution].ScreenWidth, screenResolution[actualResolution].ScreenHeight, isFullScreen);

        PlayerPrefs.SetInt("ScreenWidth", screenResolution[actualResolution].ScreenWidth);
        PlayerPrefs.SetInt("ScreenHeight", screenResolution[actualResolution].ScreenHeight);
        PlayerPrefs.SetInt("FullScreen", Convert.ToInt32(isFullScreen));
        PlayerPrefs.SetInt("ActualResolution", actualResolution);

        PlayerPrefs.Save();
    }

    public void SetResolution(TMP_Dropdown _qualityValue)
    {
        actualResolution = _qualityValue.value;
        SetScreenResolution();
    }

    public void SetResolution(Toggle toggle)
    {
        isFullScreen = toggle.isOn;
        SetScreenResolution();
    }
}

