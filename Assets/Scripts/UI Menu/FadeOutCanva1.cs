using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutCanva : MonoBehaviour
{
    private CanvasGroup canvaGroup;
    [SerializeField] float fadeSpeed;

    bool startCanvaFadeOut;
    void Start()
    {
        startCanvaFadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startCanvaFadeOut)
        {
            if (canvaGroup.alpha > 0)
            {
                canvaGroup.alpha -= fadeSpeed * Time.deltaTime;
            }
            else
            {
                canvaGroup.gameObject.SetActive(false);
                startCanvaFadeOut = false;
            }
        }
    }

    public void CanvaFadeOut(CanvasGroup _canvaGroup)
    {
        canvaGroup = _canvaGroup;
        canvaGroup.interactable = false;
        startCanvaFadeOut = true;
    }
}
