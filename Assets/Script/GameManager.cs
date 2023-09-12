using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool yourTrun;
    private void Start()
    {
        // 보드 활성화.
        // 턴제 시스템.
        yourTrun = true;
    }

    private void Update()
    {
        // 타일 배치
        if (Input.GetKeyDown(KeyCode.V))
        {

        }
    }
    // 턴제 방식으로 코드 

}
