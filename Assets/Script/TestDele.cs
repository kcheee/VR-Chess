using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDele : MonoBehaviour
{

    public delegate void TestDelegate(); // ��������Ʈ ����

    public TestDelegate testDelegate; // ��������Ʈ ���� ����

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            testDelegate();
        }
    }
}
