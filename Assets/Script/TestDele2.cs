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
            TestDele someScript = GetComponent<TestDele>(); // SomeScript�� ��������Ʈ�� ���ǵ� ��ũ��Ʈ�� �̸��Դϴ�.

            if (someScript != null)
            {
                someScript.testDelegate = ddd; // testMethod�� �����ϰ��� �ϴ� �޼����Դϴ�.
                someScript.testDelegate(); // ��������Ʈ ����
            }
        }
    }
    private void ddd()
    {
        Debug.Log("tlfgossssd");
    }
}



