using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

public class EatCake : XRGrabInteractable
{
    public GameObject player;

    float distance;
    AudioSource audioSource;
    Rigidbody rb;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
    }
    protected override void Grab()
    {
        base.Grab();

    }
    protected override void Drop()
    {
        base.Drop();
        rb.velocity = Vector3.zero;
        if (distance < 0.3f)
        {
            Debug.Log("케이크 먹음");
            audioSource.Play();
            transform.DOMove(new Vector3(player.transform.position.x, player.transform.position.y - 0.1f, player.transform.position.z), 1).OnComplete(() =>
            {
                StartCoroutine(delay_Giant());
                //Destroy(gameObject);              
            });
        }
    }

    IEnumerator delay_Giant()
    {
        yield return new WaitForSeconds(2);

        // 거인화 델리게이트에 담음.
        WalkANDGiant walkANDGiant = player.GetComponentInParent<WalkANDGiant>();

        if (walkANDGiant != null)
        {
             StartCoroutine(walkANDGiant.Giantdele());
        }
        //TestDele someScript = GetComponent<TestDele>(); // SomeScript는 델리게이트가 정의된 스크립트의 이름입니다.

        //if (someScript != null)
        //{
        //    someScript.testDelegate = ddd; // testMethod는 실행하고자 하는 메서드입니다.
        //    someScript.testDelegate(); // 델리게이트 실행
        //}
    }
}
