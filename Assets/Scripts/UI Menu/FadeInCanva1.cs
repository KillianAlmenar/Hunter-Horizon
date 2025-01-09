using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInCanva : MonoBehaviour
{
    private CanvasGroup canvaGroup;
    [SerializeField] float fadeSpeed = 4;

    bool startCanvaFadeIn;
    void Start()
    {
        startCanvaFadeIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCanvaFadeIn)
        {
            if (canvaGroup.alpha < 1)
            {
                canvaGroup.alpha += fadeSpeed * Time.deltaTime;
            }
            else
            {
                canvaGroup.interactable = true;
                startCanvaFadeIn = false;
            }
        }
    }

    public void CanvaFadeIn(CanvasGroup _canvaGroup)
    {
        canvaGroup = _canvaGroup;
        canvaGroup.gameObject.SetActive(true);
        canvaGroup.interactable = false;
        canvaGroup.alpha = 0;
       startCanvaFadeIn = true;
    }
}
