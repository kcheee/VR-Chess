using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class CH_GrabInterable : XRGrabInteractable
{

    // 체스 기물
    Chessman chessman;
    Chessman selectedChessman;
    bool[,] allowedMoves;

    // 오브젝트, 위치값 저장
    GameObject stored_Piece;
    Vector3 stored_rot;
    float stored_x;
    float stored_y;

    // 잡았는지 체크
    //static public bool canGrab =false;
    
    private void Start()
    {
        // 회전값 저장
        stored_rot = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        //canGrab = isSelected;
    }

    protected override void Drop()
    {
        // drop된 위치를 저장후

        base.Drop();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);
        Debug.Log("Drop :  " + x + " : " + y);

        // allowpos를 확인후
        // true면 
        // boardmanager에있는 움직임 실행.
        Debug.Log(selectedChessman);

        if (allowedMoves[x,y])
        {
            Debug.Log("tlfgod");
            // 기존에 있는 오브젝트 삭제.
            //Destroy(stored_Piece);
            BoardManager.Instance.MoveChessman(x,y);
        }
        else
        {
            // false면 
            // 위치 초기화.
            transform.GetComponent<Collider>().enabled = false;
            transform.DORotate(stored_rot, 0.5f);
            transform.DOMove(new Vector3(stored_x, 0, stored_y), 1).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
    // 잡을 떄 호출됌
    protected override void Grab()
    {
        // 잡을 때 호출되는 건가?
        // 그랩할 때 복사된 오브젝트둠


        base.Grab();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);

        selectedChessman = BoardManager.Instance.Chessmans[x, y];
        allowedMoves = selectedChessman.PossibleMoves();
        BoardHighlight.Instance.HighlightPossibleMoves(allowedMoves, false);
        // 위치값 저장.
        stored_x = x * 0.1f;
        stored_y = y * 0.1f;

        stored_Piece = Instantiate(transform.gameObject, transform.position, transform.rotation);
        Debug.Log("Grab :  " + x + " : " + y);
    }

    // 뗄 떄 호출.
    protected override void Detach()
    {
        base.Detach();
        //int x = (int)(transform.position.x * 10);
        //int y = (int)(transform.position.z * 10);
        //Debug.Log("Detach :  " + x + " : " + y);
    }


}
