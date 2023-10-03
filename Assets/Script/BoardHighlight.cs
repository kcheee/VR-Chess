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

    List<GameObject> HighLights = new List<GameObject>();

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
                        // �׽�Ʈ��
                        GameObject highlightObject = Instantiate(Resources.Load<GameObject>("HighlightEat")
                            , new Vector3(i *0.1f , 0.01f, j * 0.1f), Quaternion.identity);
                        HighLights.Add(highlightObject);
                    }
                    // �̵� ���� �� �κ� 
                    else
                    {
                        //// �׽�Ʈ��
                        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //go.transform.position = new Vector3(i, 0, j);
                        // ���̶���Ʈ�� ���� Mark�� Material�� ���������.

                        GameObject highlightObject = Instantiate(Resources.Load<GameObject>("Highlight")
                            , new Vector3(i * 0.1f, 0.01f, j * 0.1f),Quaternion.identity);
                        HighLights.Add(highlightObject);
                        //go.transform.position = new Vector3(i, 0, j);
                        //Debug.Log(go.transform.position);
                    }
                }
            }
        }
    }

    // �̵������� ���̶���Ʈ ����.
    public void deleteHighlight()
    {
        foreach (GameObject go in HighLights)
        {
            Destroy(go);
        }
        // ����Ʈ Ŭ����
        HighLights.Clear();
    }

}
