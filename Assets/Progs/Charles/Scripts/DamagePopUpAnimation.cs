using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{


    public AnimationCurve opacityCurve;
    public AnimationCurve heightCurve;

    private TextMeshProUGUI tmp;

    private float time = 0;

    private Vector3 origin;
    private Vector3 baseScale;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
        baseScale = transform.localScale;
    }

    private void Update()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacityCurve.Evaluate(time));
        transform.position = origin + new Vector3(0,heightCurve.Evaluate(time), 0);

        time += Time.deltaTime;
    }

}
