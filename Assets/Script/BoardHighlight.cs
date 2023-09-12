using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlight : MonoBehaviour
{

    #region �̱���
    public static BoardHighlight Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // ��� �⹰�� ���� �� ���� ���� ���̶���Ʈ�� �̵������� ���̶���Ʈ
    public void HighlightPossibleMoves(bool[,] allowedMoves, bool White)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    // Highlight Opponent
                    // ������ �ִ� �κ�.
                    if (BoardManager.Instance.Chessmans[i, j] != null)
                    {
                        Debug.Log("���� �� �ִ� �κ�.");
                        // �׽�Ʈ��
                        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        //go.transform.position = new Vector3(i, 0, j);
                    }
                    // �̵� ���� �� �κ� 
                    else
                    {
                        //// �׽�Ʈ��
                        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //go.transform.position = new Vector3(i, 0, j);
                        // ���̶���Ʈ�� ���� Mark�� Material�� ���������.
                    }
                }
            }
        }
    }

}
