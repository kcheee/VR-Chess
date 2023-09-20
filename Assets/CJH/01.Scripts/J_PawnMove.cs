using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class J_PawnMove : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    public bool isMoving = false; // 움직이는지
    public bool hasAttacked = false; //공격해야되는경우가 아니라면

    Animator anim;
    public int currentX;
    public int currentY;
    private Vector3 targetPosition;
    Chessman[,] ch = new Chessman[8, 8];

    float myAngle; //내가 움직였던 각도
    private void Start()
    {
        myAngle = transform.eulerAngles.y;
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        currentX = (int)transform.position.x;
        currentY = (int)transform.position.y;
        if (Input.GetKeyDown(KeyCode.V))
        {
            PawnMove(1, 2);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //StartCoroutine(RotatePiece(45, 1));
            PawnMove(0, 3);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PawnMove(5, 5);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            PawnMove(0, 1);

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //Attack();
            //PawnMove(0, 0);

            //TryAttack(currentX + 1, currentY + 1);
        }
    }

    //모든 말들의 움직임을 계산하는 함수 (현재 PawnMove랑 같음)
    void PieceMove(int targetX, int targetY)
    {
        Vector3 targetPos = new Vector3(targetX, 0, targetY) - transform.position;
        float dot = Vector3.Dot(transform.right, targetPos);
        //내적을 통해 회전할지를 정한다.
        int dir = (dot > 0) ? 1 : (dot < 0) ? -1 : (Vector3.Dot(transform.forward, targetPos) < 0) ? 1 : 0;
        //상대방과 나와의 각도를 잰다
        float angle = Vector3.Angle(transform.forward, targetPos);
        StartCoroutine(RotatePiece(angle * dir, (1.0f / 45) * angle, targetX, targetY, true));
        return;
    }

    float angle = 0;
    int nDir = 0;
    //모든 말들의 움직임을 계산하는 함수 
    public void PawnMove(int targetX, int targetY)
    {
        //Debug.Log("실행");
        //nDir = 0;
        // scale 0.1f 곱해줌.
        Vector3 targetPos = new Vector3((float)targetX * 0.1f, 0, (float)targetY*0.1f) - transform.position;
        float dot = Vector3.Dot(transform.right, targetPos);
        nDir = (dot > 0) ? 1 : (dot < 0) ? -1 : (Vector3.Dot(transform.forward, targetPos) < 0) ? 1 : 0;
        #region 1if
        //if (dot > 0)
        //{
        //    nDir = 1;
        //    print("오른쪽에 회전해야해");
        //}
        //else if (dot < 0)
        //{
        //    nDir = -1;
        //    print("왼쪽에 회전해야해");
        //}
        //else // dot = 0 앞/뒤
        //{
        //    float d = Vector3.Dot(transform.forward, targetPos);
        //    if (d < 0) //뒤
        //    {
        //        nDir = 1;
        //        print("뒤로 돌아라");
        //    }
        //    else //앞
        //    {
        //        nDir = 0;
        //        print("회전 하지 말아라");
        //    }
        //}
        #endregion
        //상대방과 나와의 각도를 잰다
        angle = Vector3.Angle(transform.forward, targetPos);
        //StartCoroutine(Attack(targetX, targetY));
        StartCoroutine(RotatePiece(angle * nDir, (1.0f / 45) * angle, targetX, targetY, true));

        return;
    }
    //1. 위치값을 받는다
    //2. 적이 있는지 확인한다
    //3. 있으면 원래 위치까지 움직이는 것이 아니라 -1만큼 가서
    //4. 때리는 애니메이션 하고 다시+1만큼 간다
    //++다시 앞으로 회전한다.
    //float attackRange = 2f;
    public void Attack(int targetX, int targetY)
    {

        Debug.Log("적을 공격하는 진짜 함수");
        anim.Play("Attack",0,0);
        //Chessman targetChessman = ch[targetX, targetY];

        //Debug.Log("그래도 여기 공격 함수는 들어오는거 맞지");
        //if(targetChessman != null)
        //{
        //    Debug.Log("코루틴이 문제인듯");
        //    //적인 경우
        //    if(targetChessman.isWhite != GetComponent<Chessman>().isWhite) { 
        //        Debug.Log("적 공격");
        //        anim.Play("Attack");
        //    }
        //    else
        //    {
        //        Debug.Log("적 공격 불가");
        //    }
        //}

        ////적을 탐지. 
        //Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        //foreach(Collider enemy in hitEnemies)
        //{
        //    //적인지 확인하기 위해 Chessman 스크립트를 가진 체스 찾기
        //    Chessman chessman = enemy.GetComponent<Chessman>();

        //    if(chessman != null)
        //    {
        //        //적인 경우에만 
        //        if(chessman.isWhite =false)
        //        {
        //            //isWhite값이 false인 경우 공격(적)
        //            Debug.Log("적을 공=격");
        //            anim.Play("Attack");
        //            //적 파괴
        //            //Destroy(enemy.gameObject);

        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}
    }
    //직선이동(완료)
    IEnumerator StraightMove(int targetX, int targetY)
    {
        //초기 위치
        Vector3 currentPos = transform.position;
        //타겟의 위치
        // scale을 작게 만들어줘서 0,1f을 곱함.
        targetPosition = new Vector3((float)targetX*0.1f, 0, (float)targetY * 0.1f);

        //흐른시간 체크
        float elapsedTime = 0;
        //거리잰다
        float dist = Vector3.Distance(transform.position, targetPosition);
        //시간
        float duration = dist / moveSpeed;


        //타겟의 방향
        Vector3 dir = transform.forward;
        anim.Play("Move", 0, 0);
        while (elapsedTime / duration < 1 /* 적이 없으면 */)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, targetPosition, elapsedTime / duration);
            //타겟 위치에 적이 있는 경우 공격
            if(!hasAttacked && isEnemyPosition(targetX,targetY))
            {
                Debug.Log("적이 있어서 공격");
                Attack(targetX, targetY);
                hasAttacked = true; //중복 공격 방지
            }
            yield return null;// new WaitForSeconds(0.05f);
            //print("움직인다");
        }
        transform.position = targetPosition;
        yield return new WaitForSeconds(0.04f);
        anim.CrossFade("Idle", 0.5f, 0);

        // ----------- 턴넘김----------------
        BoardManager.Instance.PieceIsMove = false;
        // ----------- 턴넘김 ----------------

        //yield return new WaitForSeconds(1f);

        //회전한만큼 회전시킨다.
        StartCoroutine(RotatePiece(-angle * nDir, (1.0f / 45) * angle, targetX, targetY, false));
    }
    //타겟위치에 적이 있는지 체크하는 함수
    bool isEnemyPosition(int targetX, int targetY)
    {
        Chessman targetChessman = ch[targetX, targetY];
        if(targetChessman != null)
        {
            //적인 경우
            if(targetChessman.isWhite != GetComponent<Chessman>().isWhite)
            {
                Debug.Log("적입니다.");
                return true;
            }
        }
        return false;
    } 
    //회전 공식(완료)
    private IEnumerator RotatePiece(float targetAngle, float duration, int x, int y, bool moveFoward)
    {
        if (duration > 0)
        {
            //자신의 각도
            float currentAngle = 0;// transform.eulerAngles.y;
            float elapsedTime = 0f; // 시간
                                    //시간이 흐르면
            while (elapsedTime < duration)
            {
                //회전을 Lerp값으로 회전
                float angle = Mathf.LerpAngle(currentAngle, targetAngle, elapsedTime / duration);

                transform.rotation = Quaternion.Euler(0, myAngle + angle, 0);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        myAngle = myAngle + targetAngle;
        transform.rotation = Quaternion.Euler(0f, myAngle, 0f);
        yield return new WaitForSeconds(1);

        if (moveFoward)
        {
            StartCoroutine(StraightMove(x, y));
        }
    }
}
