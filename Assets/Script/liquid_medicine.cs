using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liquid_medicine : MonoBehaviour
{
    Tween myTween;

    void Start()
    {
        float shakeDuration = 1f;
        float targetY = transform.position.y - 0.2f;

        myTween = transform.DOMoveY(targetY, shakeDuration).SetLoops(-1, LoopType.Yoyo);
    }


    public void tweenKill()
    {
        myTween.Kill();
    }
}
