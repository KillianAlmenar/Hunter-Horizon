using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectDetect : MonoBehaviour, ISelectHandler,IDeselectHandler
{
    private bool isSelected = false;

    public bool IsSelected { get => isSelected; set => isSelected = value; }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

}
