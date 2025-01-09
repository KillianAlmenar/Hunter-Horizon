using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI volumeText;

    [SerializeField] private Slider sliderMusic;
    [SerializeField] private TextMeshProUGUI volumeMusicText;

    [SerializeField] private Slider sliderSounds;
    [SerializeField] private TextMeshProUGUI volumeSoundsText;

    [SerializeField] private TMP_Dropdown spacialDropDown;

    int[] spacialSetting = { 1, 2, 6 };

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("GlobalVolume", 1.0f);
        volumeText.text = Convert.ToInt32(slider.value * 100).ToString("0");

        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        volumeMusicText.text = Convert.ToInt32(sliderMusic.value * 100).ToString("0");

        sliderSounds.value = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
        volumeSoundsText.text = Convert.ToInt32(sliderSounds.value * 100).ToString("0");

        spacialDropDown.value = PlayerPrefs.GetInt("SpacialSetting", 1);

        slider.onValueChanged.AddListener((value) =>
        {
            AudioListener.volume = value;
            volumeText.text = Convert.ToInt32(value *= 100).ToString("0");

            PlayerPrefs.SetFloat("GlobalVolume", AudioListener.volume);
            PlayerPrefs.Save();
        });


        sliderMusic.onValueChanged.AddListener((value) =>
        {
            SoundManager.instance.MusicVolume = value;
            volumeMusicText.text = Convert.ToInt32(value * 100).ToString("0");

            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        });


        sliderSounds.onValueChanged.AddListener((value) =>
        {
            SoundManager.instance.SoundsVolume = value;
            volumeSoundsText.text = Convert.ToInt32(sliderSounds.value * 100).ToString("0");

            PlayerPrefs.SetFloat("SoundsVolume", value);
            PlayerPrefs.Save();
        });


        AudioConfiguration config = AudioSettings.GetConfiguration();

        spacialDropDown.onValueChanged.AddListener((value) =>
        {
            
            //config.speakerMode = (AudioSpeakerMode)spacialSetting[value];

            PlayerPrefs.SetInt("SpacialMode", spacialSetting[value]);
            PlayerPrefs.SetInt("SpacialSetting", value);
            PlayerPrefs.Save();
        });

        /*
        // Afficher les informations sur la sortie audio
        Debug.Log("Sample Rate: " + config.sampleRate);
        Debug.Log("Speaker Mode: " + config.speakerMode);
        Debug.Log("Dsp Buffer Size: " + config.dspBufferSize);
        Debug.Log("Number of Real Voices: " + config.numRealVoices);
        Debug.Log("Number of Virtual Voices: " + config.numVirtualVoices);
        */
    }
}
