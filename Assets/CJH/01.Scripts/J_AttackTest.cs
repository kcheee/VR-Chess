using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격이 되는지 테스트해보는 스크립트

//G키를 누르면 공격이 실행된다.

//공격이 실행되면 애니메이션이 재생된다

//이걸 맞은 Enemy는 죽는다

//죽으면 죽는 애니메이션을 실행한다.
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
        //G키를 누르면 공격이 실행된다.
        if (Input.GetKeyDown(KeyCode.G))
        {
            Attack();

        }

    }
    void Attack()
    {

        //나의 무기 콜라이더에 상대방 콜라이더와 부딪히면  

        //공격애니메이션 실행
        anim.Play("Attack");

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
}
