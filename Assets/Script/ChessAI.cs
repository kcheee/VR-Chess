using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI : MonoBehaviour
{
    public static ChessAI Instance { set; get; }

    /* ------------------------- 환경 복제 ----------------------*/
    private List<Chessman> ActiveChessmans;
    private Chessman[,] Chessmans;
    private int[] EnPassant;

    // 본래의 Chessmans[,] 배열을 복사해둘 변수
    // (왜냐하면 Chessman 클래스와 그 하위 클래스들이 BoardManager.Instance.Chessmans를 사용하기 때문에)
    // NPC의 다음 이동을 생각하기 전에 이 변수로 복사하고,
    // NPCMove() 함수가 끝나면 다시 복원해야함
    private Chessman[,] ActualChessmansReference;
    private Chessman ActualWhiteKing;
    private Chessman ActualBlackKing;
    private Chessman ActualWhiteRook1;
    private Chessman ActualWhiteRook2;
    private Chessman ActualBlackRook1;
    private Chessman ActualBlackRook2;
    private int[] ActualEnPassant;

    // 상태 이력을 저장할 스택
    private Stack<State> History;

    // 최대 탐색 깊이 (탐색할 미래의 이동 횟수)
    private int maxDepth;

    // NPC가 이동할 체스맨과 이동할 위치
    private Chessman NPCSelectedChessman = null;
    private int moveX = -1;
    private int moveY = -1;
    private int winningValue = 0;

    // 평균 응답 시간 계산을 위한 변수
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
        // 시간 측정.
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        // detail = "Start:\n";
        // board = "Current Actual Board :\n";
        // printBoard();

        // 새로운 상태 이력 스택 생성.
        History = new Stack<State>();

        /* --------------------- 감지 --------------------- */

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

        // 무작위로 섞어주는 함수.
        Shuffle(ActiveChessmans);

        EnPassant = new int[2] { ActualEnPassant[0], ActualEnPassant[0] };

        /* --------------------- Think --------------------- */

        // Critical Part:
        // 중요한 부분:
        // 보드 매니저 스크립트에서 관리되는 게임의 핵심인 Chessmans[,] 배열을 변경할 예정입니다.
        // 이것은 BoardManager 스크립트에서 위치에 따라 보드 위에 있는 모든 체스맨에 대한 포인터를 저장하기 때문입니다.
        // 따라서 이미 실제 Chessmans[,] 배열의 참조를 저장하고 다른 복제본을 할당하고 있습니다.
        // (킹 및 룩, 앙파상 이동 배열에 대해서도 동일하게 적용됩니다)
        BoardManager.Instance.Chessmans = Chessmans;
        BoardManager.Instance.WhiteKing = Chessmans[ActualWhiteKing.currentX, ActualWhiteKing.currentY];
        BoardManager.Instance.BlackKing = Chessmans[ActualBlackKing.currentX, ActualBlackKing.currentY];
        if (ActualWhiteRook1 != null) BoardManager.Instance.WhiteRook1 = Chessmans[ActualWhiteRook1.currentX, ActualWhiteRook1.currentY];
        if (ActualWhiteRook2 != null) BoardManager.Instance.WhiteRook2 = Chessmans[ActualWhiteRook2.currentX, ActualWhiteRook2.currentY];
        if (ActualBlackRook1 != null) BoardManager.Instance.BlackRook1 = Chessmans[ActualBlackRook1.currentX, ActualBlackRook1.currentY];
        if (ActualBlackRook2 != null) BoardManager.Instance.BlackRook2 = Chessmans[ActualBlackRook2.currentX, ActualBlackRook2.currentY];
        BoardManager.Instance.EnPassant = EnPassant;

        // 가장 유리한 이동을 위한 함수.
        Think();

        // Chessmans[,] 배열을 복원함
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
        // 유리한 이동을 위해 선택된 코드.
        // 유리한 이동을 하는 기물 

        // NPC가 선택한 기물이 현재 위치와 타입 출력.
        Debug.Log(NPCSelectedChessman.GetType() + " to (" + moveX + ", " + moveY + ") " + winningValue + "\n"); // remove this line
        BoardManager.Instance.SelectedChessman = BoardManager.Instance.Chessmans[NPCSelectedChessman.currentX, NPCSelectedChessman.currentY];
        BoardManager.Instance.allowedMoves = BoardManager.Instance.SelectedChessman.PossibleMoves();

        // Debug.Log("Moving");
        // 체스를 움직임/
        BoardManager.Instance.MoveChessman(moveX, moveY);

        // 시간 측정 종료 및 결과.
        stopwatch.Stop();
        totalTime += stopwatch.ElapsedMilliseconds;
        totalRun++;

        // 평균 응답시간.
        averageResponseTime = totalTime / totalRun;
    }

    private void Think()
    {
        // 최대 탐색 깊이.
        maxDepth = 4;
        int depth = maxDepth - 1;
        // winningValue = MiniMax(depth, true);
        winningValue = AlphaBeta(depth, true, System.Int32.MinValue, System.Int32.MaxValue);
    }

    #region MinMax Algorithm 지금 사용되진않음.
    // MinMax 알고리즘 여기선 사용되진 않음.
    private int MiniMax(int depth, bool isMax)
    {
        // 만약 최대 깊이에 도달하거나 게임이 종료되었으면
        if (depth == 0 || isGameOver())
        {
            // 현재 보드의 가치를 계산.
            int value = StaticEvaluationFunction();

            return value;
        }

        // string ActiveChessmansDetail = "";

        // If it is max turn(NPC turn : Black)
        // NPC가 최대의 가치를 찾는 턴.
        // AI가 높은 가치의 움직임을 찾기 위해.
        if (isMax)
        {
            int hValue = System.Int32.MinValue;
            // int ind = 0;

            // Get list of all possible moves with their heuristic value
            // For all chessmans
            // 모든 체스맨에 대해 반복.
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                // 흰색(플레이어)이면 넘어감.
                if (chessman.isWhite) continue;

                // 현재 기물의 가능한 이동을 가져옴.
                bool[,] allowedMoves = chessman.PossibleMoves();

                // detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // 모든 가능한 이동에 대해 반복.
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving Black " + chessman.GetType() + " to (" + x + ", " + y + ")");

                            // Critical Section : 
                            // 1) Making the current move to see next possible moves after this move in next calls
                            // 현재 수를 실행하여 보드의 상탤르 변경하고, 이 변경된 상태에서 가능한 다음 수를 확인합니다.

                            // 현재 이동을 수행.
                            Move(chessman, x, y, depth);

                            // 2 ) 이 움직이에 대해 평가 계산.
                            int thisMoveValue = MiniMax(depth - 1, !isMax);

                            if (hValue < thisMoveValue)
                            {
                                hValue = thisMoveValue;

                                // 가장 높은 가치를 가진 이동을 기억함.
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
                            // 현재 이동을 취소하여 현재 상태로 돌아갑니다.
                            Undo(depth);
                        }
                    }
                }
            }

            // if(depth == maxDepth-1) detail += "ActiveChessmans : \n" + ActiveChessmansDetail + "\n";

            return hValue;
        }

        // If it is min turn(Player turn : White)
        // 낮은 가치의 턴 플레이어.
        else
        {
            // 현재 턴의 가치를 초기화
            int hValue = System.Int32.MaxValue;
            // int ind = 0;

            // Get list of all possible moves with their heuristic value
            // For all chessmans
            // 모든 기물에 대한 반복.
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "\n(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                // 검은색(Npc)이면 넘어감.
                if (!chessman.isWhite) continue;

                // 가능한 움직임을 가져옴.
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
                            // 현재 이동을 수행하여 보드의 상태를 변경.
                            Move(chessman, x, y, depth);

                            // 현재 이동의 평가를 계산.
                            int thisMoveValue = MiniMax(depth - 1, !isMax);

                            if (hValue > thisMoveValue)
                            {
                                hValue = thisMoveValue;
                                // The following 6-7 lines are commented, that is suggesting that 
                                // 최소값을 찾는 턴(min turn)에서 NPCSelectedChessman, moveX 및 moveY 값을 업데이트하지 않겠다는 의미
                                // if(depth == maxDepth-1)
                                // {
                                //     NPCSelectedChessman = chessman;
                                //     moveX = x;
                                //     moveY = y;
                                // }
                            }

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // 이동한 움직임 취소
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
        // 깊이가 최대 깊이에 도달했거나 게임이 끝났을 경우.
        if (depth == 0 || isGameOver())
        {
            // 평가 함수.
            int value = StaticEvaluationFunction();

            return value;
        }

        // string ActiveChessmansDetail = "";

        // If it is max turn(NPC turn : Black)
        // Npc의 턴. 
        // Npc는 최대의 가치를 가져야한다.
        if (isMax)
        {
            int hValue = System.Int32.MinValue;
            // int ind = 0;
            // Get list of all possible moves with their heuristic value
            // For all chessmans

            // 모든 체스 기물
            foreach (Chessman chessman in ActiveChessmans.ToArray())
            {
                // ActiveChessmansDetail = ActiveChessmansDetail + "(" + ++ind + ")" + (chessman.isWhite?"White":"Black") + chessman.GetType() + "(" + chessman.currentX + ", " + chessman.currentY + ")" + "\t\t ";

                // 플레이어 체스 기물이면 패스
                if (chessman.isWhite) continue;

                // 기물에 대한 가능한 움직임.
                bool[,] allowedMoves = chessman.PossibleMoves();

                // detail = detail + "(" + ind + ") " + (chessman.isWhite?"White":"Black") + chessman.GetType() + " at (" + chessman.currentX + ", " + chessman.currentY + ") moves :" + printMoves(allowedMoves);

                // For all possible moves
                // 모든 가능한 움직임.
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (allowedMoves[x, y])
                        {
                            // detail = detail + printTabs(maxDepth - depth) + "(" + ind + ") " + " " + (depth + " Moving Black " + chessman.GetType() + " to (" + x + ", " + y + ")");

                            // Critical Section : 
                            // 현재 이동을 수행하여 보드의 상태를 변경합니다.
                            Move(chessman, x, y, depth);

                            // 현재 이동의 평가를 계산합니다.
                            int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // 현재 이동을 취소하여 현재 상태로 돌아갑니다.
                            Undo(depth);

                            if (hValue < thisMoveValue)
                            {
                                hValue = thisMoveValue;

                                // 가장 높은 평가를 기억함.
                                if (depth == maxDepth - 1)
                                {
                                    // Npc가 선택한 기물과 움직임.
                                    NPCSelectedChessman = chessman;
                                    moveX = x;
                                    moveY = y;
                                }
                            }
                            // 알파 값을 업데이트
                            if (hValue > alpha)
                                alpha = hValue;

                            // 베타 컷오프 확인 
                            if (beta <= alpha)
                                break;
                        }
                    }
                    // 베타 컷오프 확인
                    if (beta <= alpha)
                        break;
                }
                // 베타 컷오프 확인
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
                            // 현재 이동을 수행하여 보드의 상태를 변경합니다.
                            Move(chessman, x, y, depth);

                            // 현재 움직임 평가
                            int thisMoveValue = AlphaBeta(depth - 1, !isMax, alpha, beta);

                            // if(depth-1 == 0) detail = detail + " " + thisMoveValue + "\n";
                            // else detail = detail + "\n";

                            // 현재 움직임 취소.
                            Undo(depth);

                            // 가장 낮은 값
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

    // 기물에 대한 평가 함수.
    // 나의 기물과 상대 기물의 남아있는 것들로 평가를 내는 함수이다.
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

            // 해당 기물이 흰색인 경우, 뺴고 아닌경우 더함.
            if (chessman.isWhite)
                TotalScore -= curr;
            else
                TotalScore += curr;
        }
        return TotalScore;
    }


    // CheckMate인지 확인.
    private bool isGameOver()
    {
        // To be implemented
        // 현재 게임 상태의 가치를 가져옴.
        int currScore = StaticEvaluationFunction();

        // 만약 현재 가치가 -290보다 작거나 290보다 크다면, 게임이 종료된 것으로 판단합니다.
        if ((currScore < -290) || (currScore > 290))
            return true;
        return false;
    }

    private void Move(Chessman chessman, int x, int y, int depth)
    {
        // 이동할 체스맨에 대한 정보를 저장합니다.
        // 튜플.
        (Chessman chessman, (int x, int y) oldPosition, (int x, int y) newPosition, bool isMoved) movedChessman;

        // 잡힐 체스맨에 대한 정보를 저장합니다.
        (Chessman chessman, (int x, int y) Position) capturedChessman = (null, (-1, -1));

        // 현재 허용된 '앙파상' 이동 상태를 저장합니다.
        (int x, int y) EnPassantStatus;

        // 프로모션 이동에 관한 정보입니다. (프로모션 여부, 프로모션된 체스맨)
        (bool wasPromotion, Chessman promotedChessman) PromotionMove = (false, null);

        // 캐슬링 이동에 관한 정보입니다. (캐슬링이 이루어졌는지 여부, 킹 사이드 여부)
        (bool wasCastling, bool isKingSide) CastlingMove;

        // 이동된 체스맨에 대한 정보를 저장합니다.
        movedChessman.chessman = chessman; // 이동된 체스맨
        movedChessman.oldPosition = (chessman.currentX, chessman.currentY); // 이동 전 위치, 초기 위치.
        movedChessman.newPosition = (x, y); // 이동 후 위치
        movedChessman.isMoved = chessman.isMoved; // 체스맨의 이동 여부

        EnPassantStatus = (EnPassant[0], EnPassant[1]); // 현재 앙파상 이동 상태 저장



        // 잡기 과정
        Chessman opponent = Chessmans[x, y]; // 이동한 위치에 있는 적 체스맨
        if (opponent != null)
        {
            // 적을 잡는다
            capturedChessman.chessman = opponent; // 잡힌 적 체스맨
            capturedChessman.Position = (x, y); // 잡힌 적 체스맨의 위치

            Chessmans[x, y] = null; // 체스보드에서 해당 위치를 비움
            ActiveChessmans.Remove(opponent); // 적을 활성 체스맨 리스트에서 제거
        }

        // --------------앙파상 move manager--------------
        #region 앙파상 
        // 이건 패스
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
        // -------앙파상 Move Manager Over-------


        // -------캐슬링 Move Manager------------
        #region 캐슬링
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
        // -------캐슬링 Move Manager Over-------

        // Now moving
        // 실제로 움직이는 코드

        // 이동 전 위치의 체스맨을 비웁니다.
        Chessmans[chessman.currentX, chessman.currentY] = null;

        // 새로운 위치에 체스맨을 배치합니다.
        Chessmans[x, y] = chessman;

        // 체스맨의 위치를 업데이트합니다.
        chessman.SetPosition(x, y);

        // 체스맨의 '이동했음' 상태를 true로 설정합니다.
        chessman.isMoved = true;

        // 현재 상태를 기록하여 히스토리 스택에 저장합니다.
        State currentState = new State();
        currentState.SetState(movedChessman, capturedChessman, EnPassantStatus, PromotionMove, CastlingMove, depth);
        History.Push(currentState);
    }

    // 이전 상태로 되돌리기 함수입니다.
    private void Undo(int depth)
    {
        // 스택의 맨 위에서 현재 상태를 가져옵니다.
        State currentState = History.Pop();

        // 현재 깊이가 스택에서 가져온 상태의 깊이와 일치해야 합니다.
        if (depth != currentState.depth)
        {
            Debug.Log("Depth not matched!!!");
            return;
        }

        // 현재 상태 변수들
        // 이동될 체스맨에 관한 정보
        var movedChessman = currentState.movedChessman;
        // 잡힐 체스맨에 관한 정보
        var capturedChessman = currentState.capturedChessman;
        // 현재 허용된 EnPassant 이동 상태
        var EnPassantStatus = currentState.EnPassantStatus;
        // 프로모션 이동에 관한 정보 : (프로모션이 되었는가), 퀸(프로모션된 체스맨)
        var PromotionMove = currentState.PromotionMove;
        // 캐슬링 이동에 관한 정보 : (캐슬링을 했는가, 킹 사이드인가 아닌가)
        var CastlingMove = currentState.CastlingMove;

        // EnPassant 이동 복원
        EnPassant[0] = EnPassantStatus.x;
        EnPassant[1] = EnPassantStatus.y;

        // 이동된 체스맨을 newPosition에서 oldPosition으로 복원합니다.
        Chessman chessman = movedChessman.chessman;
        chessman.isMoved = movedChessman.isMoved;
        chessman.SetPosition(movedChessman.oldPosition.x, movedChessman.oldPosition.y);
        Chessmans[movedChessman.oldPosition.x, movedChessman.oldPosition.y] = chessman;
        Chessmans[movedChessman.newPosition.x, movedChessman.newPosition.y] = null;

        // 프로모션 이동 복원
        if (PromotionMove.wasPromotion)
        {
            ActiveChessmans.Remove(PromotionMove.promotedChessman);
            ActiveChessmans.Add(chessman);
        }

        // 잡힌 말을 다시 원래 위치로 복원합니다.
        var opponent = capturedChessman;
        if (opponent.chessman != null)
        {
            Chessmans[opponent.Position.x, opponent.Position.y] = opponent.chessman;
            opponent.chessman.SetPosition(opponent.Position.x, opponent.Position.y);
            ActiveChessmans.Add(opponent.chessman);
        }

        // 캐슬링 이동 복원
        if (CastlingMove.wasCastling)
        {
            int x = movedChessman.newPosition.x;
            int y = movedChessman.newPosition.y;
            // 킹 사이드 (0, 0 방향)
            if (CastlingMove.isKingSide)
            {
                // Rook1 복원
                Chessmans[x - 1, y] = Chessmans[x + 1, y];
                Chessmans[x + 1, y] = null;
                Chessmans[x - 1, y].SetPosition(x - 1, y);
                Chessmans[x - 1, y].isMoved = false;
            }
            // 퀸 사이드 (0, 0에서 멀어지는 방향)
            else
            {
                // Rook2 복원
                Chessmans[x + 2, y] = Chessmans[x - 1, y];
                Chessmans[x - 1, y] = null;
                Chessmans[x + 2, y].SetPosition(x + 2, y);
                Chessmans[x + 2, y].isMoved = false;
            }
            // 참고: 킹은 이미 이 함수 이전에 체스맨으로서 움직였습니다.
        }
    }

    // 무작위로 섞어주는 함수,
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
