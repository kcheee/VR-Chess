using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LodingScene : MonoBehaviour
{

    public Slider slider;
    public CanvasGroup canvasG_Fade;

    private void Start()
    {
        canvasG_Fade.DOFade(0, 0.5f);
    }
    private void FixedUpdate()
    {
        slider.value += 0.5f;
        
        if(slider.value == slider.maxValue) StartCoroutine(delaySceneChage());
        
    }
    IEnumerator delaySceneChage()
    {
        SoundManager.Instance.audioSource.DOFade(0, 3);
        canvasG_Fade.DOFade(1, 2).OnComplete(() =>
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
