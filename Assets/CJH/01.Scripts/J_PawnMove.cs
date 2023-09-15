using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class J_PawnMove : MonoBehaviour
{
    //폰의 이동규칙 : 앞으로 1보, 앞으로 2보, 45도 회전 후 앞으로 1보, -45도 회전 후 앞으로 1보
    //앞으로 1보는 MovePiece(0,1), 45도 회전은 StartCoroutine(RotatePiece(-45, 1f)),  
    //45도 회전과 -45회전할때를 다시 비교해보자
    //45도회전은 x +1을 받았을 때 -45도 회전은 x-1을 받았을 때 그리고 공통적으로 y+1을 받았을 때다
    //45도 회전은 targetPosition - this.transform.position.x =1


    //회전해서 앞으로 이동했으면 다시 앞으로 또 회전한다.

    [SerializeField]
    public float moveSpeed = 5f;
    public bool isMoving = false; // 움직이는지
    public bool isRightRotate = false; //오른쪽회전하는지(+)
    public bool isLeftRotate = false; //왼쪽회전하는지(-)

    Animator anim;
    public int currentX;
    public int currentY;
    private Vector3 targetPosition;
    float originRot;
    bool isDelay;
    float currentTime;
    float dealyTime;
    float fianlAngle;
    Chessman[,] ch = new Chessman[8,8];

    float myAngle; //내가 움직였던 각도
    private void Start()
    {
        originRot = transform.localEulerAngles.y;
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
        if(Input.GetKeyDown(KeyCode.B)) {
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
            
            //PawnMove(0, 0);
            Attack(1, 1);
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
        Debug.Log("실행");
        nDir = 0;
        Vector3 targetPos =  new Vector3 (targetX, 0, targetY) - transform.position; 
        float dot = Vector3.Dot(transform.right, targetPos);

        if (dot > 0)
        {
            nDir = 1;
            print("오른쪽에 회전해야해");
        }
        else if (dot < 0)
        {
            nDir = -1;
            print("왼쪽에 회전해야해");
        }
        else // dot = 0 앞/뒤
        {
            float d = Vector3.Dot(transform.forward, targetPos);
            if (d < 0) //뒤
            {
                nDir = 1;
                print("뒤로 돌아라");
            }
            else //앞
            {
                nDir = 0;
                print("회전 하지 말아라");
            }
        }
        //상대방과 나와의 각도를 잰다
        angle = Vector3.Angle(transform.forward, targetPos);
        //StartCoroutine(Attack(targetX, targetY));
        StartCoroutine(RotatePiece(angle * nDir, (1.0f / 45) * angle, targetX, targetY, true));
        
        return;
        #region 전코드
        
        #endregion
    }
    //1. 위치값을 받는다
    //2. 적이 있는지 확인한다
    //3. 있으면 원래 위치까지 움직이는 것이 아니라 -1만큼 가서
    //4. 때리는 애니메이션 하고 다시+1만큼 간다
    //++다시 앞으로 회전한다.
    public void Attack(int targetX, int targetY)
    {
        //적이 있는지 확인한다. => 내가 가고자하는 목표값에 적이 있다면
        //적인지 구분하는 것 : Chessman 코드의 bool값의 isWhite 


        //ch.SetValue(targetX, targetY);
        //ch[targetX, targetY];
        //
        Chessman targetPosition = ch[targetX, targetY];
        // 적이라면
        if(ch[targetX, targetY].isWhite == false)
        {
            Debug.Log("무조건 띄워야 함");

        }   

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
        float dist = Vector3.Distance(transform.position, targetPosition);
        //시간
        float duration = dist / moveSpeed;


        //타겟의 방향
        Vector3 dir = transform.forward;
        anim.Play("Move", 0, 0);
        while (elapsedTime / duration <  1 /* 적이 없으면 */)
        {
            elapsedTime +=  Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, targetPosition, elapsedTime / duration);
            //멈추고
            yield return null;// new WaitForSeconds(0.05f);
            print("움직인다");
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.04f);
        //anim.SetTrigger("Idle");
        anim.CrossFade("Idle", 0.5f, 0);
        //yield return new WaitForSeconds(1f);

        //나의 앞을 기준으로 초기값으로 안되어있다면 회전시킨다.
        //초기각도 : v3.orgin; 변경된 각도 currnetAngle 
        StartCoroutine(RotatePiece(-angle * nDir, (1.0f / 45) * angle, targetX, targetY, false));

        
    }
    //회전 공식(완료)
    private IEnumerator RotatePiece(float targetAngle, float duration, int x, int y, bool moveFoward)
    {
        if(duration > 0)
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
        //StartCoroutine(StraightMove(0, 1));

        if(moveFoward)
        {
            StartCoroutine(StraightMove(x, y));
        }

        //회전해서 앞으로 이동했으면
        //다시 앞으로 또 회전한다.
        //transform.rotation = Quaternion.Euler(0f, myAngle, 0f);

        
    }
}
