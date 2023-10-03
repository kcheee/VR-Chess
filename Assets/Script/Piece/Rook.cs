using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Chessman
{
    public Rook()
    {
        value = 50;
    }

    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8];
        int x = currentX; // ���� ���� x ��ġ
        int y = currentY; // ���� ���� y ��ġ

        // Rook ������ üũ

        // ���� �¿� �̵� üũ�� �ؾ߉�.
        // if x < 0 && y < 0 && y>7 && x>7

        // Up
        while (y++ < 7)
        {
            if (!RookMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // Right
        while (x++ < 7)
        {
            if (!RookMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // Down
        while (y-- > 0)
        {
            if (!RookMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // Left
        while (x-- > 0)
        {
            if (!RookMove(x, y, ref moves)) break;
        }

        return moves;
    }

    // �޸� ������ ���� ref�� ���
    bool RookMove(int x, int y, ref bool[,] moves)
    {
        // �⹰�� ������
        Chessman piece = BoardManager.Instance.Chessmans[x, y];

        // �⹰�� ������ ��� ����.
        if (piece == null)
        {
            moves[x, y] = true;
            return true;
        }
        // if ���� �⹰�� ������ �� �ڸ����� ����.
        if(piece.isWhite != isWhite)
        {
            // �� �ڸ����� ���.
            // �Դ� �� üũ�ؾ���.
            moves[x, y] = true;
        }

        // if �츮�� �⹰�� �ִ���
        return false;
    }
}
