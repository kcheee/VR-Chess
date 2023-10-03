using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_SoundManager : MonoBehaviour
{
    public static J_SoundManager Instance;
    #region 싱글톤
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
    //각 기물 별 상황에 맞게 오디오소스가 재생된ㄷㅏ.

    //사운드
    public AudioSource audioSource;
    //public AudioClip attackSound;
    public AudioClip[] dieSound;
    public AudioClip[] attackSound;
    public AudioClip[] moveSound;

    public enum ChessType //체스 종류
    {
        KING, QUEEN, BISHOP, KNIGHT, ROOK, PAWN,
    }
    public ChessType chessType;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void DieSound(int soundIdx)
    {
        audioSource.clip = dieSound[soundIdx];
        audioSource.Play();

    }

    //음악 호출 함수
    public void PlaySound(int soundIdx)
    {        
        audioSource.clip = attackSound[soundIdx];
        audioSource.Play();
        //audioSource.PlayOneShot(clip);
    }
    //상속을 통해  함수가 겹치는것을 방지해본다
    //
    public void MoveSound(int soundIdx)
    {
        audioSource.clip = moveSound[soundIdx];
        //Debug.Log("처음");
        //플레이를 하고 있을때는 멈추고 안하고있을때는 실행이된다.
        if (audioSource.isPlaying)
        {
            //Debug.Log("음악 멈춰");
            audioSource.Stop();
        }
        else
        {
            //Debug.Log("음악 재생");
            audioSource.Play();
        }

    }

}
