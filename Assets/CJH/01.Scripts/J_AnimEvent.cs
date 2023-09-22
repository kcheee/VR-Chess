using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_AnimEvent : MonoBehaviour
{
    //�̺�Ʈ�Լ��� �����ϰ�ʹ�
    // Hit, AttackFinished
    //J_AttackTest attackTest;

    J_PieceMove pieceMove;

    // Start is called before the first frame update
    void Awake()
    {
        pieceMove = GetComponentInParent<J_PieceMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void OnAttack_Hit()
    {
        pieceMove.OnAttack_Hit();

    }
    public void OnAttack_Finished()
    {
        pieceMove.OnAttack_Finished();
    }
}
