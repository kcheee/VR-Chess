using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleUIButton : MonoBehaviour
{
    public GameObject startbutton;
    public GameObject Levelbutton;

    public CanvasGroup canvasG_Fadeout;

    private void levelUIOn()
    {
        Levelbutton.SetActive(true);
        Levelbutton.GetComponent<CanvasGroup>().DOFade(1, 1);
    }

    // start 버튼
    public void onclickStart()
    {
        startbutton.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() =>
        {
            startbutton.SetActive(false);

            levelUIOn();
        });
        //startbutton.SetActive(false);
        Debug.Log("scene Change");

    }

    // 난이도 설정

    public void onclickEasy_Level()
    {
        canvasG_Fadeout.DOFade(1, 2).OnComplete(() =>
        {
            SceneSetting.Instance.depth = 2;
            SceneManager.LoadScene(1);
        });
    }
    public void onclickMiddle_Level()
    {
        canvasG_Fadeout.DOFade(1, 2).OnComplete(() =>
        {
            SceneSetting.Instance.depth = 3;
            SceneManager.LoadScene(1);
        });

    }
    public void onclickHard_Level()
    {
        canvasG_Fadeout.DOFade(1, 2).OnComplete(() =>
        {
            SceneSetting.Instance.depth = 4;
            SceneManager.LoadScene(1);
        });
    }
}
