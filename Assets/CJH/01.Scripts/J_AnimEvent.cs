using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_AnimEvent : MonoBehaviour
{
    //이벤트함수를 제작하고싶다
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
