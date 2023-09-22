using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // ü�� �⹰�� ��ġ ������.
    public int currentX { set; get; }
    public int currentY { set; get; }

    // ������ ������
    public bool isWhite;

    public bool isMoved = false;

    // Ÿ�� ����
    public enum ChessType
    {
        KING, QUEEN, BISHUP, KNIGHT, ROOK, PAWN
    }
    public ChessType chesstype;

    // chessman Ŭ������ ������.
    public Chessman Clone()
    {
        // ��ü ���� �ϴ� ��
        // ���� ������ ������ ����. 
        // AI�� ����ϱ� ���ؼ��� �÷��̾��� ü�� ���� ��ġ�� �˾ƾ� �ϱ� �����̰� �� ��ġ���� ���������� ����ϱ� ���� ��ü ���縦 �ؼ� ����Ѵ�.
        // ��ġ�� �����´�.
        return (Chessman)this.MemberwiseClone();

    }

    // �⹰�� ���� ��ġ
    public void SetPosition(int x, int y)
    {
        currentX = x;
        currentY = y;

        // �⹰���� ������.
    }

    public void Move(int x, int y, Type type)
    {
        if(type == typeof(Pawn))
        {
            Debug.Log("����");
            transform.GetComponent<J_PieceMove>().PieceMove(x,y);
        }
        if (type == typeof(Rook))
        {
            Debug.Log("����");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
            
        }
        if (type == typeof(Knight))
        {
            Debug.Log("����");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }
        if (type == typeof(Queen))
        {
            Debug.Log("����");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }
        if (type == typeof(Bishop))
        {
            Debug.Log("����");
            transform.GetComponent<J_PieceMove>().PieceMove(x, y);
        }
        if (type == typeof(King))
        {
            Debug.Log("����");
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


    // �̵� ������ ��ġ �ʱ�ȭ.
    // ���� �⹰���� �� �Լ��� ������ �ʱ�ȭ�ϰ� ����.
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


    // ŷ�� ���� ������.
    public bool InDanger()
    {
        Chessman piece = null;

        int x = currentX;
        int y = currentY;


        // �翷 ���Ʒ� ������ 
        // Rook, Queen, king

        // �Ʒ��� Ȯ��.
        if (y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y - 1];
            // �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King�� ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }

        while (y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ĭ�� ��� �ִٸ�.
            if (piece == null)
                continue;

            // ���� ��
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

        // �ٽ� ���� ��ġ������ �ʱ�ȭ.
        x = currentX;
        y = currentY;


        // ������ Ȯ��.
        if (x + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y];

            // �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King�� ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ĭ�� ��� �ִٸ�.
            if (piece == null)
                continue;

            // ���� ��
            else if (piece.isWhite == isWhite)
                break;

            // ��� ���� ������.
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Right");
                return true;
            }

            break;
        }

        // �ٽ� ���� ��ġ������ �ʱ�ȭ.
        x = currentX;
        y = currentY;

        // ���� Ȯ��.
        if (x - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y];
            // �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King�� ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ĭ�� ����ִٸ�.
            if (piece == null)
                continue;

            // ���� ���̶��.
            else if (piece.isWhite == isWhite)
                break;

            // ��� �� üũ
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Left");
                return true;
            }

            break;
        }

        // �ٽ� ���� ��ġ������ �ʱ�ȭ.
        x = currentX;
        y = currentY;

        // ���� Ȯ��.
        if (y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y + 1];
            // �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King�� ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ĭ�� ����ִٸ�.
            if (piece == null)
                continue;

            // ���� ��.
            else if (piece.isWhite == isWhite)
                break;

            // ��� ��
            if (piece.GetType() == typeof(Rook) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Rook/Queen Up");
                return true;
            }

            break;
        }

        // �ٽ� ���� ��ġ������ �ʱ�ȭ.
        x = currentX;
        y = currentY;


        // �밢�� ���� ��ҵ�.
        // bishup, Queen, Pawn, King

        // ���� �밢�� �Ʒ� Ȯ��. �밢�� �̵�
        if (x + 1 <= 7 && y - 1 >= 0 && isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y - 1];
            // �ش� ĭ�� ������� �ʰ�, ������� ���̸�, Pawn ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x + 1 <= 7 && y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y - 1];
            // �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King�� ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7 && y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ĭ�� ����ִٸ�.
            if (piece == null)
                continue;

            // ���� ��.
            else if (piece.isWhite == isWhite)
                break;

            // �밢���̵��� �ϴ� queen�� Bishup �̶��.
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen LR Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;

        // ���� �밢�� ���� Ȯ��.
        if (x + 1 <= 7 && y + 1 <= 7 && !isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y + 1];
            // ��.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x + 1 <= 7 && y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x + 1, y + 1];
            // ŷ.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x++ < 7 && y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // �����.
            if (piece == null)
                continue;

            // ���� ���̶��.
            else if (piece.isWhite == isWhite)
                break;

            // ���� ���
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen LR Up");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // ������ �밢�� �Ʒ� Ȯ��.
        if (x - 1 >= 0 && y - 1 >= 0 && isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y - 1];
            //  �ش� ĭ�� ������� �ʰ�, ������� ���̸�, Pawn ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y - 1];
            //  �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0 && y-- > 0)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ����� �̶��.
            if (piece == null)
                continue;

            // ���� ���̶��
            else if (piece.isWhite == isWhite)
                break;

            // ���� ����̶��.
            if (piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Queen))
            {
                Debug.Log("Threat from Bishup/Queen RL Down");
                return true;
            }

            break;
        }

        x = currentX;
        y = currentY;
        // ������ �밢�� �� Ȯ��.

        if (x - 1 >= 0 && y + 1 <= 7 && !isWhite)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y + 1];
            //  �ش� ĭ�� ������� �ʰ�, ������� ���̸�, Pawn ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(Pawn))
                return true;
        }
        if (x - 1 >= 0 && y + 1 <= 7)
        {
            piece = BoardManager.Instance.Chessmans[x - 1, y + 1];
            //  �ش� ĭ�� ������� �ʰ�, ������� ���̸�, King ���, �������.
            if (piece != null && piece.isWhite != isWhite && piece.GetType() == typeof(King))
                return true;
        }
        while (x-- > 0 && y++ < 7)
        {
            piece = BoardManager.Instance.Chessmans[x, y];
            // ������̶��
            if (piece == null)
                continue;

            // ���� ���̶��.
            else if (piece.isWhite == isWhite)
                break;

            // ���� ���̶��.
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
        // ����Ʈ�� 8�������� ������.

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
            // �ش� ĭ�� ����ִٸ� ������ ����
            if (piece == null)
                return false;

            // ���� ���� ���̶�� ������ ����
            if (piece.isWhite == isWhite)
                return false;

            // ��� ���� ���̶��
            // ������� ���� ����Ʈ�� ��� ������ ����
            if (piece.GetType() == typeof(Knight))
            {
                Debug.Log("����Ʈ�� ���� ����");
                return true;        // Yes, there is a Knight threat
            }
        }
        return false;
    }


    // king�� �������� üũ�ϴ� �Լ�.
    // ���� � ���� �������� ŷ�� �����ϴٸ� �������� ������� �ʰ� ����� �Լ�.
    // ���� �����϶����� ����������ϴ� �Լ��̴�.

    public bool KingInDanger(int x, int y)
    {
        // �߿��� �κ�:
        // ���� ����� ���� ���� ���¿��� ü����(ȭ���� �ƴ�)���� ���� �����̷��� ��
        // �� �������� ŷ�� �����ϰų� üũ ���·� ������ �ʴ��� Ȯ���ؾ� ��
        // ���Ŀ��� �츮�� �� �������� �ǵ���.
        // ��� ���� ������ �� �Լ� �������� �ǵ�����
        // ȭ�鿡 ������ ��ġ�� ����.

        // ------------- ��� ���� -------------
        // �̵��Ϸ��� ��ġ�� ü���� ���� ����
        Chessman tmpChessman = BoardManager.Instance.Chessmans[x, y];
        int tmpCurrentX = currentX;
        int tmpCurrentY = currentY;
        // ------------- ��� �� -------------

        // ���� ��ġ�� ���� �̵��ϰ� ��ǥ�� ������Ʈ��
        BoardManager.Instance.Chessmans[currentX, currentY] = null;
        BoardManager.Instance.Chessmans[x, y] = this;
        this.SetPosition(x, y);

        // ����� ������ ����
        bool result = false;

        // ���� ŷ�� �������� ���θ� Ȯ����
        if (isWhite)
            result = BoardManager.Instance.WhiteKing.InDanger();
        else
            result = BoardManager.Instance.BlackKing.InDanger();

        // ���� ���� ���·� �ǵ���
        this.SetPosition(tmpCurrentX, tmpCurrentY);
        BoardManager.Instance.Chessmans[tmpCurrentX, tmpCurrentY] = this;
        BoardManager.Instance.Chessmans[x, y] = tmpChessman;

        // ��� ��ȯ
        return result;
    }


}
