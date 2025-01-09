using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private MeshRenderer[] m_Renderer;

    [SerializeField]
    private bool changeColor = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponentsInChildren<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (changeColor)
        {
            float metallic = Random.Range(0f, 1f) ;
            float smoothness = Random.Range(0f, 1f);
            float topLine = Random.Range(0f, 1f);
            float bottomLine = Random.Range(0f, 1f);

            Color baseColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            Color topColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            Color bottomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

            foreach (MeshRenderer renderer in m_Renderer)
            {
                Material mat = renderer.sharedMaterial;
                
                mat.SetFloat("_Metallic", metallic);
                mat.SetFloat("_Smoothness", smoothness);
                mat.SetFloat("_TopLine", topLine);
                mat.SetFloat("_BottomLine", bottomLine);

                mat.SetColor("_BaseColor", baseColor * Random.Range(0f, 10f));

                mat.SetColor("_TopColor", topColor * Random.Range(0f, 10f));

                mat.SetColor("_BottomColor", bottomColor * Random.Range(0f, 10f));

            }

            changeColor = false;
        }
    }
}
