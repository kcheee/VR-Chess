using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using Unity.VisualScripting;

public class CH_GrabInterable : XRGrabInteractable
{
    // 체스 기물
    Chessman[,] chessman;
    Chessman selectedChessman;
    bool[,] allowedMoves;

    // 오브젝트, 위치값 저장
    GameObject stored_Piece;
    Vector3 stored_rot;
    float stored_x;
    float stored_y;

    // Renderer 변경
    SkinnedMeshRenderer M_translucent;

    // delegate 위치 저장.
    delegate int storedPosdelegat(int a, int b);
    private void Start()
    {
        // 회전값 저장
        stored_rot = transform.eulerAngles;
        M_translucent = transform.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void FixedUpdate()
    {

    }

    protected override void Drop()
    {
        // drop된 위치를 저장후

        BoardManager.canGrab = false;

        base.Drop();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);
        Debug.Log("Drop :  " + x + " : " + y);

        // allowpos를 확인후
        // true면 
        // boardmanager에있는 움직임 실행.
        Debug.Log(selectedChessman);

        if (BoardManager.Instance.allowedMoves[x,y])
        {
            Debug.Log("tlfgod");
            // 기존에 있는 오브젝트 삭제.
            Destroy(transform.gameObject);
            // 보드 매니저의 체스 기물 재정의

            // 움직임.
            BoardManager.Instance.MoveChessman(x,y);
        }
        else
        {
            // false면 
            // 위치 초기화.
            transform.GetComponent<Collider>().enabled = false;
            BoardHighlight.Instance.deleteHighlight();
            transform.DORotate(stored_rot, 0.5f);
            transform.DOMove(new Vector3(stored_x, 0, stored_y), 1).OnComplete(() =>
            {
                // 집은 오브젝트 지우고
                // 복사된 오브젝트에 boardmanager에 재정의 해줘야함.
                // 재정의는 grab에서
            Destroy(gameObject);
            Debug.Log((int)stored_x * 10+ " : " + (int)stored_y * 10);
            });
        }
    }
    // 잡을 떄 호출됌
    protected override void Grab()
    {
        // 잡을 때 호출되는 건가?
        // 그랩할 때 복사된 오브젝트둠

        BoardManager.canGrab = true;
        base.Grab();
        int x = (int)(transform.position.x * 10);
        int y = (int)(transform.position.z * 10);

        stored_Piece = Instantiate(transform.gameObject, transform.position, transform.rotation);
        // 보드 매니저에 체스 기물 재정의
        // 활성화된 체스맨 목록에 추가합니다.
        BoardManager.Instance.Chessmans[x, y] = stored_Piece.GetComponent<Chessman>();
        BoardManager.Instance.Chessmans[x, y].SetPosition(x, y);

        // 현재 오브젝트 리스트에 지우고 복사된 오브젝트에 추가.
        BoardManager.Instance.ActiveChessmans.Remove(gameObject);
        BoardManager.Instance.ActiveChessmans.Add(stored_Piece);

        // boardmanager에서 실행하게 하자.
        // 보드에서 체스 기물을 가져오고 허락된 움직임과 하이라이팅을 띄워준다.
        // - - -
        BoardManager.Instance.SelectedChessman = BoardManager.Instance.Chessmans[x, y];
        BoardManager.Instance.allowedMoves = BoardManager.Instance.SelectedChessman.PossibleMoves();
        BoardHighlight.Instance.HighlightPossibleMoves(BoardManager.Instance.allowedMoves, false);
        // - - -

        //selectedChessman = chessman[x,y];
        //allowedMoves = selectedChessman.PossibleMoves();
        //BoardHighlight.Instance.HighlightPossibleMoves(allowedMoves, false);

        // 위치값 저장.
        stored_x = x * 0.1f;
        stored_y = y * 0.1f;


        Debug.Log("Grab :  " + x + " : " + y);

        Debug.Log(BoardManager.Instance.ActiveChessmans.Count);

        // 반투명 렌더러
        Material newMaterial = Resources.Load("M_Grab")as Material;

        Material[] materials = M_translucent.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = newMaterial; // 새로운 Material을 적용합니다.
        }
        M_translucent.materials = materials;

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
