using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInitialisation : MonoBehaviour
{

    [SerializeField] Button playButton;
    //[SerializeField] Button continueButton;
    [SerializeField] GameObject HUD;

    private float timerdemerde = 0.5f;

    private void Awake()
    {

    }
    void Start()
    {


        //Screen size
        Screen.SetResolution(PlayerPrefs.GetInt("ScreenWidth", 1920), PlayerPrefs.GetInt("ScreenHeight", 1080), Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen", 1)));

        //QualitySettings
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 2));

        QualitySettings.shadowCascades = PlayerPrefs.GetInt("ShadowCascades", 4);
        QualitySettings.shadowDistance = PlayerPrefs.GetInt("ShadowDistance", 500);

        QualitySettings.globalTextureMipmapLimit = PlayerPrefs.GetInt("TextureMipmapLimit", 0);

        QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync", 0);

        //Sounds
        AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1.0f);
        SoundManager.instance.SoundsVolume = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
        SoundManager.instance.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        //AudioSettings.speakerMode = (AudioSpeakerMode)PlayerPrefs.GetInt("SpacialMode", 2);

        //Debug.Log(AudioSettings.GetConfiguration().speakerMode);

        AudioConfiguration temp = AudioSettings.GetConfiguration();
        temp.speakerMode = (AudioSpeakerMode)PlayerPrefs.GetInt("SpacialMode", 2);
        temp.numRealVoices = 32;
        temp.numVirtualVoices = 512;
        temp.dspBufferSize = 1024;
        //AudioSettings.Reset(temp);

        //Debug.Log(AudioSettings.GetConfiguration().speakerMode);

        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(false);
        //playButton.onClick.AddListener(() => GameManager.instance.SwitchToLobby());

        //playButton.Select();

        if (GameManager.instance.IsThereAnyProgress())
        {
            //continueButton.interactable = true;
            //continueButton.onClick.AddListener(() => GameManager.instance.SwitchToLobby());
        }
        else
        {
            //Destroy(continueButton.gameObject);
        }

        if (GameManager.instance.currentGameState == GameManager.GameState.LOBBY)
        {
            Destroy(gameObject);
            player.GetComponent<ThirdPersonPlayer>().SetPlayerInput(true);

            HUD.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
    }

    private void Update()
    {
        if (timerdemerde >= 0)
        {
            timerdemerde -= Time.deltaTime;
            playButton.Select();
        }
        
    }
}
