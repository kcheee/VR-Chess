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
        Test_text.text = null;
        Test_text.DOText(talk, 5);

        yield return new WaitForSeconds(5f);
        StartCoroutine(NextTyping(str[i]));
    }
    IEnumerator NextTyping(string talk)
    {
        Test_text.text = " ";
        yield return new WaitForSeconds(1f);
        if(i<5) 
        StartCoroutine(Typing(str[i]));
    }
}
