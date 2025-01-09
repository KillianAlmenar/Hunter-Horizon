using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = cam.transform.forward;   
    }
}
