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
            transform.GetComponent<J_PawnMove>().PawnMove(x,y);
        }
    }

    // Outline
    public Outline outline;
    private void Start()
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



    //public bool KingInDanger(int x, int y)
    //{
    //    // �߿��� �κ�:
    //    // ���� ����� ���� ���� ���¿��� ü����(ȭ���� �ƴ�)���� ���� �����̷��� ��
    //    // �� �������� ŷ�� �����ϰų� üũ ���·� ������ �ʴ��� Ȯ���ؾ� ��
    //    // ���Ŀ��� �츮�� �� �������� �ǵ���.
    //    // ��� ���� ������ �� �Լ� �������� �ǵ�����
    //    // ȭ�鿡 ������ ��ġ�� ����.

    //    // ------------- ��� ���� -------------
    //    // �̵��Ϸ��� ��ġ�� ü���� ���� ����
    //    Chessman tmpChessman = BoardManager.Instance.Chessmans[x, y];
    //    int tmpCurrentX = currentX;
    //    int tmpCurrentY = currentY;
    //    // ------------- ��� �� -------------

    //    // ���� ��ġ�� ���� �̵��ϰ� ��ǥ�� ������Ʈ��
    //    BoardManager.Instance.Chessmans[currentX, currentY] = null;
    //    BoardManager.Instance.Chessmans[x, y] = this;
    //    this.SetPosition(x, y);

    //    // ����� ������ ����
    //    bool result = false;

    //    // ���� ŷ�� �������� ���θ� Ȯ����
    //    if (isWhite)
    //        result = BoardManager.Instance.WhiteKing.InDanger();
    //    else
    //        result = BoardManager.Instance.BlackKing.InDanger();

    //    // ���� ���� ���·� �ǵ���
    //    this.SetPosition(tmpCurrentX, tmpCurrentY);
    //    BoardManager.Instance.Chessmans[tmpCurrentX, tmpCurrentY] = this;
    //    BoardManager.Instance.Chessmans[x, y] = tmpChessman;

    //    // ��� ��ȯ
    //    return result;
    //}


}
