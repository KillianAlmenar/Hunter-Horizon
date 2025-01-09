using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class collectText : MonoBehaviour
{
    [SerializeField] Image image;
    public static Image imageClone;
    public List<Sprite> sprite;
    private void Start()
    {
        image.sprite = sprite[IDObject.CurrentItems];
    }
}
