using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class CH_GrabInterable : XRGrabInteractable
{

    // ü�� �⹰
    Chessman chessman;
    Chessman selectedChessman;
    bool[,] allowedMoves;

    // ������Ʈ, ��ġ�� ����
    GameObject stored_Piece;
    Vector3 stored_rot;
    float stored_x;
    float stored_y;

    // ��Ҵ��� üũ
    //static public bool canGrab =false;
    
    private void Start()
    {
        // ȸ���� ����
        stored_rot = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        //canGrab = isSelected;
    }

    protected override void Drop()
    {
        // drop�� ��ġ�� ������

        base.Drop();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);
        Debug.Log("Drop :  " + x + " : " + y);

        // allowpos�� Ȯ����
        // true�� 
        // boardmanager���ִ� ������ ����.
        Debug.Log(selectedChessman);

        if (allowedMoves[x,y])
        {
            Debug.Log("tlfgod");
            // ������ �ִ� ������Ʈ ����.
            //Destroy(stored_Piece);
            BoardManager.Instance.MoveChessman(x,y);
        }
        else
        {
            // false�� 
            // ��ġ �ʱ�ȭ.
            transform.GetComponent<Collider>().enabled = false;
            transform.DORotate(stored_rot, 0.5f);
            transform.DOMove(new Vector3(stored_x, 0, stored_y), 1).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
    // ���� �� ȣ���
    protected override void Grab()
    {
        // ���� �� ȣ��Ǵ� �ǰ�?
        // �׷��� �� ����� ������Ʈ��


        base.Grab();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);

        selectedChessman = BoardManager.Instance.Chessmans[x, y];
        allowedMoves = selectedChessman.PossibleMoves();
        BoardHighlight.Instance.HighlightPossibleMoves(allowedMoves, false);
        // ��ġ�� ����.
        stored_x = x * 0.1f;
        stored_y = y * 0.1f;

        stored_Piece = Instantiate(transform.gameObject, transform.position, transform.rotation);
        Debug.Log("Grab :  " + x + " : " + y);
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
