using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sensibility : MonoBehaviour
{
    [SerializeField]
    Slider controllerSlider;
    [SerializeField]
    Slider mouseSlider;


    public void SetControllerSensibility()
    {
       GameManager.instance.controllerSensibility = controllerSlider.value;
    }
    public void SetMouseSensibility()
    {
        GameManager.instance.mouseSensibility = mouseSlider.value;
    }

}
