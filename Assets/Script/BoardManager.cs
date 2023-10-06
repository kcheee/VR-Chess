using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // �̱���
    #region �̱���
    public static BoardManager Instance { set; get; }

    // ��Ҵ��� üũ
    static public bool canGrab = false;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Ÿ�� ������
    // Ÿ�� ������� 1�� ����
    private const float TILE_SIZE = 1.0f;
    // Ÿ���� �߾��� ����� �� ���, ��ġ ����.
    private const float TILE_OFFSET = TILE_SIZE / 2;
    #endregion

    public int ChessCount = 0;
    // ü�� �� 
    public List<GameObject> ChessmanPrefabs;

    // ���� ���� �ִ� ü������ ������Ʈ �ϱ� ���� ü����.
    public List<GameObject> ActiveChessmans;

    // cam
    Camera cam;

    // ü�� �⹰.
    public Chessman[,] Chessmans { set; get; }
    // Currently Selected Chessman
    public Chessman SelectedChessman;

    // ���� ������ 2���� �迭
    public bool[,] allowedMoves;

    // Select�ϱ� ���� x, y ��.
    private int selectionX = -1;
    private int selectionY = -1;

    int outline_selectpieceX;
    int outline_selectpieceY;

    // Turn System
    public bool isWhiteTurn = true;
    public bool move_TurnLimit = false;

    // Turn Move System
    public bool PieceIsMove = false;

    // promotion �ϱ� ���� �⹰�� ������.
    public Chessman WhiteKing;
    public Chessman BlackKing;
    public Chessman WhiteRook1;
    public Chessman WhiteRook2;
    public Chessman BlackRook1;
    public Chessman BlackRook2;

    // ���Ļ��� ���� ������. ( �ϴ� ���ϴ� �ɷ�) 
    public int[] EnPassant { set; get; }

    // ���θ�� ������
    bool promotionscale = false;


    float AdjustDown = 0.1f;
    int AdjustUp = 10;

    private void Start()
    {
        // ü�� �⹰.
        ActiveChessmans = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        cam = Camera.main;
        EnPassant = new int[2] { -1, -1 };
        SpawnAllChessmans();
    }

    // �ʿ��� 
    /*
     1. ���õ� ü���⹰�� �� ��ġ.
     2. ����� ��ġ.
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
                // ���� ���õ� ü������ ���ٸ� ���� �����ؾ� ��
                if (SelectedChessman == null)
                {
                    // Chess�� ����
                    // grab���� ��ü.
                    SelectChessman();

                }
                // �̹� ü������ ���õ� ��� �̵��ؾ� ��
                else
                {
                    // ü���� ������ ������ ��ġ�� renderer�� ����ְ� ������.
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
        // AI turn
        else if (!isWhiteTurn && !move_TurnLimit)
        {
            // NPC�� �������� ����
            ChessAI.Instance.NPCMove();
        }

    }

    // ���콺 Ŭ������ ü�� �⹰ ����.
    void SelectMouseChessman()
    {
        // ī�޶󿡼� ȭ����� ���콺 Ŭ�� ���������� ���̸� ��
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���� ����ĳ��Ʈ�� ���𰡿� �浹�ϸ�
        if (Physics.Raycast(ray, out hit, 25.0f, LayerMask.GetMask("ChessBoard")))
        {
            // ���� �ߴ� ����  Outline �����������.


            // ���õ� ��ġ�� x�� y ��ǥ�� ����Ͽ� ����
            // (hit.point.x, hit.point.z)�� �Ҽ��� �Ʒ� �ڸ��� �ݿø��ؼ� ������ ��ȯ
            selectionX = (int)((hit.point.x + 0.05f) * 10);
            selectionY = (int)((hit.point.z + 0.05f) * 10);
            //Debug.Log(selectionX + " " + selectionY);
            //Debug.Log(hit.point.z);
        }
        else
        {
            // ���� �ƹ� �͵� �浹���� ������ (-1, -1)�� �����Ͽ� ������ ������ ǥ��
            selectionX = -1;
            selectionY = -1;
        }

    }

    //  ü�� �⹰ ������.
    private void SelectChessman()
    {
        Debug.Log(Chessmans[3, 3]);
        // ���� �⹰�� �����̴� �����̶��
        if (move_TurnLimit) return;

        // ���� Ŭ���� Ÿ�Ͽ� ü������ ���ٸ�
        if (Chessmans[selectionX, selectionY] == null) return;

        // ���õ� ü������ ���� ���� �ƴ϶��
        if (Chessmans[selectionX, selectionY].isWhite != isWhiteTurn) return;

        // ���Ƿ� �������� ��.
        // ��������� ������ ü���� ����
        SelectedChessman = Chessmans[selectionX, selectionY];

        // Outline 
        if (isWhiteTurn)
            Chessmans[selectionX, selectionY].outline.enabled = true;

        // ��ġ�� ����.
        outline_selectpieceX = selectionX;
        outline_selectpieceY = selectionY;

        // ���� ������.
        allowedMoves = SelectedChessman.PossibleMoves();

        // ���� ������ UI ����ֱ�.
        // ���߿� ���� �������.
        BoardHighlight.Instance.HighlightPossibleMoves(allowedMoves, !isWhiteTurn);

    }

    // ü�� �⹰ Spawn
    private void SpawnChessman(int index, Vector3 position)
    {
        // ü���� ���� ������Ʈ�� �����ϰ� ��ġ�� ȸ���� �����մϴ�.
        GameObject ChessmanObject = Instantiate(ChessmanPrefabs[index], position, ChessmanPrefabs[index].transform.rotation) as GameObject;

        // ������ ü������ �� ��ũ��Ʈ�� �ڽ����� �����մϴ�.
        ChessmanObject.transform.SetParent(this.transform);


        // Ȱ��ȭ�� ü���� ��Ͽ� �߰��մϴ�.
        ActiveChessmans.Add(ChessmanObject);

        // ������ ü������ ��ġ�� ������ ��ȯ�Ͽ� x�� y�� �����մϴ�.
        int x = (int)(position.x);
        int y = (int)(position.z);

        if (promotionscale)
        {
            ChessmanObject.transform.localScale = ChessmanObject.transform.localScale * 0.1f;
            x = (int)(position.x * 10);
            y = (int)(position.z * 10);
            ChessmanObject.GetComponent<Rigidbody>().isKinematic = true;
        }


        // Chessmans �迭�� ������ ü������ �߰��ϰ� ���� ��ġ�� �����մϴ�.
        Chessmans[x, y] = ChessmanObject.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
    }

    // ���忡 ü�� ����.
    private void SpawnAllChessmans()
    {
        // ���� ������ 0.1f ���������.

        float  Adjustpos = 0.1f;

        #region ����(csv�� �غ���)
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

        //// ���θ�� �׽�Ʈ ��
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
        #endregion  csv�� �غ���

        // ������

        // king
        SpawnChessman(9, new Vector3(3, 0, 0));
        // rook                                          
        //SpawnChessman(6, new Vector3(0, 0, 0));
        SpawnChessman(6, new Vector3(7, 0, 0));

        // ���
        SpawnChessman(8, new Vector3(5, 0, 0));

        // pawn
        SpawnChessman(11, new Vector3(4, 0, 1));
        SpawnChessman(11, new Vector3(5, 0, 1));

        // - - - �츮�� - - -


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

        // ���� �Ŵ������� ���� ü�� �� ����.
        // Ư���� �̵��̳� üũ����Ʈ�� ����.
        WhiteKing = Chessmans[3, 7];
        BlackKing = Chessmans[3, 0];

        //WhiteRook1 = Chessmans[0, 7];
        //WhiteRook2 = Chessmans[7, 7];
        //BlackRook1 = Chessmans[0, 0];
        //BlackRook2 = Chessmans[7, 0];

        // Test ��
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

    // ü�� �⹰ �̵�
    public void MoveChessman(int x, int y)
    {
        // �� �� �ִ� ���̶��.
        if (allowedMoves[x, y])
        {
            Chessman opponent = Chessmans[x, y];

            // ��� ���� ��� �ڵ�.
            if (opponent != null)
            {
                // ��� ���� ����
                ActiveChessmans.Remove(opponent.gameObject);
                // ���߿� �����ϱ� ����.
                deletePiece = opponent;


                // �ٷ� ������������ ���߿� ����.
                //Destroy(opponent.gameObject);
            }

            // -------���Ļ�Ʈ �̵� ����------------
            // ���Ļ�Ʈ �̵��̸� ��� ���� �ı�
            if (EnPassant[0] == x && EnPassant[1] == y && SelectedChessman.GetType() == typeof(Pawn))
            {
                if (isWhiteTurn)
                    opponent = Chessmans[x, y + 1];
                else
                    opponent = Chessmans[x, y - 1];

                ActiveChessmans.Remove(opponent.gameObject);
                Destroy(opponent.gameObject);
            }

            // ���Ļ�Ʈ �̵� �ʱ�ȭ
            EnPassant[0] = EnPassant[1] = -1;

            // Pawn ���θ��.
            if (SelectedChessman.GetType() == typeof(Pawn))
            {
                //-------���θ�� �̵� ����------------
                // AI
                if (y == 7)
                {
                    // �ٷ� �ٲ������� �������� �����ٸ� �ٲ���� ��.
                    // �ڷ�ƾ���� �ٽ� ����������

                    ActiveChessmans.Remove(SelectedChessman.gameObject);
                    Destroy(SelectedChessman.gameObject);
                    // ���� ������ ��ȯ�ϱ� ������ ������ ��ȯ.
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
                //-------���θ�� �̵� ���� ��-------

                // ���Ļ�
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
            // -------���Ļ�Ʈ �̵� ���� ��-------

            // -------ĳ���� �̵� ����------------
            // ĳ�����̶� ���� ŷ�̶� �ٲٴ� �����̴�.
            // ���õ� ü������ ŷ�̰�, �̵� �Ÿ��� 2�� ��� (ĳ����)
            if (SelectedChessman.GetType() == typeof(King) && System.Math.Abs(x - SelectedChessman.currentX) == 2)
            {
                // ŷ �� (0, 0)���� ���� ���
                if (x - SelectedChessman.currentX < 0)
                {
                    // ��1�� �̵�
                    Chessmans[x + 1, y] = Chessmans[x - 1, y];
                    Chessmans[x - 1, y] = null;
                    Chessmans[x + 1, y].SetPosition(x + 1, y);
                    //Chessmans[x + 1, y].transform.position = new Vector3((x + 1)*AdjustDown, 0, y*AdjustDown);

                    StartCoroutine(move(x + 1, y, Chessmans[x + 1, y]));
                    Chessmans[x + 1, y].isMoved = true;
                }
                // �� �� (0, 0)���� ���� ���
                // ����.��
                else
                {
                    // ��2�� �̵�
                    Chessmans[x - 1, y] = Chessmans[x + 2, y];
                    // �� �ڸ��� �ִ� �� �ΰ����� ����.
                    Chessmans[x + 2, y] = null;
                    Chessmans[x - 1, y].SetPosition(x - 1, y);
                    Chessmans[x - 1, y].transform.position = new Vector3((x - 1) * AdjustDown, 0, y * AdjustDown);
                    Chessmans[x - 1, y].isMoved = true;
                }
                // ����: ŷ�� �� �Լ����� ���õ� ü�������μ� �̵��մϴ�.
            }
            // -------ĳ���� �̵� ���� ��-------

            Debug.Log(SelectedChessman);

            // ���θ���� ������ ��ġ �缳�� �̹� �Ǿ� ����.
            if (!ispromotion)
            {

                // ������ �ϴ� ��ġ�� null�̶��
                Chessmans[SelectedChessman.currentX, SelectedChessman.currentY] = null;
                Chessmans[x, y] = SelectedChessman;

                // ���õ� ü���� ��ġ ������Ʈ
                SelectedChessman.SetPosition(x, y);

                //if(SelectedChessman.GetType() != typeof(Pawn)&&!isWhiteTurn)
                //SelectedChessman.transform.position = new Vector3(x, 0, y);

                SelectedChessman.isMoved = true;

                // Promotion �� ��.
                // Outline ������ SelectedChessman ���� ���ְ� return
                if (Chessmans[x, y].isWhite)
                {
                    Debug.Log(Chessmans[x, y]);

                    StartCoroutine(IV_outline(x, y));
                    BoardHighlight.Instance.deleteHighlight();
                }

                // ��� ������ �ѱ�.

                // AI ������.

                // �����̴� �Լ� 
                StartCoroutine(move(x, y, SelectedChessman, true));
            }
            else isWhiteTurn = !isWhiteTurn;

            //isWhiteTurn = !isWhiteTurn;
            SelectedChessman = null;

            // CheckMate 
            isCheckmate();
            // �� ��
            ChessCount++;
            //Debug.Log("�� �� : " + ChessCount);
            return;
        }

        //AI�� ���� �� �Լ��� ���ϱ�.
        // 

        // ü�� �⹰�� ���� Outline ����.
        // �̰͵� ���ľ� �ǳ�.
        if (isWhiteTurn)
        {
            Debug.Log(isWhiteTurn);
            Debug.Log(SelectedChessman);
            SelectedChessman.outline.enabled = false;
            BoardHighlight.Instance.deleteHighlight();
        }

        // ���õ� ü���� ����
        SelectedChessman = null;
    }

    // outline ������
    IEnumerator IV_outline(int x, int y)
    {
        yield return new WaitForSeconds(0.5f);
        //Chessmans[x, y].outline.enabled = false;
    }

    // ü�� �⹰ ������.
    IEnumerator move(int x, int y, Chessman Mchessman, bool condition = false)
    {
        move_TurnLimit = true;
        // �� �ڷ�ƾ �Լ��� �� ����. �������� ���ϰ�
        // �̵� �ϴ� �Լ� 
        Mchessman.Move(x, y, Mchessman.GetType());

        PieceIsMove = true;

        // Move�ϴ� �⹰ ��ũ��Ʈ���� ����
        while (PieceIsMove)
        {
            yield return null;
        }

        // �ο�� ����ϰ� �� ���� ü���⹰�� �������.
        if (deletePiece != null)
        {
            if (deletePiece.GetType() == typeof(King))
            {

                Destroy(deletePiece.gameObject);
                // ���ӿ���.
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
                //Debug.LogError("üũ����Ʈ222");
            }

        }
        // ���� ������ ŷ�� üũ ���� ���
        else
        {
            if (WhiteKing.InDanger())
            {
                CheckMateDelegate.Instance.fade();
                //Debug.LogError("üũ����Ʈ111");
            }
        }

        if (condition)
        {
            isWhiteTurn = !isWhiteTurn;
            move_TurnLimit = false;
        }
        
        
        yield return null;

    }


    // üũ����Ʈ üũ �Լ�.
    private void isCheckmate()
    {

        // �ʱ⿡�� � ü���ǵ� ���� �������� ���ٰ� �����մϴ�.
        bool hasAllowedMove = false;

        // Ȱ��ȭ�� ��� ü������ ��ȸ�մϴ�.
        foreach (GameObject chessman in ActiveChessmans)
        {
            // ���� ü������ ���� �� �÷��̾��� ���� �ƴϸ� �Ѿ�ϴ�.
            if (chessman.GetComponent<Chessman>().isWhite != isWhiteTurn)
                continue;

            // ���� ü������ �� �� �ִ� �������� �����ɴϴ�.
            bool[,] allowedMoves = chessman.GetComponent<Chessman>().PossibleMoves();

            // ��� ��ǥ�� Ȯ���Ͽ� ���� �������� �ִ��� Ȯ���մϴ�.
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (allowedMoves[x, y])
                    {
                        // ���� �������� ������ �÷��׸� true�� �����ϰ� ������ �����մϴ�.
                        hasAllowedMove = true;
                        break;
                    }
                }
                if (hasAllowedMove) break; // �̹� ���� �������� ������ ������ �����մϴ�.
            }
        }

        // ���� � ü���ǵ� ���� �������� ���ٸ� üũ����Ʈ�Դϴ�.
        if (!hasAllowedMove)
        {

            Debug.LogError("üũ����Ʈ");
            // ����� �α׷� üũ����Ʈ���� ����մϴ�.

            // ���� ���� �޴��� ǥ���մϴ�.
            Debug.Log("CheckMate");
            // ���� ���� ������ ȣ���� �� �ֽ��ϴ�. (�ּ� ó���� �ڵ�)
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