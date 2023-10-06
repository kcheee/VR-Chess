using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.XR.CoreUtils;
using UnityEngine.Rendering;
using UnityEditor.XR.LegacyInputHelpers;

public class WalkANDGiant : MonoBehaviour
{

    public GameObject Des_Pos;
    public GameObject map;
    public XROrigin yoffset;
    public Transform cameraoffset;
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    public float shakeAmount = 0.2f; // 흔들림 강도

    // 게임시작 UI
    public CanvasGroup GamestartUI;
    public AudioClip clip;

    AudioSource audioSource;

    Tween myTween;

    public delegate IEnumerator WalkDelegate(); // 델리게이트 정의
    public WalkDelegate Giantdele; // 델리게이트 변수 선언


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 거대화 효과 델리게이트에 담는다.
        Giantdele = Giant;
    }

    // Update is called once per frame
    void Update()
    {
        // 거인화 기능
        #region 거인화 기능
        if (Input.GetKeyDown(KeyCode.F))
        {
            //map.transform.DOScale(0.1f, 5).SetEase(Ease.OutQuad);
            //DOTween.To(() => yoffset.CameraYOffset, x => yoffset.CameraYOffset = x, 0.2f, 5);
            //transform.DOScale(0.1f, 10);
            StartCoroutine(Giant());
        }
        //transform.DOMove(Go.transform.position, 0.1f);
        #endregion

        if (Input.GetKeyDown(KeyCode.S))
        {
            Walk();
        }
    }

    // 거인화 기능
    IEnumerator Giant()
    {
        bool endWhile = false;
        audioSource.clip = clip;
        audioSource.Play();
        map.transform.DOScale(0.1f, 5).SetEase(Ease.OutQuad).OnComplete(() => endWhile = true);
        DOTween.To(() => yoffset.CameraYOffset, x => yoffset.CameraYOffset = x, 0.25f, 5).OnComplete(() =>
        {
            // 거인화 끝나고 게임 시작 UI
            GamestartUI.DOFade(1, 2);
            //transform.position = new Vector3(transform.position.x, 0.202f, transform.position.y);
        });

        while (!endWhile)
        {
            transform.position = Vector3.Lerp(transform.position, Des_Pos.transform.position, 1f);
            yield return null;

        }
        yield return null;
    }

    // 걷는 기능.
    public void Walk()
    {
        // 도착하면 걷는 효과 멈춤.

        float shakeDuration = 0.5f;
        float targetY = cameraoffset.position.y - 0.15f;

        myTween = cameraoffset.DOMoveY(targetY, shakeDuration).SetLoops(-1, LoopType.Yoyo);

        transform.DOMove(Des_Pos.transform.position, 10).OnComplete(() =>
        {
            myTween.Kill();

            StartCoroutine(TypingTextTest.instance.delay(TypingTextTest.instance.str2));
        });
    }

}
