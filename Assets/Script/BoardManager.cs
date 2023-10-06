using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // 싱글톤
    #region 싱글톤
    public static BoardManager Instance { set; get; }

    // 잡았는지 체크
    static public bool canGrab = false;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region 타일 사이즈
    // 타일 사이즈는 1로 고정
    private const float TILE_SIZE = 1.0f;
    // 타일의 중앙을 계산할 때 사용, 위치 보정.
    private const float TILE_OFFSET = TILE_SIZE / 2;
    #endregion

    public int ChessCount = 0;
    // 체스 말 
    public List<GameObject> ChessmanPrefabs;

    // 보드 위에 있는 체스말들 업데이트 하기 위한 체스말.
    public List<GameObject> ActiveChessmans;

    // cam
    Camera cam;

    // 체스 기물.
    public Chessman[,] Chessmans { set; get; }
    // Currently Selected Chessman
    public Chessman SelectedChessman;

    // 허용된 움직임 2차원 배열
    public bool[,] allowedMoves;

    // Select하기 위한 x, y 값.
    private int selectionX = -1;
    private int selectionY = -1;

    int outline_selectpieceX;
    int outline_selectpieceY;

    // Turn System
    public bool isWhiteTurn = true;
    public bool move_TurnLimit = false;

    // Turn Move System
    public bool PieceIsMove = false;

    // promotion 하기 위해 기물들 가져옴.
    public Chessman WhiteKing;
    public Chessman BlackKing;
    public Chessman WhiteRook1;
    public Chessman WhiteRook2;
    public Chessman BlackRook1;
    public Chessman BlackRook2;

    // 앙파상을 위한 움직임. ( 일단 안하는 걸로) 
    public int[] EnPassant { set; get; }

    // 프로모션 스케일
    bool promotionscale = false;


    float AdjustDown = 0.1f;
    int AdjustUp = 10;

    private void Start()
    {
        // 체스 기물.
        ActiveChessmans = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        cam = Camera.main;
        EnPassant = new int[2] { -1, -1 };
        SpawnAllChessmans();
    }

    // 필요요소 
    /*
     1. 선택된 체스기물과 그 위치.
     2. 드랍된 위치.
     */

    private void Update()
    {
        // test
        //if (canGrab) Debug.Log(SelectedChessman);

        SelectMouseChessman();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0 && selectionX <= 0.7f && selectionY <= 0.7f)
            {
                // 만약 선택된 체스맨이 없다면 먼저 선택해야 함
                if (SelectedChessman == null)
                {
                    // Chess를 선택
                    // grab으로 대체.
                    SelectChessman();

                }
                // 이미 체스맨이 선택된 경우 이동해야 함
                else
                {
                    // 체스의 움직임 가능한 위치를 renderer에 띄워주고 움직임.
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
        // AI turn
        else if (!isWhiteTurn && !move_TurnLimit)
        {
            // NPC가 움직임을 수행
            ChessAI.Instance.NPCMove();
        }

    }

    // 마우스 클릭으로 체스 기물 선택.
    void SelectMouseChessman()
    {
        // 카메라에서 화면상의 마우스 클릭 지점까지의 레이를 쏨
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 만약 레이캐스트가 무언가와 충돌하면
        if (Physics.Raycast(ray, out hit, 25.0f, LayerMask.GetMask("ChessBoard")))
        {
            // 전에 했던 값들  Outline 설정해줘야함.


            // 선택된 위치의 x와 y 좌표를 계산하여 저장
            // (hit.point.x, hit.point.z)의 소수점 아래 자리를 반올림해서 정수로 변환
            selectionX = (int)((hit.point.x + 0.05f) * 10);
            selectionY = (int)((hit.point.z + 0.05f) * 10);
            //Debug.Log(selectionX + " " + selectionY);
            //Debug.Log(hit.point.z);
        }
        else
        {
            // 만약 아무 것도 충돌하지 않으면 (-1, -1)로 설정하여 선택이 없음을 표시
            selectionX = -1;
            selectionY = -1;
        }

    }

    //  체스 기물 선택함.
    private void SelectChessman()
    {
        Debug.Log(Chessmans[3, 3]);
        // 만약 기물이 움직이는 도중이라면
        if (move_TurnLimit) return;

        // 만약 클릭한 타일에 체스맨이 없다면
        if (Chessmans[selectionX, selectionY] == null) return;

        // 선택된 체스맨의 팀의 턴이 아니라면
        if (Chessmans[selectionX, selectionY].isWhite != isWhiteTurn) return;

        // 임의로 렌더러로 함.
        // 노란색으로 강조된 체스맨 선택
        SelectedChessman = Chessmans[selectionX, selectionY];

        // Outline 
        if (isWhiteTurn)
            Chessmans[selectionX, selectionY].outline.enabled = true;

        // 위치값 저장.
        outline_selectpieceX = selectionX;
        outline_selectpieceY = selectionY;

        // 허용된 움직임.
        allowedMoves = SelectedChessman.PossibleMoves();

        // 허용된 움직임 UI 띄워주기.
        // 나중에 설정 해줘야함.
        BoardHighlight.Instance.HighlightPossibleMoves(allowedMoves, !isWhiteTurn);

    }

    // 체스 기물 Spawn
    private void SpawnChessman(int index, Vector3 position)
    {
        // 체스맨 게임 오브젝트를 생성하고 위치와 회전을 설정합니다.
        GameObject ChessmanObject = Instantiate(ChessmanPrefabs[index], position, ChessmanPrefabs[index].transform.rotation) as GameObject;

        // 생성된 체스맨을 이 스크립트의 자식으로 설정합니다.
        ChessmanObject.transform.SetParent(this.transform);


        // 활성화된 체스맨 목록에 추가합니다.
        ActiveChessmans.Add(ChessmanObject);

        // 생성된 체스맨의 위치를 정수로 변환하여 x와 y에 저장합니다.
        int x = (int)(position.x);
        int y = (int)(position.z);

        if (promotionscale)
        {
            ChessmanObject.transform.localScale = ChessmanObject.transform.localScale * 0.1f;
            x = (int)(position.x * 10);
            y = (int)(position.z * 10);
            ChessmanObject.GetComponent<Rigidbody>().isKinematic = true;
        }


        // Chessmans 배열에 생성된 체스맨을 추가하고 현재 위치를 설정합니다.
        Chessmans[x, y] = ChessmanObject.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
    }

    // 보드에 체스 스폰.
    private void SpawnAllChessmans()
    {
        // 보정 값으로 0.1f 더해줘야함.

        float  Adjustpos = 0.1f;

        #region 기존(csv로 해보기)
        //// Spawn White Pieces
        //// Rook1
        //SpawnChessman(0, new Vector3(0 * Adjustpos, 0, 7 * Adjustpos));

        //SpawnChessman(1, new Vector3(1 * Adjustpos, 0, 7 * Adjustpos));

        //SpawnChessman(2, new Vector3(2 * Adjustpos, 0, 7 * Adjustpos));

        //SpawnChessman(3, new Vector3(3 * Adjustpos, 0, 7 * Adjustpos));

        //SpawnChessman(4, new Vector3(4 * Adjustpos, 0, 7 * Adjustpos));
        //// Bishop2                     * Adjustpos       * Adjustpos
        //SpawnChessman(2, new Vector3(5 * Adjustpos, 0, 7 * Adjustpos));
        //// Knight2                     * Adjustpos       * Adjustpos
        //SpawnChessman(1, new Vector3(6 * Adjustpos, 0, 7 * Adjustpos));
        //// Rook2                       * Adjustpos       * Adjustpos
        //SpawnChessman(0, new Vector3(7 * Adjustpos, 0, 7 * Adjustpos));

        //// Pawns
        //for (int i = 0; i < 8; i++)
        //{
        //    SpawnChessman(5, new Vector3(i * Adjustpos, 0, 6 * Adjustpos));
        //}

        //// 프로모션 테스트 용
        ////SpawnChessman(5, new Vector3(7, 0, 2));

        //// Spawn Black Pieces
        //// Rook1
        //SpawnChessman(6, new Vector3(0* Adjustpos, 0, 0* Adjustpos));
        //// Knight1                    * Adjustpos      * Adjustpos
        //SpawnChessman(7, new Vector3(1* Adjustpos, 0, 0* Adjustpos));
        //// Bishop1                    * Adjustpos      * Adjustpos
        //SpawnChessman(8, new Vector3(2* Adjustpos, 0, 0 * Adjustpos));
        //// King                       * Adjustpos
        //SpawnChessman(9, new Vector3(3* Adjustpos, 0, 0 * Adjustpos));
        //// Queen
        //SpawnChessman(10, new Vector3(4 * Adjustpos, 0, 0 * Adjustpos));
        //// Bishop2
        //SpawnChessman(8, new Vector3(5 * Adjustpos, 0, 0));
        //// Knight2
        //SpawnChessman(7, new Vector3(6 * Adjustpos, 0, 0));
        //// Rook2
        //SpawnChessman(6, new Vector3(7 * Adjustpos, 0, 0));

        //// Pawns
        //for (int i = 0; i < 8; i++)
        //{
        //    SpawnChessman(11, new Vector3(i * Adjustpos, 0, 1 * Adjustpos));
        //}
        #endregion  csv로 해보기

        // 퍼즐형

        // king
        SpawnChessman(9, new Vector3(3, 0, 0));
        // rook                                          
        //SpawnChessman(6, new Vector3(0, 0, 0));
        SpawnChessman(6, new Vector3(7, 0, 0));

        // 비숍
        SpawnChessman(8, new Vector3(5, 0, 0));

        // pawn
        SpawnChessman(11, new Vector3(4, 0, 1));
        SpawnChessman(11, new Vector3(5, 0, 1));

        // - - - 우리팀 - - -


        // pawn
        SpawnChessman(5, new Vector3(2, 0, 6));
        SpawnChessman(5, new Vector3(5, 0, 6));
        SpawnChessman(5, new Vector3(6, 0, 3));

        //SpawnChessman(5, new Vector3(2, 0, 1));

        SpawnChessman(5, new Vector3(7, 0, 6));

        SpawnChessman(0, new Vector3(0, 0, 7));
        SpawnChessman(0, new Vector3(7, 0, 7));


        SpawnChessman(1, new Vector3(0, 0, 4));

        SpawnChessman(2, new Vector3(1, 0, 4));

        SpawnChessman(3, new Vector3(3, 0, 7));

        SpawnChessman(4, new Vector3(4, 0, 3));

        // 보드 매니저에서 따로 체스 말 관리.
        // 특수한 이동이나 체크메이트를 위해.
        WhiteKing = Chessmans[3, 7];
        BlackKing = Chessmans[3, 0];

        //WhiteRook1 = Chessmans[0, 7];
        //WhiteRook2 = Chessmans[7, 7];
        //BlackRook1 = Chessmans[0, 0];
        //BlackRook2 = Chessmans[7, 0];

        // Test 용
        //WhiteKing = null;
        //BlackKing = null;

        WhiteRook1 = Chessmans[0, 7];
        WhiteRook2 = null;
        BlackRook1 = null;
        BlackRook2 = null;

        Debug.Log(Chessmans[3, 7]);
        Debug.Log(Chessmans[0, 7]);

    }

    bool ispromotion = false;
    public Chessman deletePiece = null;

    // 체스 기물 이동
    public void MoveChessman(int x, int y)
    {
        // 갈 수 있는 곳이라면.
        if (allowedMoves[x, y])
        {
            Chessman opponent = Chessmans[x, y];

            // 상대 말을 잡는 코드.
            if (opponent != null)
            {
                // 상대 말을 잡음
                ActiveChessmans.Remove(opponent.gameObject);
                // 나중에 제거하기 위해.
                deletePiece = opponent;


                // 바로 삭제하지말고 나중에 삭제.
                //Destroy(opponent.gameObject);
            }

            // -------앙파상트 이동 관리------------
            // 앙파상트 이동이면 상대 말을 파괴
            if (EnPassant[0] == x && EnPassant[1] == y && SelectedChessman.GetType() == typeof(Pawn))
            {
                if (isWhiteTurn)
                    opponent = Chessmans[x, y + 1];
                else
                    opponent = Chessmans[x, y - 1];

                ActiveChessmans.Remove(opponent.gameObject);
                Destroy(opponent.gameObject);
            }

            // 앙파상트 이동 초기화
            EnPassant[0] = EnPassant[1] = -1;

            // Pawn 프로모션.
            if (SelectedChessman.GetType() == typeof(Pawn))
            {
                //-------프로모션 이동 관리------------
                // AI
                if (y == 7)
                {
                    // 바로 바꾸지말고 움직임이 끝났다면 바꿔줘야 함.
                    // 코루틴으로 다시 설정해주자

                    ActiveChessmans.Remove(SelectedChessman.gameObject);
                    Destroy(SelectedChessman.gameObject);
                    // 보통 퀸으로 소환하기 때문에 퀸으로 소환.
                    SpawnChessman(10, new Vector3(x, 0, y));
                    SelectedChessman = Chessmans[x, y];

                }

                // Player
                if (y == 0)
                {
                    promotionscale = true;
                    ActiveChessmans.Remove(SelectedChessman.gameObject);
                    Destroy(SelectedChessman.gameObject);
                    SpawnChessman(4, new Vector3(x * 0.1f, 0, y * 0.1f));
                    SelectedChessman = Chessmans[x, y];
                    ispromotion = true;
                }
                //-------프로모션 이동 관리 끝-------

                // 앙파상
                if (SelectedChessman.currentY == 1 && y == 3)
                {
                    EnPassant[0] = x;
                    EnPassant[1] = y - 1;
                }
                if (SelectedChessman.currentY == 6 && y == 4)
                {
                    EnPassant[0] = x;
                    EnPassant[1] = y + 1;
                }
            }
            // -------앙파상트 이동 관리 끝-------

            // -------캐슬링 이동 관리------------
            // 캐슬링이란 룩을 킹이랑 바꾸는 행위이다.
            // 선택된 체스맨이 킹이고, 이동 거리가 2일 경우 (캐슬링)
            if (SelectedChessman.GetType() == typeof(King) && System.Math.Abs(x - SelectedChessman.currentX) == 2)
            {
                // 킹 쪽 (0, 0)으로 가는 경우
                if (x - SelectedChessman.currentX < 0)
                {
                    // 룩1을 이동
                    Chessmans[x + 1, y] = Chessmans[x - 1, y];
                    Chessmans[x - 1, y] = null;
                    Chessmans[x + 1, y].SetPosition(x + 1, y);
                    //Chessmans[x + 1, y].transform.position = new Vector3((x + 1)*AdjustDown, 0, y*AdjustDown);

                    StartCoroutine(move(x + 1, y, Chessmans[x + 1, y]));
                    Chessmans[x + 1, y].isMoved = true;
                }
                // 퀸 쪽 (0, 0)으로 가는 경우
                // 생략.ㄴ
                else
                {
                    // 룩2을 이동
                    Chessmans[x - 1, y] = Chessmans[x + 2, y];
                    // 그 자리에 있던 룩 널값으로 설정.
                    Chessmans[x + 2, y] = null;
                    Chessmans[x - 1, y].SetPosition(x - 1, y);
                    Chessmans[x - 1, y].transform.position = new Vector3((x - 1) * AdjustDown, 0, y * AdjustDown);
                    Chessmans[x - 1, y].isMoved = true;
                }
                // 주의: 킹은 이 함수에서 선택된 체스맨으로서 이동합니다.
            }
            // -------캐슬링 이동 관리 끝-------

            Debug.Log(SelectedChessman);

            // 프로모션을 했으면 위치 재설정 이미 되어 있음.
            if (!ispromotion)
            {

                // 가고자 하는 위치가 null이라면
                Chessmans[SelectedChessman.currentX, SelectedChessman.currentY] = null;
                Chessmans[x, y] = SelectedChessman;

                // 선택된 체스말 위치 업데이트
                SelectedChessman.SetPosition(x, y);

                //if(SelectedChessman.GetType() != typeof(Pawn)&&!isWhiteTurn)
                //SelectedChessman.transform.position = new Vector3(x, 0, y);

                SelectedChessman.isMoved = true;

                // Promotion 할 때.
                // Outline 해제와 SelectedChessman 해제 해주고 return
                if (Chessmans[x, y].isWhite)
                {
                    Debug.Log(Chessmans[x, y]);

                    StartCoroutine(IV_outline(x, y));
                    BoardHighlight.Instance.deleteHighlight();
                }

                // 상대 턴으로 넘김.

                // AI 턴제한.

                // 움직이는 함수 
                StartCoroutine(move(x, y, SelectedChessman, true));
            }
            else isWhiteTurn = !isWhiteTurn;

            //isWhiteTurn = !isWhiteTurn;
            SelectedChessman = null;

            // CheckMate 
            isCheckmate();
            // 턴 수
            ChessCount++;
            //Debug.Log("턴 수 : " + ChessCount);
            return;
        }

        //AI도 같이 이 함수를 쓰니깐.
        // 

        // 체스 기물에 대한 Outline 해제.
        // 이것도 고쳐야 되네.
        if (isWhiteTurn)
        {
            Debug.Log(isWhiteTurn);
            Debug.Log(SelectedChessman);
            SelectedChessman.outline.enabled = false;
            BoardHighlight.Instance.deleteHighlight();
        }

        // 선택된 체스맨 해제
        SelectedChessman = null;
    }

    // outline 딜레이
    IEnumerator IV_outline(int x, int y)
    {
        yield return new WaitForSeconds(0.5f);
        //Chessmans[x, y].outline.enabled = false;
    }

    // 체스 기물 움직임.
    IEnumerator move(int x, int y, Chessman Mchessman, bool condition = false)
    {
        move_TurnLimit = true;
        // 이 코루틴 함수가 될 동안. 선택하지 못하게
        // 이동 하는 함수 
        Mchessman.Move(x, y, Mchessman.GetType());

        PieceIsMove = true;

        // Move하는 기물 스크립트에서 조종
        while (PieceIsMove)
        {
            yield return null;
        }

        // 싸우는 모션하고 그 다음 체스기물이 사라지게.
        if (deletePiece != null)
        {
            if (deletePiece.GetType() == typeof(King))
            {

                Destroy(deletePiece.gameObject);
                // 게임오버.
                EndGame();
            }
            Destroy(deletePiece.gameObject);
            deletePiece = null;
        }

        if (isWhiteTurn)
        {
            if (BlackKing.InDanger())
            {
                CheckMateDelegate.Instance.fade();
                //Debug.LogError("체크메이트222");
            }

        }
        // 현재 검은색 킹이 체크 중인 경우
        else
        {
            if (WhiteKing.InDanger())
            {
                CheckMateDelegate.Instance.fade();
                //Debug.LogError("체크메이트111");
            }
        }

        if (condition)
        {
            isWhiteTurn = !isWhiteTurn;
            move_TurnLimit = false;
        }
        
        
        yield return null;

    }


    // 체크메이트 체크 함수.
    private void isCheckmate()
    {

        // 초기에는 어떤 체스맨도 허용된 움직임이 없다고 가정합니다.
        bool hasAllowedMove = false;

        // 활성화된 모든 체스맨을 순회합니다.
        foreach (GameObject chessman in ActiveChessmans)
        {
            // 현재 체스맨이 현재 턴 플레이어의 것이 아니면 넘어갑니다.
            if (chessman.GetComponent<Chessman>().isWhite != isWhiteTurn)
                continue;

            // 현재 체스맨이 할 수 있는 움직임을 가져옵니다.
            bool[,] allowedMoves = chessman.GetComponent<Chessman>().PossibleMoves();

            // 모든 좌표를 확인하여 허용된 움직임이 있는지 확인합니다.
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (allowedMoves[x, y])
                    {
                        // 허용된 움직임이 있으면 플래그를 true로 설정하고 루프를 종료합니다.
                        hasAllowedMove = true;
                        break;
                    }
                }
                if (hasAllowedMove) break; // 이미 허용된 움직임이 있으면 루프를 종료합니다.
            }
        }

        // 만약 어떤 체스맨도 허용된 움직임이 없다면 체크메이트입니다.
        if (!hasAllowedMove)
        {

            Debug.LogError("체크메이트");
            // 디버그 로그로 체크메이트임을 출력합니다.

            // 게임 오버 메뉴를 표시합니다.
            Debug.Log("CheckMate");
            // 게임 종료 로직을 호출할 수 있습니다. (주석 처리된 코드)
            EndGame();
        }
    }

    void EndGame()
    {
        if (isWhiteTurn)
        {
            Debug.Log("PlayerWin");
            GameManager.instance.Winner();
        }
        else Debug.Log("AI Win");
        Instance.enabled = false;
    }
}