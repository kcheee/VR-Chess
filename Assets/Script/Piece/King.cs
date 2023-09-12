using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{

    // king은 가로 세로
    // 대각선 방향으로 움직임.

    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8];
        int x = currentX;
        int y = currentY;

        // Down
        KingMove(x, y - 1, ref moves);

        // Left
        KingMove(x - 1, y, ref moves);

        // Right
        KingMove(x + 1, y, ref moves);

        // Up
        KingMove(x, y + 1, ref moves);

        // DownLeft
        KingMove(x - 1, y - 1, ref moves);

        // DownRight
        KingMove(x + 1, y - 1, ref moves);

        // UpLeft
        KingMove(x - 1, y + 1, ref moves);

        // UpRight
        KingMove(x + 1, y + 1, ref moves);


        return moves;
    }
    private void KingMove(int x, int y, ref bool[,] moves)
    {
        if (x >= 0 && y >= 0 && x <= 7 && y <= 7)
        {
            Chessman piece = BoardManager.Instance.Chessmans[x, y];

            if (piece == null)
            {
                    moves[x, y] = true;
            }
            // If the piece is not from same team
            else if (piece.isWhite != isWhite)
            {
                    moves[x, y] = true;
            }
        }
    }

    // castiling 함수 해줘야 함.

}
