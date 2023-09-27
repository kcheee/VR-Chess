using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_AnimEvent : MonoBehaviour
{
    //�̺�Ʈ�Լ��� �����ϰ�ʹ�
    // Hit, AttackFinished
    //J_AttackTest attackTest;
    J_PieceMove pieceMove;

    //����
    public AudioSource audioSource;
    //public AudioClip attackSound;
    public AudioClip[] attackSound;

    public enum ChessType //ü�� ����
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
