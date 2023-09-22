using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIButton : MonoBehaviour
{
    public GameObject startbutton;
    public List<GameObject> gameObjects;


    private void levelUIOn()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(true);
        }
    }

    // start 버튼
    public void onclickStart()
    {
        startbutton.SetActive(false);
        Debug.Log("scene Change");
        levelUIOn();

    }

    // 난이도 설정

    public void onclickEasy_Level()
    {
        SceneSetting.Instance.depth = 2;
        SceneManager.LoadScene(1);
    }
    public void onclickMiddle_Level()
    {
        SceneSetting.Instance.depth = 3;

        SceneManager.LoadScene(1);
    }
    public void onclickHard_Level()
    {
        SceneSetting.Instance.depth = 4;

        SceneManager.LoadScene(1);
    }
}
