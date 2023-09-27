using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_AnimEvent : MonoBehaviour
{
    //이벤트함수를 제작하고싶다
    // Hit, AttackFinished
    //J_AttackTest attackTest;
    J_PieceMove pieceMove;

    //사운드
    public AudioSource audioSource;
    //public AudioClip attackSound;
    public AudioClip[] attackSound;

    public enum ChessType //체스 종류
    {
        KING, QUEEN, BISHOP, KNIGHT, ROOK, PAWN,
    }
    public ChessType chessType;
    // Start is called before the first frame update
    void Awake()
    {
        pieceMove = GetComponentInParent<J_PieceMove>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void OnAttack_Hit()
    {
        pieceMove.OnAttack_Hit();
        J_SoundManager.Instance.PlaySound((int)pieceMove.chessType);
    }

    public void OnAttack_Finished()
    {
        pieceMove.OnAttack_Finished();
        
    }
}
