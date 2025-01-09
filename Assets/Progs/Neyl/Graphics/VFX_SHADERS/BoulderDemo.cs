using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoulderDemo : MonoBehaviour
{

    public float circleSize;
    float tigre;
    [SerializeField] Transform startPoint;
    // Start is called before the first frame update
    void Start()
    {
        tigre = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tigre += Time.deltaTime;
        transform.position = new Vector3(circleSize * Mathf.Cos(tigre) + startPoint.position.x, 0, circleSize * Mathf.Sin(tigre) + startPoint.position.z);
    }
}
