using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    [SerializeField] Image image;
    public List<Sprite> sprite;
    private float initialSpeed = 50;
    private void Update()
    {
        image.sprite = sprite[IDObject.CurrentItems];
        Vector3 newPos = gameObject.transform.position;
        newPos.y += Time.deltaTime * initialSpeed;
        gameObject.transform.position = newPos;
        image.color = new Color(1, 1, 1, image.color.a - Time.deltaTime);
        initialSpeed -= Time.deltaTime * 10;

        if(image.color.a < 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}