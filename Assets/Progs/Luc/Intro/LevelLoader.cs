using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    static public LevelLoader instance;

    [SerializeField] GameObject canva;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI textPourcent;

    [SerializeField] AnimationCurve curve;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    public void LoadLevelIntro(float waitTime)
    {
        StartCoroutine(LoadIntroCoroutine("Lobby", waitTime));
    }

    IEnumerator LoadIntroCoroutine(string sceneName, float wait = 1)
    {
        AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName);
        handle.allowSceneActivation = false;

        float tempTimer = wait;

        while (tempTimer > 0)
        {
            tempTimer -= Time.deltaTime;
            yield return null;
        }

        canva.gameObject.SetActive(true);

        float timer = 0.0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            SetBlackscreenAlpha(curve.Evaluate(timer));
            slider.value = Mathf.Clamp01(handle.progress / 0.9f);
            textPourcent.text = ((int)(slider.value * 100)).ToString() + " %";
            yield return null;
        }

        SetBlackscreenAlpha(1.0f);
        slider.value = Mathf.Clamp01(handle.progress / 0.9f);

        yield return new WaitWhile(() => handle.progress < 0.9f);


        handle.allowSceneActivation = true;
        yield return null;

        timer = 1;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            SetBlackscreenAlpha(curve.Evaluate(timer));
            yield return null;
        }

        SetBlackscreenAlpha(0.0f);

        canva.gameObject.SetActive(false);
    }


    IEnumerator LoadLevelCoroutine(string sceneName)
    {
        AsyncOperation handle = SceneManager.LoadSceneAsync(sceneName);
        canva.gameObject.SetActive(true);

        handle.allowSceneActivation = false;

        float timer = 0.0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            SetBlackscreenAlpha(curve.Evaluate(timer));
            slider.value = Mathf.Clamp01(handle.progress / 0.9f);
            textPourcent.text = ((int)(slider.value * 100)).ToString() + " %"; 
            yield return null;
        }

        SetBlackscreenAlpha(1.0f);
        slider.value = Mathf.Clamp01(handle.progress / 0.9f);

        yield return new WaitWhile(() => handle.progress < 0.9f);

        handle.allowSceneActivation = true;
        yield return null;

        timer = 1;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            SetBlackscreenAlpha(curve.Evaluate(timer));
            yield return null;
        }

        SetBlackscreenAlpha(0.0f);

        canva.gameObject.SetActive(false);
    }

    void SetBlackscreenAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
}
