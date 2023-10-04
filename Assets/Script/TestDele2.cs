using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDele2 : MonoBehaviour
{

    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            TestDele someScript = GetComponent<TestDele>(); // SomeScript는 델리게이트가 정의된 스크립트의 이름입니다.

            if (someScript != null)
            {
                someScript.testDelegate = ddd; // testMethod는 실행하고자 하는 메서드입니다.
                someScript.testDelegate(); // 델리게이트 실행
            }
        }
    }
    private void ddd()
    {
        Debug.Log("tlfgossssd");
    }
}



