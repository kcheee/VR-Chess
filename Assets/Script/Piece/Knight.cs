using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{

    public override bool[,] PossibleMoves()
    {

        bool[,] moves = new bool[8, 8];
        int x = currentX; // 현재 폰의 x 위치
        int y = currentY; // 현재 폰의 y 위치

        // knight 움직임 체크

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

    // if 우리팀 기물이 있거나
    // if 상대팀 기물이 있거나 체크 함.

    void knightMove(int x, int y, ref bool[,] moves)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            // 그 위치에 있는 체스 기물 가져옴
            Chessman piece = BoardManager.Instance.Chessmans[x, y];

            // 그 위치가 null이면
            if (piece == null)
            {
                moves[x, y] = true;
            }
            // null이 아니고 상대편 기물이 있으면
            else if (piece.isWhite != isWhite)
            {
                moves[x, y] = true;
            }

        }
    }

}
