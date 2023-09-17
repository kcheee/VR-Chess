using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �Ǵ��� �׽�Ʈ�غ��� ��ũ��Ʈ

//GŰ�� ������ ������ ����ȴ�.

//������ ����Ǹ� �ִϸ��̼��� ����ȴ�

//�̰� ���� Enemy�� �״´�

//������ �״� �ִϸ��̼��� �����Ѵ�.
public class J_AttackTest : MonoBehaviour
{
    Animator anim;
    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //GŰ�� ������ ������ ����ȴ�.
        if (Input.GetKeyDown(KeyCode.G))
        {
            Attack();

        }

    }
    void Attack()
    {

        //���� ���� �ݶ��̴��� ���� �ݶ��̴��� �ε�����  

        //���ݾִϸ��̼� ����
        anim.Play("Attack");

    }
    private void OnTriggerEnter(Collider other)
    {
        //���� �ݶ��̴� ����
        if (other.CompareTag("Enemy")){
            J_EnemyHP enemyHP = other.GetComponent<J_EnemyHP>();
            if (enemyHP != null)
            {
                Debug.Log("�� ����");
                //���� HP ����
                enemyHP.HP -= (int)damage;
                //HP�� 0���ϰ� �Ǹ� �����ϴ� ����
                if (enemyHP.HP < 0)
                {
                    Debug.Log("�ı�");
                    //���� �ı� �ڵ�
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
