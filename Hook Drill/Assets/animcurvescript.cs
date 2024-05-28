using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animcurvescript : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    IEnumerator currentCoroutine;


    public float mySpeed;

    public float duration = 1f;


    private float timer = 0f;
    IEnumerator myCoroutine()
    {

        while (timer < duration)
        {
            float normalizedTime = timer / duration;
            float curveValue = curve.Evaluate(normalizedTime);
            mySpeed *= curveValue;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    void Start()
    {
        StartCoroutine(myCoroutine());
    }
}
