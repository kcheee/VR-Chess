using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions.Must;

//공격이 되는지 테스트해보는 스크립트

//G키를 누르면 공격이 실행된다.

//공격이 실행되면 애니메이션이 재생된다

//이걸 맞은 Enemy는 죽는다

//죽으면 죽는 애니메이션을 실행한다.
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
        //t키를 누르면 
        //Red.
        //타겟 위치에 적이 있는 경우 공격
       

        //G키를 누르면 공격이 실행된다.
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
    //        //적인 경우
    //        if (targetChessman.isWhite != GetComponent<Chessman>().isWhite)
    //        {
    //            Debug.Log("적입니다.");
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    IEnumerator Attack(int targetX, int targetY)
    {
        //타겟의 위치
        targetPosition = new Vector3(targetX,0,targetY);
        //공격애니메이션을 실행한다
        anim.CrossFade("Attack",0,0);
        //공격 애니메이션이 끝날때쯤 Red의 게임오브젝트 안에 붙어있는 애니메이터인 Die가 실행된다.
        yield return new WaitForSeconds(1);

        Debug.Log("공격 애니메이션 성공");
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
        //상대방 콜라이더 검출
        if (other.CompareTag("Enemy")){
            J_EnemyHP enemyHP = other.GetComponent<J_EnemyHP>();
            if (enemyHP != null)
            {
                Debug.Log("적 감지");
                //상대방 HP 감소
                enemyHP.HP -= (int)damage;
                //HP가 0이하가 되면 제거하는 로직
                if (enemyHP.HP < 0)
                {
                    Debug.Log("파괴");
                    //상대방 파괴 코드
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
        Debug.Log("다이 애니");
        EnemyAnimator.CrossFade("Die",0,0);
    }
}
