using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using Unity.VisualScripting;

public class CH_GrabInterable : XRGrabInteractable
{
    // ü�� �⹰
    Chessman[,] chessman;
    Chessman selectedChessman;
    bool[,] allowedMoves;

    // ������Ʈ, ��ġ�� ����
    GameObject stored_Piece;
    Vector3 stored_rot;
    float stored_x;
    float stored_y;

    // Renderer ����
    SkinnedMeshRenderer M_translucent;

    // delegate ��ġ ����.
    delegate int storedPosdelegat(int a, int b);
    private void Start()
    {
        // ȸ���� ����
        stored_rot = transform.eulerAngles;
        M_translucent = transform.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void FixedUpdate()
    {

    }

    protected override void Drop()
    {
        // drop�� ��ġ�� ������

        BoardManager.canGrab = false;

        base.Drop();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);
        Debug.Log("Drop :  " + x + " : " + y);

        // allowpos�� Ȯ����
        // true�� 
        // boardmanager���ִ� ������ ����.
        Debug.Log(selectedChessman);

        if (BoardManager.Instance.allowedMoves[x,y])
        {
            Debug.Log("tlfgod");
            // ������ �ִ� ������Ʈ ����.
            Destroy(transform.gameObject);
            // ���� �Ŵ����� ü�� �⹰ ������

            // ������.
            BoardManager.Instance.MoveChessman(x,y);
        }
        else
        {
            // false�� 
            // ��ġ �ʱ�ȭ.
            transform.GetComponent<Collider>().enabled = false;
            BoardHighlight.Instance.deleteHighlight();
            transform.DORotate(stored_rot, 0.5f);
            transform.DOMove(new Vector3(stored_x, 0, stored_y), 1).OnComplete(() =>
            {
                // ���� ������Ʈ �����
                // ����� ������Ʈ�� boardmanager�� ������ �������.
                // �����Ǵ� grab����
            Destroy(gameObject);
            Debug.Log((int)stored_x * 10+ " : " + (int)stored_y * 10);
            });
        }
    }
    // ���� �� ȣ���
    protected override void Grab()
    {
        // ���� �� ȣ��Ǵ� �ǰ�?
        // �׷��� �� ����� ������Ʈ��

        BoardManager.canGrab = true;
        base.Grab();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);

        stored_Piece = Instantiate(transform.gameObject, transform.position, transform.rotation);
        // ���� �Ŵ����� ü�� �⹰ ������
        // Ȱ��ȭ�� ü���� ��Ͽ� �߰��մϴ�.
        BoardManager.Instance.Chessmans[x, y] = stored_Piece.GetComponent<Chessman>();
        BoardManager.Instance.Chessmans[x, y].SetPosition(x, y);

        // ���� ������Ʈ ����Ʈ�� ����� ����� ������Ʈ�� �߰�.
        BoardManager.Instance.ActiveChessmans.Remove(gameObject);
        BoardManager.Instance.ActiveChessmans.Add(stored_Piece);

        // boardmanager���� �����ϰ� ����.
        // ���忡�� ü�� �⹰�� �������� ����� �����Ӱ� ���̶������� ����ش�.
        // - - -
        BoardManager.Instance.SelectedChessman = BoardManager.Instance.Chessmans[x, y];
        BoardManager.Instance.allowedMoves = BoardManager.Instance.SelectedChessman.PossibleMoves();
        BoardHighlight.Instance.HighlightPossibleMoves(BoardManager.Instance.allowedMoves, false);
        // - - -

        //selectedChessman = chessman[x,y];
        //allowedMoves = selectedChessman.PossibleMoves();
        //BoardHighlight.Instance.HighlightPossibleMoves(allowedMoves, false);

        // ��ġ�� ����.
        stored_x = x * 0.1f;
        stored_y = y * 0.1f;


        Debug.Log("Grab :  " + x + " : " + y);

        Debug.Log(BoardManager.Instance.ActiveChessmans.Count);

        // ������ ������
        Material newMaterial = Resources.Load("M_Grab")as Material;

        Material[] materials = M_translucent.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = newMaterial; // ���ο� Material�� �����մϴ�.
        }
        M_translucent.materials = materials;

    }

    // �� �� ȣ��.
    protected override void Detach()
    {
        base.Detach();
        //int x = (int)(transform.position.x * 10);
        //int y = (int)(transform.position.z * 10);
        //Debug.Log("Detach :  " + x + " : " + y);
    }


}
