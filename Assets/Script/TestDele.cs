using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDele : MonoBehaviour
{

    public delegate void TestDelegate(); // 델리게이트 정의

    public TestDelegate testDelegate; // 델리게이트 변수 선언

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            testDelegate();
        }
    }
}
