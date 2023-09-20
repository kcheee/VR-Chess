using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // ������� 1������ ����.
    public Text timerText;
    public float totalTime = 60f;
    private float currentTime = 60f;

    public GameObject board;

    private void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        // Ÿ�̸� ����.
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
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
