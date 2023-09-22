using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LodingScene : MonoBehaviour
{

    public Slider slider;
    public CanvasGroup canvasGroup;

    private void FixedUpdate()
    {
        slider.value += 0.5f;
        
        if(slider.value == slider.maxValue) StartCoroutine(delaySceneChage());
        
    }
    IEnumerator delaySceneChage()
    {
        canvasGroup.DOFade(1, 2).OnComplete(() =>
        {
            StartCoroutine(delaySceneChage2());
        });
        yield return null;
    }
    IEnumerator delaySceneChage2()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);


    }
}
