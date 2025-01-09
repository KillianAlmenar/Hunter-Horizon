using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour
{
    private RawImage image;
    private Vector3 imageVect;
    [SerializeField] List<Sprite> sprites;
    public int IDSprite;
    private void Start()
    {
        image = GetComponent<RawImage>();
        image.texture = sprites[IDSprite].texture;
    }
    private void Update()
    {
        image.color = new Color(255, 255, 255, image.color.a - Time.deltaTime);
        imageVect = image.gameObject.transform.position;
        imageVect.y += Time.deltaTime * 50;
        image.gameObject.transform.position = imageVect;

        if(image.color.a < 0)
        {
            Destroy(gameObject);
        }
    }
}

