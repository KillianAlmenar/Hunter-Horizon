using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    // Start is called before the first frame update

    
    void Start()
    {

        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    // Update is called once per frame
    void Update()
    {

        float forwardValue = Input.GetAxis("Vertical");
        float rotationValue = Input.GetAxis("Horizontal");

        transform.Rotate(0, rotationValue * Time.deltaTime * 180.0f, 0);
        transform.position += transform.forward * forwardValue * Time.deltaTime * 10.0f;

        
    }
}
