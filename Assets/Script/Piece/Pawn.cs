using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{
    // ������ �������� ������ �����̴� �ڵ带 ¥����.

    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8];
        int x = currentX; // ���� ���� x ��ġ
        int y = currentY; // ���� ���� y ��ġ

        // null ������ üũ.
        Chessman leftChessman = null; // ���� �밢���� �ִ� ��
        Chessman rightChessman = null; // ������ �밢���� �ִ� ��
        Chessman forwardChessman = null; // ���� ���⿡ �ִ� ��

        int[] EnPassant = BoardManager.Instance.EnPassant; // ���Ļ� �������� �����ϴ� �迭

        if (isWhite)
        {
            if (y > 0)
            {
                // ���� �밢������ �̵� ������ ���, �ش� ��ġ�� �ִ� ���� �����ɴϴ�.
                if (x > 0) leftChessman = BoardManager.Instance.Chessmans[x - 1, y - 1];
                // ������ �밢������ �̵� ������ ���, �ش� ��ġ�� �ִ� ���� �����ɴϴ�.
                if (x < 7) rightChessman = BoardManager.Instance.Chessmans[x + 1, y - 1];
                // �������� �̵� ������ ���, �ش� ��ġ�� �ִ� ���� �����ɴϴ�.
                forwardChessman = BoardManager.Instance.Chessmans[x, y - 1];
            }

            // ���� ���� üũ���¸� ����� ��.
            // Player Turn�� AI Turn �������.

            // �������� �� ĭ �̵� �����ϰ�, �̵� ���Ŀ� ���� üũ ���°� ���� �ʴ� ���,
            if (forwardChessman == null)
            {
                moves[x, y - 1] = true;
            }

            // ���� �밢���� ���� ���� �ְ�, ���� üũ ���°� ���� �ʴ� ���,
            if (leftChessman != null && !leftChessman.isWhite)
            {
                moves[x - 1, y - 1] = true;
            }

            // ������ �밢���� ���� ���� �ְ�, ���� üũ ���°� ���� �ʴ� ���,
            if (rightChessman != null && !rightChessman.isWhite)
            {
                moves[x + 1, y - 1] = true;
            }

            // ù ��° �������� ���, �� ĭ ������ �̵� �����ϰ� ���� üũ ���°� ���� �ʴ� ���,
            if (y == 6 && forwardChessman == null && BoardManager.Instance.Chessmans[x, y - 2] == null)
            {
                moves[x, y - 2] = true;
            }
        }
        else // ���� ���� �������� ���
        {
            if (y < 7)
            {
                // ���� �밢������ �̵� ������ ���, �ش� ��ġ�� �ִ� ���� �����ɴϴ�.
                if (x > 0) leftChessman = BoardManager.Instance.Chessmans[x - 1, y + 1];
                // ������ �밢������ �̵� ������ ���, �ش� ��ġ�� �ִ� ���� �����ɴϴ�.
                if (x < 7) rightChessman = BoardManager.Instance.Chessmans[x + 1, y + 1];
                // �������� �̵� ������ ���, �ش� ��ġ�� �ִ� ���� �����ɴϴ�.
                forwardChessman = BoardManager.Instance.Chessmans[x, y + 1];
            }
            // �������� �� ĭ �̵� �����ϰ�, �̵� ���Ŀ� ���� üũ ���°� ���� �ʴ� ���,
            if (forwardChessman == null)
            {

                    moves[x, y + 1] = true;
            }
            // ���� �밢���� ���� ���� �ְ�, ���� üũ ���°� ���� �ʴ� ���,
            if (leftChessman != null && leftChessman.isWhite)
            {

                    moves[x - 1, y + 1] = true;
            }
            // ���Ļ�(En Passant) �������� Ȯ���ϰ� ������ ���,
            else if (leftChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x - 1)
            {

                    moves[x - 1, y + 1] = true;
            }
            // ������ �밢���� ���� ���� �ְ�, ���� üũ ���°� ���� �ʴ� ���,
            if (rightChessman != null && rightChessman.isWhite)
            {

                    moves[x + 1, y + 1] = true;
            }
            // ���Ļ�(En Passant) �������� Ȯ���ϰ� ������ ���,
            else if (rightChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x + 1)
            {

                    moves[x + 1, y + 1] = true;
            }
            // ù ��° �������� ���, �� ĭ ������ �̵� �����ϰ� ���� üũ ���°� ���� �ʴ� ���,
            if (y == 1 && forwardChessman == null && BoardManager.Instance.Chessmans[x, y + 2] == null)
            {
                    moves[x, y + 2] = true;
            }
        }
        return moves;
    }

}
