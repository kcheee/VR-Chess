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
    public AudioClip[] dieSound;
    public AudioClip[] attackSound;
    public AudioClip[] moveSound;

    public enum ChessType //ü�� ����
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

    //���� ȣ�� �Լ�
    public void PlaySound(int soundIdx)
    {        
        audioSource.clip = attackSound[soundIdx];
        audioSource.Play();
        //audioSource.PlayOneShot(clip);
    }
    //����� ����  �Լ��� ��ġ�°��� �����غ���
    //
    public void MoveSound(int soundIdx)
    {
        audioSource.clip = moveSound[soundIdx];
        //Debug.Log("ó��");
        //�÷��̸� �ϰ� �������� ���߰� ���ϰ��������� �����̵ȴ�.
        if (audioSource.isPlaying)
        {
            //Debug.Log("���� ����");
            audioSource.Stop();
        }
        else
        {
            //Debug.Log("���� ���");
            audioSource.Play();
        }

    }

}
