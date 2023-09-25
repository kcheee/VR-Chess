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

    public Text Test_text;

    public string[] str1;
    public string[] str2;

    int i;
    private void Start()
    {
        // 딜레이 주고 시작.
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(Typing(str1[i]));
    }
    IEnumerator Typing(string talk)
    {
        i++;
        Test_text.text = null;
        Test_text.DOText(talk, 3);

        yield return new WaitForSeconds(3f);
        if (i < str1.Length)
            StartCoroutine(NextTyping(str1[i]));
    }
    IEnumerator NextTyping(string talk)
    {
        Test_text.text = " ";
        yield return new WaitForSeconds(1f);
        Debug.Log(str1.Length);
        StartCoroutine(Typing(str1[i]));
    }
}
