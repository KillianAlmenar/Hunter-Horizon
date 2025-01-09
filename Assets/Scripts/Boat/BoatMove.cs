using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMove : MonoBehaviour
{
    [SerializeField] Transform boat;
    [SerializeField] float frontMove = 0;
    [SerializeField] float sideMove = 0;

    [SerializeField] AnimationCurve curveFront;
    [SerializeField] AnimationCurve curveSide;
    float time = 0;

    public float yRotation = 0;
    public float multiplicator = 1;

    public float AddIncline = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime;
        Quaternion temp = Quaternion.Euler(curveFront.Evaluate(time) * frontMove * multiplicator, yRotation, curveSide.Evaluate(time) * sideMove * multiplicator + AddIncline);

        transform.rotation = temp;


    }
}
