using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // 체스 기물의 위치 가져옴.
    public int currentX { set; get; }
    public int currentY { set; get; }

    // 누구의 턴인지
    public bool isWhite;

    public bool isMoved = false;

    // 기물의 현재 위치
    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    // Outline
    public Outline outline;
    private void Start()
    {
        outline = transform.GetComponent<Outline>();
        outline.enabled = false;
    }


    // 이동 가능한 위치 초기화.
    // 각자 기물에서 이 함수를 가져와 초기화하고 실행.
    public virtual bool[,] PossibleMoves()
    {
        bool[,] arr = new bool[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                arr[i, j] = false;
            }
        }
        return arr;
    }

    //public bool KingInDanger(int x, int y)
    //{
    //    // 중요한 부분:
    //    // 아직 명령을 받지 않은 상태에서 체스판(화면이 아님)에서 말을 움직이려고 함
    //    // 이 움직임이 킹을 위협하거나 체크 상태로 만들지 않는지 확인해야 함
    //    // 이후에는 우리가 한 움직임을 되돌림.
    //    // 모든 변경 사항은 이 함수 내에서만 되돌려짐
    //    // 화면에 영향을 끼치지 않음.

    //    // ------------- 백업 시작 -------------
    //    // 이동하려는 위치의 체스맨 참조 저장
    //    Chessman tmpChessman = BoardManager.Instance.Chessmans[x, y];
    //    int tmpCurrentX = currentX;
    //    int tmpCurrentY = currentY;
    //    // ------------- 백업 끝 -------------

    //    // 현재 위치를 떠나 이동하고 좌표를 업데이트함
    //    BoardManager.Instance.Chessmans[currentX, currentY] = null;
    //    BoardManager.Instance.Chessmans[x, y] = this;
    //    this.SetPosition(x, y);

    //    // 결과를 저장할 변수
    //    bool result = false;

    //    // 이제 킹이 위험한지 여부를 확인함
    //    if (isWhite)
    //        result = BoardManager.Instance.WhiteKing.InDanger();
    //    else
    //        result = BoardManager.Instance.BlackKing.InDanger();

    //    // 이제 원래 상태로 되돌림
    //    this.SetPosition(tmpCurrentX, tmpCurrentY);
    //    BoardManager.Instance.Chessmans[tmpCurrentX, tmpCurrentY] = this;
    //    BoardManager.Instance.Chessmans[x, y] = tmpChessman;

    //    // 결과 반환
    //    return result;
    //}


}
