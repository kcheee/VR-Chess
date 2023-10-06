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
            Debug.Log("����ũ ����");
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

        // ����ȭ ��������Ʈ�� ����.
        WalkANDGiant walkANDGiant = player.GetComponentInParent<WalkANDGiant>();

        if (walkANDGiant != null)
        {
             StartCoroutine(walkANDGiant.Giantdele());
        }
        //TestDele someScript = GetComponent<TestDele>(); // SomeScript�� ��������Ʈ�� ���ǵ� ��ũ��Ʈ�� �̸��Դϴ�.

        //if (someScript != null)
        //{
        //    someScript.testDelegate = ddd; // testMethod�� �����ϰ��� �ϴ� �޼����Դϴ�.
        //    someScript.testDelegate(); // ��������Ʈ ����
        //}
    }
}
