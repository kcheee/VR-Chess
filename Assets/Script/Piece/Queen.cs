using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8];
        int x = currentX; // ���� ���� x ��ġ
        int y = currentY; // ���� ���� y ��ġ

        // Queen ������ üũ

        // if x < 0 && y < 0 && y>7 && x>7

        // �밢�� üũ
        // UpRight
        while (y++ < 7 && x++ < 7)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // UpLeft
        while (y++ < 7 && x-- > 0)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // DownRight
        while (y-- > 0 && x++ < 7)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // DownLeft
        while (y-- > 0 && x-- > 0)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }


        // ���� �¿� �̵� üũ�� �ؾ߉�.
        // if x < 0 && y < 0 && y>7 && x>7

        x = currentX;
        y = currentY;

        // Up
        while (y++ < 7)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // Right
        while (x++ < 7)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // Down
        while (y-- > 0)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        x = currentX;
        y = currentY;

        // Left
        while (x-- > 0)
        {
            if (!QueenMove(x, y, ref moves)) break;
        }

        return moves;
    }
    // �޸� ������ ���� ref�� ���
    bool QueenMove(int x, int y, ref bool[,] moves)
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
        if (piece.isWhite != isWhite)
        {
            // �� �ڸ����� ���.
            // �Դ� �� üũ�ؾ���.
            moves[x, y] = true;
        }

        // if �츮�� �⹰�� �ִ���
        return false;
    }
}
