using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI : MonoBehaviour
{
    public static ChessAI Instance { set; get; }

    /* ------------------------- ȯ�� ���� ----------------------*/
    private List<Chessman> ActiveChessmans;
    private Chessman[,] Chessmans;
    private int[] EnPassant;

    // ������ Chessmans[,] �迭�� �����ص� ����
    // (�ֳ��ϸ� Chessman Ŭ������ �� ���� Ŭ�������� BoardManager.Instance.Chessmans�� ����ϱ� ������)
    // NPC�� ���� �̵��� �����ϱ� ���� �� ������ �����ϰ�,
    // NPCMove() �Լ��� ������ �ٽ� �����ؾ���
    private Chessman[,] ActualChessmansReference;
    private Chessman ActualWhiteKing;
    private Chessman ActualBlackKing;
    private Chessman ActualWhiteRook1;
    private Chessman ActualWhiteRook2;
    private Chessman ActualBlackRook1;
    private Chessman ActualBlackRook2;
    private int[] ActualEnPassant;

    // ���� �̷��� ������ ����
    private Stack<State> History;

    // �ִ� Ž�� ���� (Ž���� �̷��� �̵� Ƚ��)
    private int maxDepth;

    // NPC�� �̵��� ü���ǰ� �̵��� ��ġ
    private Chessman NPCSelectedChessman = null;
    private int moveX = -1;
    private int moveY = -1;
    private int winningValue = 0;

    // ��� ���� �ð� ����� ���� ����
    private long totalTime = 0;
    private long totalRun = 0;
    public long averageResponseTime = 0;

    string detail, board;

    private void Start()
    {
        Instance = this;
    }

    // Funtion that makes NPC move
    public void NPCMove()
    {
        // �ð� ����.
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        // detail = "Start:\n";
        // board = "Current Actual Board :\n";
        // printBoard();

        // ���ο� ���� �̷� ���� ����.
        History = new Stack<State>();

        /* --------------------- ���� --------------------- */

        ActualChessmansReference = BoardManager.Instance.Chessmans;
        ActualWhiteKing = BoardManager.Instance.WhiteKing;
        ActualBlackKing = BoardManager.Instance.BlackKing;
        ActualWhiteRook1 = BoardManager.Instance.WhiteRook1;
        ActualWhiteRook2 = BoardManager.Instance.WhiteRook2;
        ActualBlackRook1 = BoardManager.Instance.BlackRook1;
        ActualBlackRook2 = BoardManager.Instance.BlackRook2;
        ActualEnPassant = BoardManager.Instance.EnPassant;

        ActiveChessmans = new List<Chessman>();
        Chessmans = new Chessman[8, 8];

        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                if (ActualChessmansReference[x, y] != null)
                {
                    Chessman currChessman = ActualChessmansReference[x, y].Clone();
                    ActiveChessmans.Add(currChessman);
                    Chessmans[x, y] = currChessman;
                }
                else
                {
                    Chessmans[x, y] = null;
                }
            }

        // �������� �����ִ� �Լ�.
        Shuffle(ActiveChessmans);

        EnPassant = new int[2] { ActualEnPassant[0], ActualEnPassant[0] };

        /* --------------------- Think --------------------- */

        // Critical Part:
        // �߿��� �κ�:
        // ���� �Ŵ��� ��ũ��Ʈ���� �����Ǵ� ������ �ٽ��� Chessmans[,] �迭�� ������ �����Դϴ�.
        // �̰��� BoardManager ��ũ��Ʈ���� ��ġ�� ���� ���� ���� �ִ� ��� ü���ǿ� ���� �����͸� �����ϱ� �����Դϴ�.
        // ���� �̹� ���� Chessmans[,] �迭�� ������ �����ϰ� �ٸ� �������� �Ҵ��ϰ� �ֽ��ϴ�.
        // (ŷ �� ��, ���Ļ� �̵� �迭�� ���ؼ��� �����ϰ� ����˴ϴ�)
        BoardManager.Instance.Chessmans = Chessmans;
        BoardManager.Instance.WhiteKing = Chessmans[ActualWhiteKing.currentX, ActualWhiteKing.currentY];
        BoardManager.Instance.BlackKing = Chessmans[ActualBlackKing.currentX, ActualBlackKing.currentY];
        if (ActualWhiteRook1 != null) BoardManager.Instance.WhiteRook1 = Chessmans[ActualWhiteRook1.currentX, ActualWhiteRook1.currentY];
        if (ActualWhiteRook2 != null) BoardManager.Instance.WhiteRook2 = Chessmans[ActualWhiteRook2.currentX, ActualWhiteRook2.currentY];
        if (ActualBlackRook1 != null) BoardManager.Instance.BlackRook1 = Chessmans[ActualBlackRook1.currentX, ActualBlackRook1.currentY];
        if (ActualBlackRook2 != null) BoardManager.Instance.BlackRook2 = Chessmans[ActualBlackRook2.currentX, ActualBlackRook2.currentY];
        BoardManager.Instance.EnPassant = EnPassant;

        // ���� ������ �̵��� ���� �Լ�.
        Think();

        // Chessmans[,] �迭�� ������
        BoardManager.Instance.Chessmans = ActualChessmansReference;
        BoardManager.Instance.WhiteKing = ActualWhiteKing;
        BoardManager.Instance.BlackKing = ActualBlackKing;
        BoardManager.Instance.WhiteRook1 = ActualWhiteRook1;
        BoardManager.Instance.WhiteRook2 = ActualWhiteRook2;
        BoardManager.Instance.BlackRook1 = ActualBlackRook1;
        BoardManager.Instance.BlackRook2 = ActualBlackRook2;
        BoardManager.Instance.EnPassant = ActualEnPassant;

        // board = board + "\n\nAfter Restoring with actual board :\n";
        // printBoard();

        /* ---------------------- Act ---------------------- */
        // ������ �̵��� ���� ���õ� �ڵ�.
        // ������ �̵��� �ϴ� �⹰ 

        // NPC�� ������ �⹰�� ���� ��ġ�� Ÿ�� ���.
        Debug.Log(NPCSelectedChessman.GetType() + " to (" + moveX + ", " + moveY + ") " + winningValue + "\n"); // remove this line
        BoardManager.Instance.SelectedChessman = BoardManager.Instance.Chessmans[NPCSelectedChessman.currentX, NPCSelectedChessman.currentY];
        BoardManager.Instance.allowedMoves = BoardManager.Instance.SelectedChessman.PossibleMoves();

        // Debug.Log("Moving");
        // ü���� ������/
        BoardManager.Instance.MoveChessman(moveX, moveY);

        // �ð� ���� ���� �� ���.
        stopwatch.Stop();
        totalTime += stopwatch.ElapsedMilliseconds;
        totalRun++;

        // ��� ����ð�.
        averageResponseTime = totalTime / totalRun;
    }

    private void Think()
    {
        // �ִ� Ž�� ����.
        maxDepth = 4;
        int depth = maxDepth - 1;
        // winningValue = MiniMax(depth, true);
        winningValue = AlphaBeta(depth, true, System.Int32.MinValue, System.Int32.MaxValue);
    }

    #region MinMax Algorithm ���� ����������.
    // MinMax �˰��� ���⼱ ������ ����.
    private int MiniMax(int depth, bool isMax)
    {
        // ���� �ִ� ���̿� �����ϰų� ������ ����Ǿ�����
        if (depth == 0 || isGameOver())
        {
            // ���� ������ ��ġ�� ���.
            int value = StaticEvaluationFunction();

            return value;
        }

        // string ActiveChessmansDetail = "";

        // If it is max turn(NPC turn : Black)
        // NPC�� �ִ��� ��ġ�� ã�� ��.
        // AI�� ���� ��ġ�� �������� ã�� ����.
        if (isMax)
        {
            int hValue = System.Int32.MinValue;
            // int ind = 0;

            // Get list of all possible moves with their heuristic value
            // For all chessmans
            // ��� ü���ǿ� ���� �ݺ�.
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                // ���(�÷��̾�)�̸� �Ѿ.
                if (chessman.isWhite) continue;

                // ���� �⹰�� ������ �̵��� ������.
                bool[,] allowedMoves = chessman.PossibleMoves();

                // detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // ��� ������ �̵��� ���� �ݺ�.
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving Black " + chessman.GetType() + " to (" + x + ", " + y + ")");

                            // Critical Section : 
                            // 1) Making the current move to see next possible moves after this move in next calls
                            // ���� ���� �����Ͽ� ������ ���Ÿ� �����ϰ�, �� ����� ���¿��� ������ ���� ���� Ȯ���մϴ�.

                            // ���� �̵��� ����.
                            Move(chessman, x, y, depth);

                            // 2 ) �� �����̿� ���� �� ���.
                            int thisMoveValue = MiniMax(depth - 1, !isMax);

                            if (hValue < thisMoveValue)
                            {
                                hValue = thisMoveValue;

                                // ���� ���� ��ġ�� ���� �̵��� �����.
                                if (depth == maxDepth - 1)
                                {
                                    NPCSelectedChessman = chessman;
                                    moveX = x;
                                    moveY = y;
                                }
                            }

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // 3 ) Undo the current move to get back the same state that was there before making the current move'
                            // ���� �̵��� ����Ͽ� ���� ���·� ���ư��ϴ�.
                            Undo(depth);
                        }
                    }
                }
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }

        // If it is min turn(Player turn : White)
        // ���� ��ġ�� �� �÷��̾�.
        else
        {
            // ���� ���� ��ġ�� �ʱ�ȭ
            int hValue = System.Int32.MaxValue;
            // int ind = 0;

            // Get list of all possible moves with their heuristic value
            // For all chessmans
            // ��� �⹰�� ���� �ݺ�.
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "\n(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                // ������(Npc)�̸� �Ѿ.
                if (!chessman.isWhite) continue;

                // ������ �������� ������.
                bool[,] allowedMoves = chessman.PossibleMoves();

                // if(depth == 2) detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // For all possible moves
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving White " + chessman.GetType() + " to (" + x + ", " + y + ")\n");

                            // Critical Section : 
                            // ���� �̵��� �����Ͽ� ������ ���¸� ����.
                            Move(chessman, x, y, depth);

                            // ���� �̵��� �򰡸� ���.
                            int thisMoveValue = MiniMax(depth - 1, !isMax);

                            if (hValue > thisMoveValue)
                            {
                                hValue = thisMoveValue;
                                // The following 6-7 lines are commented, that is suggesting that 
                                // �ּҰ��� ã�� ��(min turn)���� NPCSelectedChessman, moveX �� moveY ���� ������Ʈ���� �ʰڴٴ� �ǹ�
                                // if(depth == maxDepth-1)
                                // {
                                //     NPCSelectedChessman = chessman;
                                //     moveX = x;
                                //     moveY = y;
                                // }
                            }

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // �̵��� ������ ���
                            Undo(depth);
                        }
                    }
                }
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }
    }

    #endregion

    // Alpha-Beta Algorithm
    private int AlphaBeta(int depth, bool isMax, int alpha, int beta)
    {
        // ���̰� �ִ� ���̿� �����߰ų� ������ ������ ���.
        if (depth == 0 || isGameOver())
        {
            // �� �Լ�.
            int value = StaticEvaluationFunction();

            return value;
        }

        // string ActiveChessmansDetail = "";

        // If it is max turn(NPC turn : Black)
        // Npc�� ��. 
        // Npc�� �ִ��� ��ġ�� �������Ѵ�.
        if (isMax)
        {
            int hValue = System.Int32.MinValue;
            // int ind = 0;
            // Get list of all possible moves with their heuristic value
            // For all chessmans

            // ��� ü�� �⹰
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                // �÷��̾� ü�� �⹰�̸� �н�
                if (chessman.isWhite) continue;

                // �⹰�� ���� ������ ������.
                bool[,] allowedMoves = chessman.PossibleMoves();

                // detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // For all possible moves
                // ��� ������ ������.
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving Black " + chessman.GetType() + " to (" + x + ", " + y + ")");

                            // Critical Section : 
                            // ���� �̵��� �����Ͽ� ������ ���¸� �����մϴ�.
                            Move(chessman, x, y, depth);

                            // ���� �̵��� �򰡸� ����մϴ�.
                            int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // ���� �̵��� ����Ͽ� ���� ���·� ���ư��ϴ�.
                            Undo(depth);

                            if (hValue < thisMoveValue)
                            {
                                hValue = thisMoveValue;

                                // ���� ���� �򰡸� �����.
                                if (depth == maxDepth - 1)
                                {
                                    // Npc�� ������ �⹰�� ������.
                                    NPCSelectedChessman = chessman;
                                    moveX = x;
                                    moveY = y;
                                }
                            }
                            // ���� ���� ������Ʈ
                            if (hValue > alpha)
                                alpha = hValue;

                            // ��Ÿ �ƿ��� Ȯ�� 
                            if (beta <= alpha)
                                break;
                        }
                    }
                    // ��Ÿ �ƿ��� Ȯ��
                    if (beta <= alpha)
                        break;
                }
                // ��Ÿ �ƿ��� Ȯ��
                if (beta <= alpha)
                    break;
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }
        // If it is min turn(Player turn : White)
        else
        {
            int hValue = System.Int32.MaxValue;
            // int ind = 0;

            // Get list of all possible moves with their heuristic value
            // For all chessmans
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "\n(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                if (!chessman.isWhite) continue;

                bool[,] allowedMoves = chessman.PossibleMoves();

                // if(depth == 2) detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // For all possible moves
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving White " + chessman.GetType() + " to (" + x + ", " + y + ")\n");

                            // Critical Section : 
                            // ���� �̵��� �����Ͽ� ������ ���¸� �����մϴ�.
                            Move(chessman, x, y, depth);

                            // ���� ������ ��
                            int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // ���� ������ ���.
                            Undo(depth);

                            // ���� ���� ��
                            if (hValue > thisMoveValue)
                            {
                                hValue = thisMoveValue;
                                // The following 6-7 lines are commented, that is suggesting that 
                                // We won't update NPCSelectedChessman, moveX and moveY in min turn
                                // if(depth == maxDepth-1)
                                // {
                                //     NPCSelectedChessman = chessman;
                                //     moveX = x;
                                //     moveY = y;
                                // }
                            }

                            if (hValue < beta)
                                beta = hValue;

                            if (beta <= alpha)
                                break;
                        }
                    }

                    if (beta <= alpha)
                        break;
                }

                if (beta <= alpha)
                    break;
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }
    }

    // �⹰�� ���� �� �Լ�.
    // ���� �⹰�� ��� �⹰�� �����ִ� �͵�� �򰡸� ���� �Լ��̴�.
    private int StaticEvaluationFunction()
    {
        int TotalScore = 0;
        int curr = 0;
        foreach (Chessman chessman in ActiveChessmans)
        {
            if (chessman.GetType() == typeof(King))
                curr = 900;
            if (chessman.GetType() == typeof(Queen))
                curr = 90;
            if (chessman.GetType() == typeof(Rook))
                curr = 50;
            if (chessman.GetType() == typeof(Bishop))
                curr = 30;
            if (chessman.GetType() == typeof(Knight))
                curr = 30;
            if (chessman.GetType() == typeof(Pawn))
                curr = 10;

            // �ش� �⹰�� ����� ���, ���� �ƴѰ�� ����.
            if (chessman.isWhite)
                TotalScore -= curr;
            else
                TotalScore += curr;
        }
        return TotalScore;
    }


    // CheckMate���� Ȯ��.
    private bool isGameOver()
    {
        // To be implemented
        // ���� ���� ������ ��ġ�� ������.
        int currScore = StaticEvaluationFunction();

        // ���� ���� ��ġ�� -290���� �۰ų� 290���� ũ�ٸ�, ������ ����� ������ �Ǵ��մϴ�.
        if ((currScore < -290) || (currScore > 290))
            return true;
        return false;
    }

    private void Move(Chessman chessman, int x, int y, int depth)
    {
        // �̵��� ü���ǿ� ���� ������ �����մϴ�.
        // Ʃ��.
        (Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman;

        // ���� ü���ǿ� ���� ������ �����մϴ�.
        (Chessman chessman, (int x, int y) Position) capturedChessman = (null, (-1, -1));

        // ���� ���� '���Ļ�' �̵� ���¸� �����մϴ�.
        (int x, int y) EnPassantStatus;

        // ���θ�� �̵��� ���� �����Դϴ�. (���θ�� ����, ���θ�ǵ� ü����)
        (bool wasPromotion, Chessman promotedChessman) PromotionMove = (false, null);

        // ĳ���� �̵��� ���� �����Դϴ�. (ĳ������ �̷�������� ����, ŷ ���̵� ����)
        (bool wasCastling, bool isKingSide) CastlingMove;

        // �̵��� ü���ǿ� ���� ������ �����մϴ�.
        movedChessman.chessman = chessman; // �̵��� ü����
        movedChessman.oldPosition = (chessman.currentX, chessman.currentY); // �̵� �� ��ġ, �ʱ� ��ġ.
        movedChessman.newPosition = (x, y); // �̵� �� ��ġ
        movedChessman.isMoved = chessman.isMoved; // ü������ �̵� ����

        EnPassantStatus = (EnPassant[0], EnPassant[1]); // ���� ���Ļ� �̵� ���� ����



        // ��� ����
        Chessman opponent = Chessmans[x, y]; // �̵��� ��ġ�� �ִ� �� ü����
        if (opponent != null)
        {
            // ���� ��´�
            capturedChessman.chessman = opponent; // ���� �� ü����
            capturedChessman.Position = (x, y); // ���� �� ü������ ��ġ

            Chessmans[x, y] = null; // ü�����忡�� �ش� ��ġ�� ���
            ActiveChessmans.Remove(opponent); // ���� Ȱ�� ü���� ����Ʈ���� ����
        }

        // --------------���Ļ� move manager--------------
        #region ���Ļ� 
        // �̰� �н�
        // If it is an EnPassant move than Capture the opponent
        if (EnPassant[0] == x && EnPassant[1] == y && chessman.GetType() == typeof(Pawn))
        {
            if (chessman.isWhite)
            {
                opponent = Chessmans[x, y + 1];

                capturedChessman.chessman = opponent;
                capturedChessman.Position = (x, y + 1);
                Chessmans[x, y + 1] = null;
            }
            else
            {
                opponent = Chessmans[x, y - 1];

                capturedChessman.chessman = opponent;
                capturedChessman.Position = (x, y - 1);
                Chessmans[x, y - 1] = null;
            }

            ActiveChessmans.Remove(opponent);
        }

        // Reset the EnPassant move
        EnPassant[0] = EnPassant[1] = -1;

        // Set EnPassant available for opponent
        if (chessman.GetType() == typeof(Pawn))
        {
            //-------Promotion Move Manager------------
            if (y == 7 || y == 0)
            {
                ActiveChessmans.Remove(chessman);
                Chessmans[x, y] = gameObject.AddComponent<Queen>();
                Chessmans[x, y].SetPosition(x, y);
                Chessmans[x, y].isWhite = chessman.isWhite;
                chessman = Chessmans[x, y];
                ActiveChessmans.Add(chessman);

                PromotionMove = (true, chessman);
            }
            //-------Promotion Move Manager Over-------

            if (chessman.currentY == 1 && y == 3)
            {
                EnPassant[0] = x;
                EnPassant[1] = y - 1;
            }
            if (chessman.currentY == 6 && y == 4)
            {
                EnPassant[0] = x;
                EnPassant[1] = y + 1;
            }
        }
        #endregion
        // -------���Ļ� Move Manager Over-------


        // -------ĳ���� Move Manager------------
        #region ĳ����
        CastlingMove = (false, false);

        // If the selected chessman is King and is trying Castling move(two steps movement)
        if (chessman.GetType() == typeof(King) && System.Math.Abs(x - chessman.currentX) == 2)
        {
            // King Side (towards (0, 0))
            if (x - chessman.currentX < 0)
            {
                // Moving Rook1
                Chessmans[x + 1, y] = Chessmans[x - 1, y];
                Chessmans[x - 1, y] = null;
                Chessmans[x + 1, y].SetPosition(x + 1, y);
                Chessmans[x + 1, y].isMoved = true;

                CastlingMove = (true, true);
            }
            // Queen side (away from (0, 0))
            else
            {
                // Moving Rook2
                Chessmans[x - 1, y] = Chessmans[x + 2, y];
                Chessmans[x + 2, y] = null;
                Chessmans[x - 1, y].SetPosition(x - 1, y);
                Chessmans[x - 1, y].isMoved = true;

                CastlingMove = (true, false);
            }
            // Note : King will move as a chessman by this function later
        }
        #endregion
        // -------ĳ���� Move Manager Over-------

        // Now moving
        // ������ �����̴� �ڵ�

        // �̵� �� ��ġ�� ü������ ���ϴ�.
        Chessmans[chessman.currentX, chessman.currentY] = null;

        // ���ο� ��ġ�� ü������ ��ġ�մϴ�.
        Chessmans[x, y] = chessman;

        // ü������ ��ġ�� ������Ʈ�մϴ�.
        chessman.SetPosition(x, y);

        // ü������ '�̵�����' ���¸� true�� �����մϴ�.
        chessman.isMoved = true;

        // ���� ���¸� ����Ͽ� �����丮 ���ÿ� �����մϴ�.
        State currentState = new State();
        currentState.SetState(movedChessman, capturedChessman, EnPassantStatus, PromotionMove, CastlingMove, depth);
        History.Push(currentState);
    }

    // ���� ���·� �ǵ����� �Լ��Դϴ�.
    private void Undo(int depth)
    {
        // ������ �� ������ ���� ���¸� �����ɴϴ�.
        State currentState = History.Pop();

        // ���� ���̰� ���ÿ��� ������ ������ ���̿� ��ġ�ؾ� �մϴ�.
        if (depth != currentState.depth)
        {
            Debug.Log("Depth not matched!!!");
            return;
        }

        // ���� ���� ������
        // �̵��� ü���ǿ� ���� ����
        var movedChessman = currentState.movedChessman;
        // ���� ü���ǿ� ���� ����
        var capturedChessman = currentState.capturedChessman;
        // ���� ���� EnPassant �̵� ����
        var EnPassantStatus = currentState.EnPassantStatus;
        // ���θ�� �̵��� ���� ���� : (���θ���� �Ǿ��°�), ��(���θ�ǵ� ü����)
        var PromotionMove = currentState.PromotionMove;
        // ĳ���� �̵��� ���� ���� : (ĳ������ �ߴ°�, ŷ ���̵��ΰ� �ƴѰ�)
        var CastlingMove = currentState.CastlingMove;

        // EnPassant �̵� ����
        EnPassant[0] = EnPassantStatus.x;
        EnPassant[1] = EnPassantStatus.y;

        // �̵��� ü������ newPosition���� oldPosition���� �����մϴ�.
        Chessman chessman = movedChessman.chessman;
        chessman.isMoved = movedChessman.isMoved;
        chessman.SetPosition(movedChessman.oldPosition.x, movedChessman.oldPosition.y);
        Chessmans[movedChessman.oldPosition.x, movedChessman.oldPosition.y] = chessman;
        Chessmans[movedChessman.newPosition.x, movedChessman.newPosition.y] = null;

        // ���θ�� �̵� ����
        if (PromotionMove.wasPromotion)
        {
            ActiveChessmans.Remove(PromotionMove.promotedChessman);
            ActiveChessmans.Add(chessman);
        }

        // ���� ���� �ٽ� ���� ��ġ�� �����մϴ�.
        var opponent = capturedChessman;
        if (opponent.chessman != null)
        {
            Chessmans[opponent.Position.x, opponent.Position.y] = opponent.chessman;
            opponent.chessman.SetPosition(opponent.Position.x, opponent.Position.y);
            ActiveChessmans.Add(opponent.chessman);
        }

        // ĳ���� �̵� ����
        if (CastlingMove.wasCastling)
        {
            int x = movedChessman.newPosition.x;
            int y = movedChessman.newPosition.y;
            // ŷ ���̵� (0, 0 ����)
            if (CastlingMove.isKingSide)
            {
                // Rook1 ����
                Chessmans[x - 1, y] = Chessmans[x + 1, y];
                Chessmans[x + 1, y] = null;
                Chessmans[x - 1, y].SetPosition(x - 1, y);
                Chessmans[x - 1, y].isMoved = false;
            }
            // �� ���̵� (0, 0���� �־����� ����)
            else
            {
                // Rook2 ����
                Chessmans[x + 2, y] = Chessmans[x - 1, y];
                Chessmans[x - 1, y] = null;
                Chessmans[x + 2, y].SetPosition(x + 2, y);
                Chessmans[x + 2, y].isMoved = false;
            }
            // ����: ŷ�� �̹� �� �Լ� ������ ü�������μ� ���������ϴ�.
        }
    }

    // �������� �����ִ� �Լ�,
    public void Shuffle(List<Chessman> list)
    {
        System.Random rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Chessman value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // private string printTabs(int num)
    // {
    //     string detail = "";
    //     while(--num > 0) detail = detail + "\t";
    //     return detail;
    // }

    // private string printMoves(bool[,] moves)
    // {
    //     string str = "\n";
    //     for(int i=0; i<8; i++)
    //     {
    //         for(int j=7; j>=0; j--)
    //         {
    //             str = str + (moves[j, i]?1:0) + " ";
    //         }
    //         str = str + "\n";
    //     }
    //     return str;
    // }

    // //to be deleted
    // private void printBoard()
    // {
    //     Chessman[,] Chessmans = BoardManager.Instance.Chessmans;
    //     for(int i=0; i<8; i++)
    //     {
    //         for(int j=7; j>=0; j--)
    //         {
    //             if(Chessmans[j,i] == null)
    //             {
    //                 board = board + "[] ";
    //                 continue;
    //             }

    //             board = board + (Chessmans[j,i].isWhite ? "W":"B");
    //             Chessman chessman = Chessmans[j,i];

    //             if(chessman.GetType() == typeof(King))
    //                 board = board + "K ";
    //             if(chessman.GetType() == typeof(Queen))
    //                 board = board + "Q ";
    //             if(chessman.GetType() == typeof(Rook))
    //                 board = board + "R ";
    //             if(chessman.GetType() == typeof(Bishup))
    //                 board = board + "B ";
    //             if(chessman.GetType() == typeof(Knight))
    //                 board = board + "k ";
    //             if(chessman.GetType() == typeof(Pawn))
    //                 board = board + "P ";
    //         }

    //         board = board + "\n";
    //     }
    // }
}
