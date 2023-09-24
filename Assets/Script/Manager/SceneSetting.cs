using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// 난이도 값 저장과 각종 씬들 전환시 값 참조
public class SceneSetting : MonoBehaviour
{
    #region 싱글톤, don'tDestoryOnLoad
    static public SceneSetting Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // 난이도 관리
    [Header("난이도 설정(2부터 쉬움)")]
    public int depth = 2;


}
