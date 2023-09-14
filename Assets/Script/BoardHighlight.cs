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
                        Debug.Log("먹을 수 있는 부분.");
                        // 테스트용
                        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        go.transform.position = new Vector3(i, 0, j);
                    }
                    // 이동 가능 한 부분 
                    else
                    {
                        //// 테스트용
                        //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //go.transform.position = new Vector3(i, 0, j);
                        // 하이라이트나 따로 Mark를 Material를 보여줘야함.
                    }
                }
            }
        }
    }

}
