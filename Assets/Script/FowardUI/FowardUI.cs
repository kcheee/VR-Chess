using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FowardUI : MonoBehaviour
{
    public GameObject Player;
    RectTransform rt;
    CanvasGroup canvasG;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        canvasG = GetComponent<CanvasGroup>();

        StartCoroutine(delayStart());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(rt.transform.position + " " + Player.transform.position);
            rt.transform.position = Player.transform.position + Player.transform.forward * 0.5f;
        }
    }
    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(2);
        rt.transform.position = Player.transform.position + Player.transform.forward * 0.5f;
        canvasG.DOFade(1, 1);
    }
}
