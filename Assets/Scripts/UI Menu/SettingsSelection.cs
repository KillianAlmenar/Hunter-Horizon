using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSelection : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Canvas[] canvas;


    [SerializeField] FadeOutCanva fadeOut;
    [SerializeField] FadeInCanva fadeIn;

    int actualMenu = 0;

    // Start is called before the first frame update
    void Start()
    {

        
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; 
            buttons[i].onClick.AddListener(() => OnButtonClick(buttonIndex));
        }

      
        for (int i = 1; i < canvas.Length; i++)
        {
            canvas[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    // Fonction appelée lorsqu'un bouton est cliqué
    void OnButtonClick(int buttonIndex)
    {
        if(actualMenu != buttonIndex)
        {
            for (int i = 0; i < canvas.Length; i++)
            {
                if (canvas[i].gameObject.activeInHierarchy)
                    fadeOut.CanvaFadeOut(canvas[i].gameObject.GetComponent<CanvasGroup>());
            }
            fadeIn.CanvaFadeIn(canvas[buttonIndex].gameObject.GetComponent<CanvasGroup>());
            actualMenu = buttonIndex;
        }
    }
}
