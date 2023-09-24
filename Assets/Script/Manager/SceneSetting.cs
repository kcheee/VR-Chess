using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

// ���̵� �� ����� ���� ���� ��ȯ�� �� ����
public class SceneSetting : MonoBehaviour
{
    #region �̱���, don'tDestoryOnLoad
    static public SceneSetting Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // ���̵� ����
    [Header("���̵� ����(2���� ����)")]
    public int depth = 2;


}
