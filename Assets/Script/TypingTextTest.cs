using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TypingTextTest : MonoBehaviour
{
    public Text Test_text;

    public string[] str;
    int i;
    private void Start()
    {
        
        StartCoroutine(Typing(str[i]));
    }
    IEnumerator Typing(string talk)
    {
        i++;
        Test_text.text = "¾Ù¸®½º : ";
        Test_text.DOText(talk, 3);

        yield return new WaitForSeconds(3f);
        if (i < str.Length)
            StartCoroutine(NextTyping(str[i]));
    }
    IEnumerator NextTyping(string talk)
    {
        Test_text.text = " ";
        yield return new WaitForSeconds(1f);
        Debug.Log(str.Length);
        StartCoroutine(Typing(str[i]));
    }
}
