using System;
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

    // 타입 설정
    public enum ChessType
    {
        KING, QUEEN, BISHUP, KNIGHT, ROOK, PAWN
    }
    public ChessType chesstype;

    // chessman 클래스의 생성재.
    public Chessman Clone()
    {
        // 객체 복사 하는 것
        // 쓰는 이유는 다음과 같다. 
        // AI를 사용하기 위해서는 플레이어의 체스 말의 위치를 알아야 하기 때문이고 그 위치값을 자유자제로 사용하기 위해 객체 복사를 해서 사용한다.
        // 위치를 가져온다.
        return (Chessman)this.MemberwiseClone();

    }

    // 기물의 현재 위치
    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;

        // 기물들을 움직임.
    }

    public void Move(int x, int y, Type type)
    {
        if(type == typeof(Pawn))
        {
            Debug.Log("실행");
            transform.GetComponent<J_PieceMove>().PieceMove(x,y);
        }
        if (type == typeof(Rook))
        {
            Debug.Log("실행");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
            
        }
        if (type == typeof(Knight))
        {
            Debug.Log("실행");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }
        if (type == typeof(Queen))
        {
            Debug.Log("실행");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }
        if (type == typeof(Bishop))
        {
            Debug.Log("실행");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }
        if (type == typeof(King))
        {
            Debug.Log("실행");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }

    }

    // Outline
    public Outline outline;
    private void Awake()
    {
        if (isWhite)
        {
            outline = transform.GetComponent<Outline>();
            outline.enabled = false;
        }
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


    // 킹에 대한 위협들.
    public bool InDanger()
    {
        Chessman piece = null;

        int x = currentX;
        int y = currentY;


        // 양옆 위아래 위험요소 
        // Rook, Queen, king

        // 아래쪽 확인.
        if (y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y - 1];
            // 해당 칸이 비어있지 않고, 상대팀의 말이며, King일 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }

        while (y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 칸이 비어 있다면.
            if (piece == null)
                continue;

            // 같은 팀
            else if (piece.isWhite == isWhite)
                break;

            // Else if the piece is from opponent team
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Down");
                return true;
            }
            break;
        }

        // 다시 현재 위치값으로 초기화.
        x = currentX;
        y = currentY;


        // 오른쪽 확인.
        if (x + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y];

            // 해당 칸이 비어있지 않고, 상대팀의 말이며, King일 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 칸이 비어 있다면.
            if (piece == null)
                continue;

            // 같은 팀
            else if (piece.isWhite == isWhite)
                break;

            // 룩과 퀸이 있으면.
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Right");
                return true;
            }

            break;
        }

        // 다시 현재 위치값으로 초기화.
        x = currentX;
        y = currentY;

        // 왼쪽 확인.
        if (x - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y];
            // 해당 칸이 비어있지 않고, 상대팀의 말이며, King일 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 칸이 비어있다면.
            if (piece == null)
                continue;

            // 같은 팀이라면.
            else if (piece.isWhite == isWhite)
                break;

            // 룩과 퀸 체크
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Left");
                return true;
            }

            break;
        }

        // 다시 현재 위치값으로 초기화.
        x = currentX;
        y = currentY;

        // 위쪽 확인.
        if (y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y + 1];
            // 해당 칸이 비어있지 않고, 상대팀의 말이며, King일 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 칸이 비어있다면.
            if (piece == null)
                continue;

            // 같은 팀.
            else if (piece.isWhite == isWhite)
                break;

            // 룩과 퀸
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Up");
                return true;
            }

            break;
        }

        // 다시 현재 위치값으로 초기화.
        x = currentX;
        y = currentY;


        // 대각선 위험 요소들.
        // bishup, Queen, Pawn, King

        // 왼쪽 대각선 아래 확인. 대각선 이동
        if (x + 1 <= 7 && y - 1 >= 0 && isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y - 1];
            // 해당 칸이 비어있지 않고, 상대팀의 말이며, Pawn 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x + 1 <= 7 && y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y - 1];
            // 해당 칸이 비어있지 않고, 상대팀의 말이며, King일 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7 && y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 칸이 비어있다면.
            if (piece == null)
                continue;

            // 같은 팀.
            else if (piece.isWhite == isWhite)
                break;

            // 대각선이동을 하는 queen과 Bishup 이라면.
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen LR Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;

        // 왼쪽 대각선 위에 확인.
        if (x + 1 <= 7 && y + 1 <= 7 && !isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y + 1];
            // 폰.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x + 1 <= 7 && y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y + 1];
            // 킹.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7 && y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 빈공간.
            if (piece == null)
                continue;

            // 같은 팀이라면.
            else if (piece.isWhite == isWhite)
                break;

            // 퀸과 비숍
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen LR Up");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // 오른쪽 대각선 아래 확인.
        if (x - 1 >= 0 && y - 1 >= 0 && isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y - 1];
            //  해당 칸이 비어있지 않고, 상대팀의 말이며, Pawn 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y - 1];
            //  해당 칸이 비어있지 않고, 상대팀의 말이며, King 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0 && y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 빈공간 이라면.
            if (piece == null)
                continue;

            // 같은 팀이라면
            else if (piece.isWhite == isWhite)
                break;

            // 퀸과 비숍이라면.
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen RL Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // 오른쪽 대각선 위 확인.

        if (x - 1 >= 0 && y + 1 <= 7 && !isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y + 1];
            //  해당 칸이 비어있지 않고, 상대팀의 말이며, Pawn 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x - 1 >= 0 && y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y + 1];
            //  해당 칸이 비어있지 않고, 상대팀의 말이며, King 경우, 위험상태.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0 && y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // 빈공간이라면
            if (piece == null)
                continue;

            // 같은 팀이라면.
            else if (piece.isWhite == isWhite)
                break;

            // 비숍과 퀸이라면.
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen RL Up");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;

        // Knight Threats
        // 나이트는 8방향으로 움직임.

        // DownLeft
        if (KnightThreat(x - 1, y - 2))
            return true;

        // DownRight
        if (KnightThreat(x + 1, y - 2))
            return true;

        // RightDown
        if (KnightThreat(x + 2, y - 1))
            return true;

        // RightUp
        if (KnightThreat(x + 2, y + 1))
            return true;

        // LeftDown
        if (KnightThreat(x - 2, y - 1))
            return true;

        // LeftUp
        if (KnightThreat(x - 2, y + 1))
            return true;

        // UpLeft
        if (KnightThreat(x - 1, y + 2))
            return true;

        // UpRight
        if (KnightThreat(x + 1, y + 2))
            return true;

        return false;
    }


    public bool KnightThreat(int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            Chessman piece = BoardManager.Instance.Chessmans[x, y];
            // 해당 칸이 비어있다면 위협이 없음
            if (piece == null)
                return false;

            // 같은 팀의 말이라면 위협이 없음
            if (piece.isWhite == isWhite)
                return false;

            // 상대 팀의 말이라면
            // 상대팀의 말이 나이트인 경우 위협이 있음
            if (piece.GetType() == typeof(Knight))
            {
                Debug.Log("나이트에 의한 위협");
                return true;        // Yes, there is a Knight threat
            }
        }
        return false;
    }


    // king이 위험한지 체크하는 함수.
    // 만약 어떤 말을 움직여서 킹이 위험하다면 움직임을 허용하지 않게 만드는 함수.
    // 말을 움직일때마다 실행해줘야하는 함수이다.

    public bool KingInDanger(int x, int y)
    {
        // 중요한 부분:
        // 아직 명령을 받지 않은 상태에서 체스판(화면이 아님)에서 말을 움직이려고 함
        // 이 움직임이 킹을 위협하거나 체크 상태로 만들지 않는지 확인해야 함
        // 이후에는 우리가 한 움직임을 되돌림.
        // 모든 변경 사항은 이 함수 내에서만 되돌려짐
        // 화면에 영향을 끼치지 않음.

        // ------------- 백업 시작 -------------
        // 이동하려는 위치의 체스맨 참조 저장
        Chessman tmpChessman = BoardManager.Instance.Chessmans[x, y];
        int tmpCurrentX = currentX;
        int tmpCurrentY = currentY;
        // ------------- 백업 끝 -------------

        // 현재 위치를 떠나 이동하고 좌표를 업데이트함
        BoardManager.Instance.Chessmans[currentX, currentY] = null;
        BoardManager.Instance.Chessmans[x, y] = this;
        this.SetPosition(x, y);

        // 결과를 저장할 변수
        bool result = false;

        // 이제 킹이 위험한지 여부를 확인함
        if (isWhite)
            result = BoardManager.Instance.WhiteKing.InDanger();
        else
            result = BoardManager.Instance.BlackKing.InDanger();

        // 이제 원래 상태로 되돌림
        this.SetPosition(tmpCurrentX, tmpCurrentY);
        BoardManager.Instance.Chessmans[tmpCurrentX, tmpCurrentY] = this;
        BoardManager.Instance.Chessmans[x, y] = tmpChessman;

        // 결과 반환
        return result;
    }


}
