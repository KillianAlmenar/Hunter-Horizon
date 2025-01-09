using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualitySetting : MonoBehaviour
{
    struct ShadowSetting
    {
        public int cascade;
        public int distance;
    }
    ShadowSetting[] shadowSet;

    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] TMP_Dropdown shadowDropdown;
    [SerializeField] TMP_Dropdown textureDropdown;
    [SerializeField] Toggle vsyncToggle;


    private void Start()
    {
        LoadShadowSettings();

        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 2);
        shadowDropdown.value = PlayerPrefs.GetInt("ShadowSetting", 3);
        textureDropdown.value = PlayerPrefs.GetInt("TextureSetting", 0);

        vsyncToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Vsync", 0));
    }

    private void LoadShadowSettings()
    {
        shadowSet = new ShadowSetting[3];

        shadowSet[0].cascade = 0;
        shadowSet[0].distance = 0;

        shadowSet[1].cascade = 2;
        shadowSet[1].distance = 75;

        shadowSet[2].cascade = 4;
        shadowSet[2].distance = 500;
    }

    public void SetQuality(TMP_Dropdown _qualityValue)
    {
        QualitySettings.SetQualityLevel(_qualityValue.value);
        PlayerPrefs.SetInt("Quality", _qualityValue.value);

        PlayerPrefs.Save();
    }

    public void ShadowsSettings(TMP_Dropdown _shadowValue)
    {
        QualitySettings.shadowCascades = shadowSet[_shadowValue.value].cascade;
        QualitySettings.shadowDistance = shadowSet[_shadowValue.value].distance;

        PlayerPrefs.SetInt("ShadowCascades", shadowSet[_shadowValue.value].cascade);
        PlayerPrefs.SetInt("ShadowDistance", shadowSet[_shadowValue.value].distance);

        PlayerPrefs.SetInt("ShadowSetting", _shadowValue.value);
        PlayerPrefs.Save();

    }

    public void TexturesSettings(TMP_Dropdown _textureValue)
    {
        QualitySettings.globalTextureMipmapLimit = _textureValue.value;

        PlayerPrefs.SetInt("TextureMipmapLimit", _textureValue.value);
        PlayerPrefs.SetInt("TextureSetting", _textureValue.value);
        PlayerPrefs.Save();
    }

    public void vsync(Toggle _toggle)
    {
       QualitySettings.vSyncCount = Convert.ToInt32(_toggle.isOn);

        PlayerPrefs.SetInt("Vsync", Convert.ToInt32(_toggle.isOn));
        PlayerPrefs.Save();
    }


}
