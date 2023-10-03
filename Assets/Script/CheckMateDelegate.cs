using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CheckMateDelegate : MonoBehaviour
{
    public static CheckMateDelegate Instance;
    private void Awake()
    {
        Instance = this;
    }
    public delegate void FadeInOut();
    public FadeInOut fadeinout;

    CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // ÆäÀÌµå ÀÎ ¾Æ¿ô.
        fadeinout = fade;
    }

    public void fade()
    {
        Debug.Log("r°³»õ²¥");
        canvasGroup.DOFade(1, 1.5f).OnComplete(() =>
        {
            canvasGroup.DOFade(0, 1);
        });
    }
}
