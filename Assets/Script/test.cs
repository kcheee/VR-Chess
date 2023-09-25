using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.XR.CoreUtils;
using UnityEngine.Rendering;
using UnityEditor.XR.LegacyInputHelpers;

public class test : MonoBehaviour
{

    public GameObject Des_Pos;
    public GameObject map;
    public XROrigin yoffset;
    public Transform cameraoffset;
    public float shakeDuration = 0.5f; // 흔들림 지속 시간
    public float shakeAmount = 0.2f; // 흔들림 강도
    public int vibration = 10; // 진동 수
    float a;

    // 게임시작 UI
    public CanvasGroup GamestartUI;

    Tween myTween;
    void Start()
    {
        
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
        //transform.position += transform.forward * 2 * Time.deltaTime;
        //transform.position += transform.forward*5*Time.deltaTime;

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    //float shakeDuration = 0.5f; // 강도를 높임
        //    //Vector3 shakeStrength = new Vector3(0, 0.2f, 0); // 아래로만 움직이도록 조절

        //    //cameraoffset.DOShakePosition(shakeDuration, -shakeStrength, vibrato: 0, randomness: 0, snapping: false);

        //    // 첫 번째 Tween: transform을 Go의 위치로 이동시킵니다.

        //    // 두 번째 Tween: cameraoffset을 아래로 흔들기 시작합니다.
        //    float shakeDuration = 0.5f;
        //    Vector3 targetPos = new Vector3(cameraoffset.position.x, cameraoffset.position.y - 0.1f, cameraoffset.position.z);

        //    transform.DOMove(Go.transform.position, 10);

        //    //Tween Tes = transform.DOMove(Go.transform.position, 10);
        //    // 나중에 이 Tween을 중지시킵니다.
        //    //myTween.Kill();

        //}
    }
    IEnumerator Giant()
    {
        bool endWhile = false;
        map.transform.DOScale(0.1f, 5).SetEase(Ease.OutQuad).OnComplete(()=> endWhile = true);
        DOTween.To(() => yoffset.CameraYOffset, x => yoffset.CameraYOffset = x, 0.25f, 5).OnComplete(() =>
        {
            // 거인화 끝나고 게임 시작 UI
            GamestartUI.DOFade(1, 2);
        });

        while (!endWhile)
        {
            transform.position = Vector3.Lerp(transform.position,Des_Pos.transform.position,1f);
            yield return null;

        }
        yield return null;
    }
    void Walk()
    {
        // 도착하면 걷는 효과 멈춤.

        float shakeDuration = 0.5f;
        float targetY = cameraoffset.position.y - 0.15f;

        myTween = cameraoffset.DOMoveY(targetY, shakeDuration).SetLoops(-1, LoopType.Yoyo);

        transform.DOMove(Des_Pos.transform.position, 10).OnComplete(() => {
            myTween.Kill();

            StartCoroutine(TypingTextTest.instance.delay(TypingTextTest.instance.str2));
            Debug.Log("tlfgod");
            });

    }

}
