using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // �̱���
    #region �̱���
    public static BoardManager Instance { set; get; }

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

    // ü�� �� 
    public List<GameObject> ChessmanPrefabs;

    // ���� ���� �ִ� ü������ ������Ʈ �ϱ� ���� ü����.
    private List<GameObject> ActiveChessmans;

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

    public Chessman WhiteKing;
    public Chessman BlackKing;

    private void Start()
    {
        // ü�� �⹰.
        ActiveChessmans = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        cam = Camera.main;
        SpawnAllChessmans();
    }

    private void Update()
    {
        SelectMouseChessman();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0 && selectionX <= 7 && selectionY <= 7)
            {
                // ���� ���õ� ü������ ���ٸ� ���� �����ؾ� ��
                if (SelectedChessman == null)
                {
                    // Chess�� ����
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
        else if (!isWhiteTurn)
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
            selectionX = (int)(hit.point.x + 0.5f);
            selectionY = (int)(hit.point.z + 0.5f);

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

        // ���� Ŭ���� Ÿ�Ͽ� ü������ ���ٸ�
        if (Chessmans[selectionX, selectionY] == null) return;

        // ���õ� ü������ ���� ���� �ƴ϶��
        if (Chessmans[selectionX, selectionY].isWhite != isWhiteTurn) return;

        // ���Ƿ� �������� ��.
        // ��������� ������ ü���� ����
        SelectedChessman = Chessmans[selectionX, selectionY];

        // Outline 
        Chessmans[selectionX, selectionY].outline.enabled = true;

        // ��ġ�� ����.
        outline_selectpieceX = selectionX;
        outline_selectpieceY = selectionY;

        // ���� ������.
        allowedMoves = SelectedChessman.PossibleMoves();

        // ���� ������ UI ����ֱ�.
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

        // Chessmans �迭�� ������ ü������ �߰��ϰ� ���� ��ġ�� �����մϴ�.
        Chessmans[x, y] = ChessmanObject.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
    }

    // ���忡 ü�� ����.
    private void SpawnAllChessmans()
    {
        // Spawn White Pieces
        // Rook1
        SpawnChessman(0, new Vector3(0, 0, 7));
        // Knight1
        SpawnChessman(1, new Vector3(1, 0, 7));
        // Bishop1
        SpawnChessman(2, new Vector3(2, 0, 7));
        // King
        SpawnChessman(3, new Vector3(3, 0, 7));
        // Queen
        SpawnChessman(4, new Vector3(4, 0, 7));
        // Bishop2
        SpawnChessman(2, new Vector3(5, 0, 7));
        // Knight2
        SpawnChessman(1, new Vector3(6, 0, 7));
        // Rook2
        SpawnChessman(0, new Vector3(7, 0, 7));

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, new Vector3(i, 0, 6));
        }

        // Spawn Black Pieces
        // Rook1
        SpawnChessman(6, new Vector3(0, 0, 0));
        // Knight1
        SpawnChessman(7, new Vector3(1, 0, 0));
        // Bishop1
        SpawnChessman(8, new Vector3(2, 0, 0));
        // King
        SpawnChessman(9, new Vector3(3, 0, 0));
        // Queen
        SpawnChessman(10, new Vector3(4, 0, 0));
        // Bishop2
        SpawnChessman(8, new Vector3(5, 0, 0));
        // Knight2
        SpawnChessman(7, new Vector3(6, 0, 0));
        // Rook2
        SpawnChessman(6, new Vector3(7, 0, 0));

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, new Vector3(i, 0, 1));
        }
    }

    bool ispromotion = false;
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
                Destroy(opponent.gameObject);
            }

            // Pawn ���θ��.
            if (SelectedChessman.GetType() == typeof(Pawn))
            {

                //-------���θ�� �̵� ����------------
                // AI
                if (y == 7)
                {
                    ActiveChessmans.Remove(SelectedChessman.gameObject);
                    Destroy(SelectedChessman.gameObject);
                    // ���� ������ ��ȯ�ϱ� ������ ������ ��ȯ.
                    SpawnChessman(10, new Vector3(x, 0, y));
                    SelectedChessman = Chessmans[x, y];

                }
                // Player
                if (y == 0)
                {
                    ActiveChessmans.Remove(SelectedChessman.gameObject);
                    Destroy(SelectedChessman.gameObject);
                    SpawnChessman(4, new Vector3(x, 0, y));
                    SelectedChessman = Chessmans[x, y];

                    ispromotion = true;
                }
                //-------���θ�� �̵� ���� ��-------
            }

            // ������ �ϴ� ��ġ�� null�̶��
            Chessmans[SelectedChessman.currentX, SelectedChessman.currentY] = null;
            Chessmans[x, y] = SelectedChessman;

            // ���õ� ü���� ��ġ ������Ʈ
            SelectedChessman.SetPosition(x, y);

            SelectedChessman.transform.position = new Vector3(x, 0, y);
            SelectedChessman.isMoved = true;

            // ��� ������ �ѱ�.
            //isWhiteTurn = !isWhiteTurn;

            // Outline ������ SelectedChessman ���� ���ְ� return
            if(!ispromotion)
            Chessmans[x, y].outline.enabled = false;
            else ispromotion = false;
            SelectedChessman = null;
            Debug.Log(ispromotion);
            return;
        }

        // ü�� �⹰�� ���� Outline ����.
        if (Chessmans[outline_selectpieceX, outline_selectpieceY].GetComponent<Outline>() != null)
            Chessmans[outline_selectpieceX, outline_selectpieceY].outline.enabled = false;
        // ���õ� ü���� ����
        SelectedChessman = null;

        isWhiteTurn = !isWhiteTurn;
    }
}