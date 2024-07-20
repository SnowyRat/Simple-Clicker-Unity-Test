using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve criticalScaleCurve; // AnimationCurve for critical hits
    public AnimationCurve heightCurve;
    private TextMeshProUGUI tmp;
    private float time = 0f;
    private Vector3 origin;
    private bool criticalHit;

    void Start()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    public void SetCriticalHit(bool isCriticalHit)
    {
        criticalHit = isCriticalHit;
        if (isCriticalHit)
        {
            // Apply critical hit scaleCurv
            transform.localScale = Vector3.one * criticalScaleCurve.Evaluate(time);
        }
        else
        {
            // Apply regular scaleCurve
            transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        }
    }

    void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        if(criticalHit){
            transform.localScale = Vector3.one * criticalScaleCurve.Evaluate(time);
        }
        else{
            transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        }
        transform.position = origin + new Vector3(0, heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
