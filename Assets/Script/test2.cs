using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.XR.LegacyInputHelpers;

public class test2 : MonoBehaviour
{
    // Start is called before the first frame update
    Tween myTween;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // �ȴ� ȿ��.
            
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            
        }
    }
    public void Gesture()
    {
        Debug.Log("����");
    }
}
