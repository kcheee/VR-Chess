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
    public float shakeDuration = 0.5f; // ��鸲 ���� �ð�
    public float shakeAmount = 0.2f; // ��鸲 ����

    // ���ӽ��� UI
    public CanvasGroup GamestartUI;
    public AudioClip clip;

    AudioSource audioSource;

    Tween myTween;

    public delegate IEnumerator WalkDelegate(); // ��������Ʈ ����
    public WalkDelegate Giantdele; // ��������Ʈ ���� ����


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // �Ŵ�ȭ ȿ�� ��������Ʈ�� ��´�.
        Giantdele = Giant;
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
    }

    // ����ȭ ���
    IEnumerator Giant()
    {
        bool endWhile = false;
        audioSource.clip = clip;
        audioSource.Play();
        map.transform.DOScale(0.1f, 5).SetEase(Ease.OutQuad).OnComplete(() => endWhile = true);
        DOTween.To(() => yoffset.CameraYOffset, x => yoffset.CameraYOffset = x, 0.25f, 5).OnComplete(() =>
        {
            // ����ȭ ������ ���� ���� UI
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

    // �ȴ� ���.
    public void Walk()
    {
        // �����ϸ� �ȴ� ȿ�� ����.

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
