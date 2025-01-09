using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanarReflection : MonoBehaviour
{
    [SerializeField] private Camera cameraReflection;
    [SerializeField] private RenderTexture reflectionRT;

     private Vector2 resolution;

    [SerializeField] private int reflectionResolution;
    private void Start()
    {
        cameraReflection.fieldOfView = Camera.main.fieldOfView;
    }
    private void LateUpdate()   
    {
        transform.position = new Vector3(Camera.main.transform.position.x, -Camera.main.transform.position.y + transform.position.y , Camera.main.transform.position.z);  
        transform.rotation= Quaternion.Euler(-Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y , 0f);

        resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

        reflectionRT.Release();
        reflectionRT.width = Mathf.RoundToInt(resolution.x) * reflectionResolution / Mathf.RoundToInt(resolution.y);
        reflectionRT.height = reflectionResolution;
        

    }
}