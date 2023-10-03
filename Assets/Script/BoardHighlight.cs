using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlight : MonoBehaviour
{

    #region 싱글톤
    public static BoardHighlight Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    List<GameObject> HighLights = new List<GameObject>();

    // 상대 기물을 먹을 수 있을 때의 하이라이트와 이동가능한 하이라이트
    public void HighlightPossibleMoves(bool[,] allowedMoves, bool White)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    // Highlight Opponent
                    // 먹을수 있는 부분.
                    if (BoardManager.Instance.Chessmans[i, j] != null)
                    {
                        // 테스트용
                        GameObject highlightObject = Instantiate(Resources.Load<GameObject>("HighlightEat")
                            , new Vector3(i *0.1f , 0.01f, j * 0.1f), Quaternion.identity);
                        HighLights.Add(highlightObject);
                    }
                    // 이동 가능 한 부분 
                    else
                    {
                        //// 테스트용
                        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //go.transform.position = new Vector3(i, 0, j);
                        // 하이라이트나 따로 Mark를 Material를 보여줘야함.

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

    // 이동했을때 하이라이트 뺴줌.
    public void deleteHighlight()
    {
        foreach (GameObject go in HighLights)
        {
            Destroy(go);
        }
        // 리스트 클리어
        HighLights.Clear();
    }

}
