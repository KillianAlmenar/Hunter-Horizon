using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGroundCrack : MonoBehaviour
{
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] float delay;
    float timer;
    float samplerCurve;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        samplerCurve = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if( timer > delay )
        {
            samplerCurve += Time.deltaTime;
            transform.localScale = new Vector3(scaleCurve.Evaluate(samplerCurve), scaleCurve.Evaluate(samplerCurve), scaleCurve.Evaluate(samplerCurve));
        }

        if(transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}
