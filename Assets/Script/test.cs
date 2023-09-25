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
    public float shakeDuration = 0.5f; // ��鸲 ���� �ð�
    public float shakeAmount = 0.2f; // ��鸲 ����
    public int vibration = 10; // ���� ��
    float a;

    // ���ӽ��� UI
    public CanvasGroup GamestartUI;

    Tween myTween;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ����ȭ ���
        #region ����ȭ ���
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
        //    //float shakeDuration = 0.5f; // ������ ����
        //    //Vector3 shakeStrength = new Vector3(0, 0.2f, 0); // �Ʒ��θ� �����̵��� ����

        //    //cameraoffset.DOShakePosition(shakeDuration, -shakeStrength, vibrato: 0, randomness: 0, snapping: false);

        //    // ù ��° Tween: transform�� Go�� ��ġ�� �̵���ŵ�ϴ�.

        //    // �� ��° Tween: cameraoffset�� �Ʒ��� ���� �����մϴ�.
        //    float shakeDuration = 0.5f;
        //    Vector3 targetPos = new Vector3(cameraoffset.position.x, cameraoffset.position.y - 0.1f, cameraoffset.position.z);

        //    transform.DOMove(Go.transform.position, 10);

        //    //Tween Tes = transform.DOMove(Go.transform.position, 10);
        //    // ���߿� �� Tween�� ������ŵ�ϴ�.
        //    //myTween.Kill();

        //}
    }
    IEnumerator Giant()
    {
        bool endWhile = false;
        map.transform.DOScale(0.1f, 5).SetEase(Ease.OutQuad).OnComplete(()=> endWhile = true);
        DOTween.To(() => yoffset.CameraYOffset, x => yoffset.CameraYOffset = x, 0.25f, 5).OnComplete(() =>
        {
            // ����ȭ ������ ���� ���� UI
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
        // �����ϸ� �ȴ� ȿ�� ����.

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
