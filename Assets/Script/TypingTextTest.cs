using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TypingTextTest : MonoBehaviour
{
    static public TypingTextTest instance;
    private void Awake()
    {
        instance = this; 
    }

    public CanvasGroup TextFade;
    public Text Test_text;

    public string[] str1;
    public string[] str2;

    int i;
    private void Start()
    {
        // 딜레이 주고 시작.
        // 앨리스 대사
        StartCoroutine(delay(str1));
    }

   public IEnumerator delay(string[] s)
    {
        yield return new WaitForSeconds(2f);
        TextFade.DOFade(1, 1);
        // 앨리스 대사
        StartCoroutine(Typing(s));
    }
    IEnumerator Typing(string[] talk)
    {
        yield return new WaitForSeconds(1f);

        Test_text.text = null;
        Test_text.DOText(talk[i], 3);
        i++;

        yield return new WaitForSeconds(2f);
        if (i < talk.Length)
            StartCoroutine(NextTyping(talk));
        else
        {
            i = 0;
            yield return new WaitForSeconds(2f);
            TextFade.DOFade(0, 2);
        }
    }
    IEnumerator NextTyping(string[] talk)
    {

        yield return new WaitForSeconds(2f);
        Test_text.text = " ";
        StartCoroutine(Typing(talk));
    }
}
