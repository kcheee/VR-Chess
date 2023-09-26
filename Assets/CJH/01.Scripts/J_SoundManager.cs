using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_SoundManager : MonoBehaviour
{
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
        audioSource = gameObject.AddComponent<AudioSource>();

        #region �� �⹰�� �����Ŭ��
        if (chessType == ChessType.KING)
        {
            audioSource.clip = attackSound[0];
        }
        else if (chessType == ChessType.QUEEN)
        {
            audioSource.clip = attackSound[1];
        }
        else if (chessType == ChessType.BISHOP)
        {
            audioSource.clip = attackSound[2];
        }
        else if (chessType == ChessType.KNIGHT)
        {
            audioSource.clip = attackSound[3];
        }
        else if (chessType == ChessType.ROOK)
        {
            audioSource.clip = attackSound[4];
        }
        else if (chessType == ChessType.PAWN)
        {
            audioSource.clip = attackSound[5];
        }
        #endregion

    }

    //���� ȣ�� �Լ�
    public void OnSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    //����� ����  �Լ��� ��ġ�°��� �����غ���
    //

    // Update is called once per frame
    void Update()
    {
        
    }
}
