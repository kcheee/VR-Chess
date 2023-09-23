using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.XR.LegacyInputHelpers;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject go;
    Tween myTween;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // °È´Â È¿°ú.
            float shakeDuration = 0.5f;
            float targetY = transform.position.y - 0.15f;

             myTween = transform.DOMoveY(targetY, shakeDuration).SetLoops(-1, LoopType.Yoyo);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            myTween.Kill();
        }
    }
}
