using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class J_PieceMove : MonoBehaviour
{
    //체스말 상태의 FSM
    public enum PieceState
    {
        Idle, // 기본
        Rotate1, // 움직이기 전 회전
        Move1, // 이동
        Attack, // 공격
        Move2, // 공격 후 타겟 위치로 이동
        Rotate2 // 앞으로 돌아보기
    }
    public enum ChessType //체스 종류
    {
        KING, QUEEN, BISHUP, KNIGHT, ROOK, PAWN,
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
    public PieceState pieceState = PieceState.Idle;

    int targetX, targetZ; //타겟위치
    int preTargetX, preTargetZ; // 공격하기 전 위치
    float currTime = 0;

    float myAngle; //내가 움직였던 각도

    public GameObject particleObject; //파티클 시스템
    private void Start()
    {
        myAngle = transform.eulerAngles.y;
        anim = GetComponentInChildren<Animator>();
        
    }

    void ChangeState(PieceState s)
    {
        pieceState = s;
        switch (pieceState)
        {
            case PieceState.Idle:
                UpdateIdle();
                break;
            case PieceState.Rotate1:
                UpdateRotate1();
                break;
            case PieceState.Move1:
                UpdateMove1();
                break;
            case PieceState.Attack:
                UpdateAttack();
                break;
            case PieceState.Move2:
                UpdateMove2();
                break;
            case PieceState.Rotate2:
                UpdateRotate2();
                break;
        }
    }

    private void UpdateIdle()
    {
        anim.CrossFade("Idle", 0.5f, 0);
    }

    private void UpdateRotate1()
    {
        //비숍일 때
        //targetX = 4;   //0
        //targetZ = 3;   //6
        #region 비숍
        if (chessType == ChessType.BISHUP)
        {
            if (transform.position.x - targetX < 0) preTargetX = targetX - 1;
            else if (transform.position.x - targetX > 0) preTargetX = targetX + 1;
            if (transform.position.z - targetZ < 0) preTargetZ = targetZ - 1;
            else if (transform.position.z - targetZ > 0) preTargetZ = targetZ + 1;
        }

        #endregion
        #region 룩
        if (chessType == ChessType.ROOK)
        {
            if (transform.position.x - targetX == 0 && transform.position.z - targetZ < 0)
            {
                preTargetX = targetX;
                preTargetZ = targetZ - 1;
            }
            if (transform.position.x - targetX == 0 && transform.position.z - targetZ > 0)
            {
                preTargetX = targetX;
                preTargetZ = targetZ + 1;
            }
            if (transform.position.x - targetX > 0 && transform.position.z - targetZ == 0)
            {
                preTargetX = targetX + 1;
                preTargetZ = targetZ;
            }
            if (transform.position.x - targetX < 0 && transform.position.z - targetZ == 0)
            {
                preTargetX = targetX - 1;
                preTargetZ = targetZ;
            }
        }

        #endregion
        #region 폰
        if (chessType == ChessType.PAWN)
        {
            //비숍처럼
            if (transform.position.x - targetX < 0) preTargetX = targetX - 1;
            else if (transform.position.x - targetX > 0) preTargetX = targetX + 1;
            if (transform.position.z - targetZ < 0) preTargetZ = targetZ - 1;
            else if (transform.position.z - targetZ > 0) preTargetZ = targetZ + 1;
        }
        #endregion
        #region 퀸
        if (chessType == ChessType.QUEEN)
        {
            //비숍처럼
            if (transform.position.x - targetX < 0) preTargetX = targetX - 1;
            else if (transform.position.x - targetX > 0) preTargetX = targetX + 1;
            if (transform.position.z - targetZ < 0) preTargetZ = targetZ - 1;
            else if (transform.position.z - targetZ > 0) preTargetZ = targetZ + 1;
            //룩처럼
            if (transform.position.x - targetX == 0 && transform.position.z - targetZ < 0)
            {
                preTargetX = targetX;
                preTargetZ = targetZ - 1;
            }
            if (transform.position.x - targetX == 0 && transform.position.z - targetZ > 0)
            {
                preTargetX = targetX;
                preTargetZ = targetZ + 1;
            }
            if (transform.position.x - targetX > 0 && transform.position.z - targetZ == 0)
            {
                preTargetX = targetX + 1;
                preTargetZ = targetZ;
            }
            if (transform.position.x - targetX < 0 && transform.position.z - targetZ == 0)
            {
                preTargetX = targetX - 1;
                preTargetZ = targetZ;
            }
        }

        #endregion
        #region 킹
        if (chessType == ChessType.KING)
        {
            //비숍처럼
            if (transform.position.x - targetX < 0) preTargetX = targetX - 1;
            else if (transform.position.x - targetX > 0) preTargetX = targetX + 1;
            if (transform.position.z - targetZ < 0) preTargetZ = targetZ - 1;
            else if (transform.position.z - targetZ > 0) preTargetZ = targetZ + 1;
            //룩처럼
            if (transform.position.x - targetX == 0 && transform.position.z - targetZ < 0)
            {
                preTargetX = targetX;
                preTargetZ = targetZ - 1;
            }
            if (transform.position.x - targetX == 0 && transform.position.z - targetZ > 0)
            {
                preTargetX = targetX;
                preTargetZ = targetZ + 1;
            }
            if (transform.position.x - targetX > 0 && transform.position.z - targetZ == 0)
            {
                preTargetX = targetX + 1;
                preTargetZ = targetZ;
            }
            if (transform.position.x - targetX < 0 && transform.position.z - targetZ == 0)
            {
                preTargetX = targetX - 1;
                preTargetZ = targetZ;
            }
        }
           
        #endregion
        PieceMove(targetX, targetZ);
    }

    private void UpdateMove1()
    {
        Debug.Log("ssssdfjshajkfhsdkfgjsah");

        StartCoroutine(StraightMove(preTargetX, preTargetZ));
    }

   public void OnAttack_Hit()
    {
        Instantiate(particleObject);
        BoardManager.Instance.Chessmans[targetX, targetZ].gameObject.GetComponentInChildren<Animator>().CrossFade("Hit", 0, 0);
    }
    public void OnAttack_Finished()
    {
        BoardManager.Instance.Chessmans[targetX, targetZ].gameObject.GetComponentInChildren<Animator>().CrossFade("Die", 0, 0);
    }


    private void UpdateAttack()
    {
        currTime = 0;
        anim.CrossFade("Attack", 0, 0);
        
    }

    private void UpdateMove2()
    {

        StartCoroutine(StraightMove(targetX, targetZ));
    }

    private void UpdateRotate2()
    {
        // ----------- 턴넘김----------------
        BoardManager.Instance.PieceIsMove = false;
        // ----------- 턴넘김 ----------------

        //yield return new WaitForSeconds(1f);

        //회전한만큼 회전시킨다.
        StartCoroutine(RotatePiece(-angle * nDir, (1.0f / 45) * angle));

        ChangeState(PieceState.Idle);
    }

    private void Update()
    {
        //currentX = (int)(transform.position.x*10);
        //currentY = (int)(transform.position.z*10);

        if (Input.GetKeyDown(KeyCode.N))
        {
            ChangeState(PieceState.Rotate1);
        }

        if(pieceState == PieceState.Attack)
        {
            currTime += Time.deltaTime;
            if(currTime > 3)
            {
                ChangeState(PieceState.Move2);
            }
        }
    }
    float angle = 0;
    int nDir = 0;
    //모든 말들의 움직임을 계산하는 함수 
    public void PieceMove(int targetX, int targetY)
    {
        Vector3 targetPos = new Vector3(targetX, 0, targetY) - transform.position*10;
        Debug.Log(new Vector3(targetX, 0, targetY) + " :  " + transform.position*10);
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
        Debug.Log(angle);
        //StartCoroutine(Attack(targetX, targetY));
        StartCoroutine(RotatePiece(angle * nDir, (1.0f / 45) * angle));

        return;
    }
    #region 공격로직 완성
    //1. 위치값을 받는다
    //2. 적이 있는지 확인한다
    //3. 있으면 원래 위치까지 움직이는 것이 아니라 -1만큼 가서
    //4. 때리는 애니메이션 하고 다시+1만큼 간다
    //++다시 앞으로 회전한다.
    //float attackRange = 2f;
    #endregion
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
        targetPosition = new Vector3(targetX, 0, targetY);

        //흐른시간 체크
        float elapsedTime = 0;
        //거리잰다
        float dist = Vector3.Distance(transform.position, targetPosition*0.1f);
        //시간
        float duration = dist / moveSpeed;
        Debug.Log(duration+"dui");

        //타겟의 방향
        Vector3 dir = transform.forward;
        anim.Play("Move", 0, 0);
        while (elapsedTime / duration < 1 /* 적이 없으면 */)
        {
            elapsedTime += Time.deltaTime;
            transform.position = (Vector3.Lerp(currentPos, targetPosition, elapsedTime / duration))*0.1f;
            Debug.Log("실행");
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
        yield return new WaitForSeconds(0.01f);

        if(pieceState == PieceState.Move1)
        {
            ChangeState(PieceState.Attack);           
        }
        else if(pieceState == PieceState.Move2) 
        {
            ChangeState(PieceState.Rotate2);
        }
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
        Debug.Log(pieceState);

        //if(pieceState == PieceState.Rotate1)
        //{
        //    ChangeState(PieceState.Move1);
        //}
        ChangeState(PieceState.Rotate1);

        if (pieceState == PieceState.Rotate1)
        {
            //적 판별하기 Move1 or move2
            if (BoardManager.Instance.Chessmans[targetX, targetZ] != null) //적이 있다면
            {
                ChangeState(PieceState.Move1);
            }
            else if(BoardManager.Instance.Chessmans[targetX, targetZ] == null){
                
                ChangeState(PieceState.Move2);
                Debug.Log("적이 있다");

            }
        }

       
    }
}
