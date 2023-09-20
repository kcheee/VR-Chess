using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions.Must;

//������ �Ǵ��� �׽�Ʈ�غ��� ��ũ��Ʈ

//GŰ�� ������ ������ ����ȴ�.

//������ ����Ǹ� �ִϸ��̼��� ����ȴ�

//�̰� ���� Enemy�� �״´�

//������ �״� �ִϸ��̼��� �����Ѵ�.
public class J_AttackTest : MonoBehaviour
{
    Chessman[,] ch = new Chessman[8, 8];
    Animator anim;
    Animator EnemyAnimator;
    public int damage = 10;
    bool hasAttacked;
    private Vector3 targetPosition;
    public GameObject Red;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        EnemyAnimator = Red.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //tŰ�� ������ 
        //Red.
        //Ÿ�� ��ġ�� ���� �ִ� ��� ����
       

        //GŰ�� ������ ������ ����ȴ�.
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(Attack(1,1));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EnemyDie();
        }
    }
    //bool isEnemyPosition(int targetX, int targetY)
    //{
    //    Chessman targetChessman = ch[targetX, targetY];
    //    if (targetChessman != null)
    //    {
    //        //���� ���
    //        if (targetChessman.isWhite != GetComponent<Chessman>().isWhite)
    //        {
    //            Debug.Log("���Դϴ�.");
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    IEnumerator Attack(int targetX, int targetY)
    {
        //Ÿ���� ��ġ
        targetPosition = new Vector3(targetX,0,targetY);
        //���ݾִϸ��̼��� �����Ѵ�
        anim.CrossFade("Attack",0,0);
        //���� �ִϸ��̼��� �������� Red�� ���ӿ�����Ʈ �ȿ� �پ��ִ� �ִϸ������� Die�� ����ȴ�.
        yield return new WaitForSeconds(1);

        Debug.Log("���� �ִϸ��̼� ����");
        //Animator EnemyAnimator = Red.GetComponentInChildren<Animator>();
        //EnemyAnimator.Play("Hit");
        //EnemyAnimator.Play("Die",0,0);
        
    }

    void EnemyDie()
    {
        anim.Play("Die");
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
    public void OnAttack_Hit()
    {
        EnemyAnimator.CrossFade("Hit",0,0);
    }
    public void OnAttack_HitPlus()
    {
        EnemyAnimator.CrossFade("Hit", 0, 0);
    }
    public void OnAttack_Finished()
    {
        Debug.Log("���� �ִ�");
        EnemyAnimator.CrossFade("Die",0,0);
    }
}
