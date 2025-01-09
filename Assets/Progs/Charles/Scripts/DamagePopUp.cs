using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public AudioListener listener;
    public static DamagePopUp current;

    // Update is called once per frame
    GameObject prefab;
    public GameObject prefabNormal;
    public GameObject prefabCrit;
    private void Awake()
    {
        AudioListener listener = GetComponent<AudioListener>();
 
        current = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10)) 
        { 
            //CreatePopUp(this.gameObject.transform.position,Random.Range(0,1000).ToString(),Color.white,false);
        }
    }

    public void CreatePopUp(Vector3 position, string text, Color color, bool isCrit)
    {   
        Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
        if(isCrit)
        {
            prefab = prefabCrit;
        }
        else
        {
            prefab = prefabNormal;
        }
        GameObject popup = Instantiate(prefab, position + randomness, Quaternion.identity);

        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        temp.color = color;
        temp.text = text;

        Destroy(popup,1f);
    }
}
