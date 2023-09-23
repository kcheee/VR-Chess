using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor.Rendering.LookDev;

public class GameManager : MonoBehaviour
{

    #region 싱글톤
    static public GameManager instance;

    private void Awake()
    {
        instance = this; 
    }
    #endregion

    // 프로토는 1분으로 제한.
    public Text timerText;
    public float totalTime = 60f;
    private float currentTime = 60f;

    public GameObject board;

    // fadeout
    public CanvasGroup canvasGroup;
    public CanvasGroup winText;

    private void Start()
    {
        // 페이드 인
        currentTime = totalTime;
        SoundManager.Instance.audioSource.clip = SoundManager.Instance.BGMList[2];
        SoundManager.Instance.audioSource.Play();
        SoundManager.Instance.audioSource.DOFade(1, 7);
    }

    void Update()
    {
        // 타이머 설정.
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            // 타이머가 종료되었을 때 실행할 작업
            // 게임 오버
            TimerOver();

        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            // 소인화 테스트.

            board.transform.DOScale(0.1f, 1);
        }
    }

    public void Winner()
    {
        //winText.
        winText.DOFade(1,2).OnComplete(() =>
        {
            canvasGroup.DOFade(1, 2);
        });
        Debug.Log("실행");
    }

    // 타이머 설정.
    #region Timer
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void TimerOver()
    {
        // 게임 꺼버림.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    #endregion
}
