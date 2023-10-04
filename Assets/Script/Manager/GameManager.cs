using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    #region �̱���
    static public GameManager instance;

    private void Awake()
    {
        instance = this; 
    }
    #endregion

    // ������� 1������ ����.
    public Text timerText;
    public float totalTime = 60f;
    private float currentTime = 60f;

    public GameObject board;

    // fade In & Out
    public CanvasGroup canvasGroup;
    public CanvasGroup winText;

    // Start��ư ����.
    public GameObject Startbutton;

    private void Start()
    {
        // ���̵� �� & ���̵� �� ����
        currentTime = totalTime;
        // MainScene�׽�Ʈ�� ����
        // SoundManager�� Title���� Don't destory�� ������.
        if (SoundManager.Instance != null)
        {
        SoundManager.Instance.audioSource.clip = SoundManager.Instance.BGMList[1];
        SoundManager.Instance.audioSource.Play();
        SoundManager.Instance.audioSource.DOFade(1, 7);
        }

        canvasGroup.DOFade(0, 5);
    }

    void Update()
    {
        // Ÿ�̸� ����.
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            //UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            // Ÿ�̸Ӱ� ����Ǿ��� �� ������ �۾�
            // ���� ����
            TimerOver();

        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            // ����ȭ �׽�Ʈ.

            board.transform.DOScale(0.1f, 1);
        }
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Winner();
        }
    }

    // ���� ���� 
    // ���� ��ư���� ȣ��.
    public void GameStart()
    {
        Startbutton.SetActive(false);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.audioSource.clip = SoundManager.Instance.BGMList[2];
            SoundManager.Instance.audioSource.Play();
            //SoundManager.Instance.audioSource.DOFade(1, 4);
        }
    }

    // �¸� UI
    public void Winner()
    {
        //winText.
        winText.DOFade(1,2).OnComplete(() =>
        {
            canvasGroup.DOFade(1, 2);
        });
        Debug.Log("����");
    }

    // Ÿ�̸� ����.
    #region Timer
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void TimerOver()
    {
        // ���� ������.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }
    #endregion
}
