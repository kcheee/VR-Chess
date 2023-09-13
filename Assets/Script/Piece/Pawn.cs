using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{
    // 가능한 움직임을 가지고 움직이는 코드를 짜보자.

    public override bool[,] PossibleMoves()
    {
        bool[,] moves = new bool[8, 8];
        int x = currentX; // 현재 폰의 x 위치
        int y = currentY; // 현재 폰의 y 위치

        // null 값으로 체크.
        Chessman leftChessman = null; // 왼쪽 대각선에 있는 말
        Chessman rightChessman = null; // 오른쪽 대각선에 있는 말
        Chessman forwardChessman = null; // 직진 방향에 있는 말

        int[] EnPassant = BoardManager.Instance.EnPassant; // 앙파상 움직임을 추적하는 배열

        if (isWhite)
        {
            if (y > 0)
            {
                // 왼쪽 대각선으로 이동 가능한 경우, 해당 위치에 있는 말을 가져옵니다.
                if (x > 0) leftChessman = BoardManager.Instance.Chessmans[x - 1, y - 1];
                // 오른쪽 대각선으로 이동 가능한 경우, 해당 위치에 있는 말을 가져옵니다.
                if (x < 7) rightChessman = BoardManager.Instance.Chessmans[x + 1, y - 1];
                // 직진으로 이동 가능한 경우, 해당 위치에 있는 말을 가져옵니다.
                forwardChessman = BoardManager.Instance.Chessmans[x, y - 1];
            }

            // 왕의 대한 체크상태를 해줘야 함.
            // Player Turn과 AI Turn 해줘야함.

            // 직진으로 한 칸 이동 가능하고, 이동 전후에 왕이 체크 상태가 되지 않는 경우,
            if (forwardChessman == null)
            {
                moves[x, y - 1] = true;
            }

            // 왼쪽 대각선에 적군 말이 있고, 왕이 체크 상태가 되지 않는 경우,
            if (leftChessman != null && !leftChessman.isWhite)
            {
                moves[x - 1, y - 1] = true;
            }

            // 오른쪽 대각선에 적군 말이 있고, 왕이 체크 상태가 되지 않는 경우,
            if (rightChessman != null && !rightChessman.isWhite)
            {
                moves[x + 1, y - 1] = true;
            }

            // 첫 번째 움직임인 경우, 두 칸 앞으로 이동 가능하고 왕이 체크 상태가 되지 않는 경우,
            if (y == 6 && forwardChessman == null && BoardManager.Instance.Chessmans[x, y - 2] == null)
            {
                moves[x, y - 2] = true;
            }
        }
        else // 현재 폰이 검은색인 경우
        {
            if (y < 7)
            {
                // 왼쪽 대각선으로 이동 가능한 경우, 해당 위치에 있는 말을 가져옵니다.
                if (x > 0) leftChessman = BoardManager.Instance.Chessmans[x - 1, y + 1];
                // 오른쪽 대각선으로 이동 가능한 경우, 해당 위치에 있는 말을 가져옵니다.
                if (x < 7) rightChessman = BoardManager.Instance.Chessmans[x + 1, y + 1];
                // 직진으로 이동 가능한 경우, 해당 위치에 있는 말을 가져옵니다.
                forwardChessman = BoardManager.Instance.Chessmans[x, y + 1];
            }
            // 직진으로 한 칸 이동 가능하고, 이동 전후에 왕이 체크 상태가 되지 않는 경우,
            if (forwardChessman == null)
            {

                    moves[x, y + 1] = true;
            }
            // 왼쪽 대각선에 적군 말이 있고, 왕이 체크 상태가 되지 않는 경우,
            if (leftChessman != null && leftChessman.isWhite)
            {

                    moves[x - 1, y + 1] = true;
            }
            // 앙파상(En Passant) 움직임을 확인하고 가능한 경우,
            else if (leftChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x - 1)
            {

                    moves[x - 1, y + 1] = true;
            }
            // 오른쪽 대각선에 적군 말이 있고, 왕이 체크 상태가 되지 않는 경우,
            if (rightChessman != null && rightChessman.isWhite)
            {

                    moves[x + 1, y + 1] = true;
            }
            // 앙파상(En Passant) 움직임을 확인하고 가능한 경우,
            else if (rightChessman == null && EnPassant[1] == y + 1 && EnPassant[0] == x + 1)
            {

                    moves[x + 1, y + 1] = true;
            }
            // 첫 번째 움직임인 경우, 두 칸 앞으로 이동 가능하고 왕이 체크 상태가 되지 않는 경우,
            if (y == 1 && forwardChessman == null && BoardManager.Instance.Chessmans[x, y + 2] == null)
            {
                    moves[x, y + 2] = true;
            }
        }
        return moves;
    }

}
