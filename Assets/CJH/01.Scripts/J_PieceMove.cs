using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class J_PieceMove : MonoBehaviour
{
    public enum ChessType //체스 종류
    {
        KING, QUEEN, BISHOP, KNIGHT, ROOK, PAWN,
    }
    [SerializeField]
    private float moveSpeed = 3f;
    public bool isMoving = false; // 움직이는지
    public bool hasAttacked = false; //공격해야되는경우가 아니라면

    Animator anim;
    public int currentX;
    public int currentY;
    private Vector3 targetPosition;
    Chessman[,] ch = new Chessman[8, 8];
    public ChessType chessType;

    int PosX, PosY; //타겟위치
    float preTargetX, preTargetZ; // 공격하기 전 위치
    float currTime = 0;

    float myAngle; //내가 움직였던 각도

    bool EndRot = false;  // 마지막 앞을 바라보는 회전

    public ParticleSystem particleObject; //파티클 시스템
    public Transform particlePos; //파티클 생성 위치
    private void Start()
    {
        myAngle = transform.eulerAngles.y;
        anim = GetComponentInChildren<Animator>();
        anim.Play("Idle");
    }

    private void UpdateRotate1(int x, int y)
    {
        //비숍일 때
        //targetX = 4;   //0
        //targetZ = 3;   //6
        PosX = x;
        PosY = y;

        #region 비숍
        if (chessType == ChessType.BISHOP)
        {
            if (transform.position.x * 10 - PosX < 0) preTargetX = PosX - 1;
            else if (transform.position.x * 10 - PosX > 0) preTargetX = PosX + 1;
            if (transform.position.z * 10 - PosY < 0) preTargetZ = PosY - 1;
            else if (transform.position.z * 10 - PosY > 0) preTargetZ = PosY + 1;
        }

        #endregion
        #region 룩
        if (chessType == ChessType.ROOK)
        {
            if (transform.position.x * 10 - PosX == 0 && transform.position.z * 10 - PosY < 0)
            {
                preTargetX = PosX;
                preTargetZ = PosY - 1f;
            }
            if (transform.position.x * 10 - PosX == 0 && transform.position.z * 10 - PosY > 0)
            {
                preTargetX = PosX;
                preTargetZ = PosY + 1f;
            }
            if (transform.position.x * 10 - PosX > 0 && transform.position.z * 10 - PosY == 0)
            {
                preTargetX = PosX + 1f;
                preTargetZ = PosY;
            }
            if (transform.position.x * 10 - PosX < 0 && transform.position.z * 10 - PosY == 0)
            {
                preTargetX = PosX - 1f;
                preTargetZ = PosY;
            }
        }

        #endregion
        #region 폰
        if (chessType == ChessType.PAWN)
        {

            //비숍처럼
            if (transform.position.x * 10 - PosX == 0)
            {
                Debug.Log("000");
                preTargetX = PosX;

            }
            if (transform.position.x * 10 - PosX < 0)
            {
                Debug.Log("111");
                preTargetX = PosX - 1f;
                
            }
            else if (transform.position.x * 10 - PosX > 0)
            {
                Debug.Log("222");

                preTargetX = PosX + 1f;
            }
            if (transform.position.z * 10 - PosY < 0)
            {
                Debug.Log("333f");

                preTargetZ = PosY - 1f;
            }
            else if (transform.position.z * 10 - PosY > 0)
            {
                Debug.Log("444f");

                preTargetZ = PosY + 1f;
            }
        }
        #endregion
        #region 퀸
        if (chessType == ChessType.QUEEN)
        {
            //비숍처럼
            if (transform.position.x * 10 - PosX < 0) preTargetX = PosX - 1f;
            else if (transform.position.x * 10 - PosX > 0) preTargetX = PosX + 1f;
            if (transform.position.z * 10 - PosY < 0) preTargetZ = PosY - 1f;
            else if (transform.position.z * 10 - PosY > 0) preTargetZ = PosY + 1f;
            //룩처럼
            if (transform.position.x * 10 - PosX == 0 && transform.position.z * 10 - PosY < 0)
            {
                preTargetX = PosX;
                preTargetZ = PosY - 1f;
            }
            if (transform.position.x * 10 - PosX == 0 && transform.position.z * 10 - PosY > 0)
            {
                preTargetX = PosX;
                preTargetZ = PosY + 1f;
            }
            if (transform.position.x * 10 - PosX > 0 && transform.position.z * 10 - PosY == 0)
            {
                preTargetX = PosX + 1f;
                preTargetZ = PosY;
            }
            if (transform.position.x * 10 - PosX < 0 && transform.position.z * 10 - PosY == 0)
            {
                preTargetX = PosX - 1f;
                preTargetZ = PosY;
            }
        }

        #endregion
        #region 킹
        if (chessType == ChessType.KING)
        {
            //비숍처럼
            if (transform.position.x * 10 - PosX < 0) preTargetX = PosX - 1f;
            else if (transform.position.x * 10 - PosX > 0) preTargetX = PosX + 1f;
            if (transform.position.z * 10 - PosY < 0) preTargetZ = PosY - 1f;
            else if (transform.position.z * 10 - PosY > 0) preTargetZ = PosY + 1f;
            //룩처럼
            if (transform.position.x * 10 - PosX == 0 && transform.position.z * 10 - PosY < 0)
            {
                preTargetX = PosX;
                preTargetZ = PosY - 1f;
            }
            if (transform.position.x * 10 - PosX == 0 && transform.position.z * 10 - PosY > 0)
            {
                preTargetX = PosX;
                preTargetZ = PosY + 1f;
            }
            if (transform.position.x * 10 - PosX > 0 && transform.position.z * 10 - PosY == 0)
            {
                preTargetX = PosX + 1f;
                preTargetZ = PosY;
            }
            if (transform.position.x * 10 - PosX < 0 && transform.position.z * 10 - PosY == 0)
            {
                preTargetX = PosX - 1f;
                preTargetZ = PosY;
            }
        }

        #endregion
        Debug.Log(PosX);
        Debug.Log(preTargetX );
        PieceMove(PosX, PosY);
    }
    float posOffset = 0.022f;
    public void OnAttack_Hit()
    {
        Vector3 spawnPos = transform.position + transform.forward * posOffset;
        ParticleSystem newParticle = Instantiate(particleObject,spawnPos, Quaternion.identity);
        newParticle.transform.parent = transform;
        
        //Destroy(newParticle);
        //BoardManager.Instance.Chessmans[targetX, targetZ].gameObject.GetComponentInChildren<Animator>().CrossFade("Hit", 0, 0);
    }
    public void OnAttack_Finished()
    {
        //BoardManager.Instance.Chessmans[targetX, targetZ].gameObject.GetComponentInChildren<Animator>().CrossFade("Die", 0, 0);
    }
    private void Update()
    {
        currentX = (int)transform.position.x;
        currentY = (int)transform.position.y;

        if (Input.GetKeyDown(KeyCode.K))
        {
            //PieceMove(3, 3);
            UpdateRotate1(5, 4);
        }
    }
    float angle = 0;
    int nDir = 0;
    //모든 말들의 움직임을 계산하는 함수 
    public void PieceMove(int targetX, int targetY)
    {

        Vector3 targetPos = new Vector3(targetX, 0, targetY) - transform.position * 10;
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
        StartCoroutine(RotatePiece(angle * nDir, (1.0f / 45) * angle));
        //ChangeState(PieceState.Rotate1);

        return;
    }

    //직선이동(완료)
    IEnumerator StraightMove(float targetX, float targetY, bool Enemy = false, bool rot = false)
    {
        //초기 위치
        Vector3 currentPos = transform.position * 10;
        //타겟의 위치
        targetPosition = new Vector3(targetX, 0, targetY);

        //흐른시간 체크
        float elapsedTime = 0;
        //거리잰다
        float dist = Vector3.Distance(transform.position * 10, targetPosition);
        //시간
        float duration = dist / moveSpeed;


        //타겟의 방향
        Vector3 dir = transform.forward;
        anim.CrossFade("Move", 0, 0);

        while (elapsedTime / duration < 1 /* 적이 없으면 */)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos * 0.1f, targetPosition * 0.1f, elapsedTime / duration);
            yield return null;// new WaitForSeconds(0.05f);
            //print("움직인다");
        }
        transform.position = targetPosition * 0.1f;
        yield return new WaitForSeconds(0.01f);

        // 적이있다면 공격
        if (Enemy)
        {
            // 코루틴으로 딜레이주고 실행.
            StartCoroutine(Co_Attack());
        }
        if (rot)
        {
            EndRot = true;
            // 앞에 보게 회전.
            StartCoroutine(RotatePiece(-angle * nDir, (1f / 45) * angle));
            
        }
    }
    //회전 공식(완료)
    private IEnumerator RotatePiece(float targetAngle, float duration)
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
        yield return new WaitForSeconds(0.1f);
        anim.CrossFade("Idle",0,0);
        // 움직임
        // 적이 있는지 판별

        if (!EndRot)
        {
            //1.적이없다면 바로감
            //if (BoardManager.Instance.Chessmans[PosX, PosY] == null)
            StartCoroutine(StraightMove(PosX, PosY));

            // 2. 적이 있다면 pretarget
            //else
            Debug.Log(preTargetX + " " + preTargetZ);

            StartCoroutine(StraightMove(preTargetX, preTargetZ, true));
        }
    }
    //공격 함수
    IEnumerator Co_Attack()
    {
        //yield return null;
        anim.CrossFade("Attack", 0, 0);
        Debug.Log(PosX + " " + PosY);
        yield return new WaitForSeconds(3);
        StartCoroutine(StraightMove(PosX, PosY, false, true));

    }
}