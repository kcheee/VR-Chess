using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_SoundManager : MonoBehaviour
{
    //각 기물 별 상황에 맞게 오디오소스가 재생된ㄷㅏ.

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
    void Start()
    { 
        audioSource = gameObject.AddComponent<AudioSource>();

        #region 각 기물별 오디오클립
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

    //음악 호출 함수
    public void OnSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    //상속을 통해  함수가 겹치는것을 방지해본다
    //

    // Update is called once per frame
    void Update()
    {
        
    }
}
