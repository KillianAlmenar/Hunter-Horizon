using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPoPUp : MonoBehaviour
{
    [SerializeField] public GameObject Frame;
    [SerializeField] public GameObject Item;
    [SerializeField] public GameObject Name;
    [SerializeField] public GameObject Description;
    private Color color;
    bool finishSpawned;
    void Start()
    {
        finishSpawned = false;
        color = new Color(255, 255, 255, 0);
        Frame.GetComponent<Image>().color = color;
        Item.GetComponent<Image>().color = color;
        Name.GetComponent<TextMeshProUGUI>().color = color;
        Description.GetComponent<TextMeshProUGUI>().color = color;
    }
    void Update()
    {

        if(color.a < 1 && !finishSpawned)
        {
            Vector3 pos = transform.position;
            pos.x -= Time.deltaTime * 10;
            transform.position = pos;  
            color.a += Time.deltaTime;
            Frame.GetComponent<Image>().color = color;
            Item.GetComponent<Image>().color = color;
            Name.GetComponent<TextMeshProUGUI>().color = color;
            Description.GetComponent<TextMeshProUGUI>().color = color;
        }
        else
        {
            finishSpawned = true;
        }

        if(finishSpawned)
        {
            StartCoroutine(FadePoUp());
        }
    }
    private IEnumerator FadePoUp()
    {
        yield return new WaitForSeconds(1f); 

        Vector3 pos = transform.position;
        pos.y -= Time.deltaTime * 20;
        transform.position = pos;
        color.a -= Time.deltaTime;
        Frame.GetComponent<Image>().color = color;
        Item.GetComponent<Image>().color = color;
        Name.GetComponent<TextMeshProUGUI>().color = color;
        Description.GetComponent<TextMeshProUGUI>().color = color;
        if(color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
