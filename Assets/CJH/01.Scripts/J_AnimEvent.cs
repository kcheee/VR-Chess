using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_AnimEvent : MonoBehaviour
{
    //�̺�Ʈ�Լ��� �����ϰ�ʹ�
    // Hit, AttackFinished
    J_AttackTest attackTest;

    // Start is called before the first frame update
    void Awake()
    {
        attackTest = GetComponentInParent<J_AttackTest>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public void OnAttack_Hit()
    {
        attackTest.OnAttack_Hit();

    }
    public void OnAttack_Finished()
    {
        attackTest.OnAttack_Finished();
    }
}
