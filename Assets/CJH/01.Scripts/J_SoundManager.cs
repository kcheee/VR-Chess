using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_SoundManager : MonoBehaviour
{
    public static J_SoundManager Instance;
    #region �̱���
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    #endregion 
    //�� �⹰ �� ��Ȳ�� �°� ������ҽ��� ����Ȥ���.

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
    void Start()
    {

        #region �� �⹰�� �����Ŭ��
        //if (chessType == ChessType.KING)
        //{
        //    audioSource.clip = attackSound[0];
        //}
        //else if (chessType == ChessType.QUEEN)
        //{
        //    audioSource.clip = attackSound[1];
        //}
        //else if (chessType == ChessType.BISHOP)
        //{
        //    audioSource.clip = attackSound[2];
        //}
        //else if (chessType == ChessType.KNIGHT)
        //{
        //    audioSource.clip = attackSound[3];
        //}
        //else if (chessType == ChessType.ROOK)
        //{
        //    audioSource.clip = attackSound[4];
        //    PlaySound(ChessType.ROOK);
        //}
        //else if (chessType == ChessType.PAWN)
        //{
        //    audioSource.clip = attackSound[5];
        //    PlaySound(ChessType.PAWN);
        //}
        #endregion

    }

    //���� ȣ�� �Լ�
    public void PlaySound(int soundIdx)
    {
        
        audioSource.clip = attackSound[soundIdx];
        audioSource.Play();
        //audioSource.PlayOneShot(clip);
    }
    //����� ����  �Լ��� ��ġ�°��� �����غ���
    //
}
