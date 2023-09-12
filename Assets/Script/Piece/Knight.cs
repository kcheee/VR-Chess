using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{

    public override bool[,] PossibleMoves()
    {

        bool[,] moves = new bool[8, 8];
        int x = currentX; // ���� ���� x ��ġ
        int y = currentY; // ���� ���� y ��ġ

        // knight ������ üũ

        // Up - left right
        // up left
        knightMove(x - 1, y + 2, ref moves);
        // up right
        knightMove(x + 1, y + 2, ref moves);

        // down - left right
        // down left
        knightMove(x-1, y-2, ref moves);
        // down right
        knightMove(x + 1, y - 2, ref moves);

        // right - up down
        // right up
        knightMove(x+2,y+1, ref moves);
        // right down 
        knightMove(x + 2, y - 1, ref moves);

        // left - up down
        // left up
        knightMove(x - 2, y + 1, ref moves);
        // left down
        knightMove(x - 2, y - 1, ref moves);

        return moves;
    }

    // if �츮�� �⹰�� �ְų�
    // if ����� �⹰�� �ְų� üũ ��.

    void knightMove(int x, int y, ref bool[,] moves)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            // �� ��ġ�� �ִ� ü�� �⹰ ������
            Chessman piece = BoardManager.Instance.Chessmans[x, y];

            // �� ��ġ�� null�̸�
            if (piece == null)
            {
                moves[x, y] = true;
            }
            // null�� �ƴϰ� ����� �⹰�� ������
            else if (piece.isWhite != isWhite)
            {
                moves[x, y] = true;
            }

        }
    }

}
