using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//폰의 이동규칙 : 앞으로 1보, 앞으로 2보, 45도 회전 후 앞으로 1보, -45도 회전 후 앞으로 1보
//앞으로 1보는 MovePiece(0,1), 45도 회전은 StartCoroutine(RotatePiece(-45, 1f)),  
//45도 회전과 -45회전할때를 다시 비교해보자
//45도회전은 x +1을 받았을 때 -45도 회전은 x-1을 받았을 때 그리고 공통적으로 y+1을 받았을 때다
//45도 회전은 targetPosition - this.transform.position.x =1
public class J_Pawn : MonoBehaviour
{
    #region 싱글톤
    public static J_Pawn Instance { set; get; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    //G버튼을 누르면 앞으로 1만큼 움직이고 싶다

    [SerializeField]
    public float moveSpeed = 5f;
    public bool isMoving = false; // 움직이는지
    private bool isRotate = false; //회전하는지
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    Animator anim;
    public float rotAnglePerSecond = 360f;
    public int currentX ;
    public int currentY;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime / 5);
            //도착했다면 움직이는 것을 멈춘다
            if (transform.position == targetPosition)
            {
                isMoving = false;
                anim.SetTrigger("Idle");
            }

        }
        if(Input.GetKeyDown(KeyCode.T) && !isMoving)
        {
            notyet = true;
        }
        if (notyet == true)
        {
            currentX = (int)transform.position.x;
            currentY = (int)transform.position.z;
            
            MoveMethod(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.G) && !isMoving)
        {
            MovePiece(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.H) && !isMoving)
        {
            //2칸 움직인다
            MovePiece(0,2);
        }
        if(Input.GetKeyDown(KeyCode.J) && !isMoving)
        {
            //PawnMove(5,5);
            StartCoroutine(RotatePiece(45, 1f));
            //MovePiece(1);
            //RoatatePiece(1);j
        }
        if (Input.GetKeyDown(KeyCode.K) && !isMoving)
        {
            //PawnMove(5,5);
            StartCoroutine(RotatePiece(-45, 1f));
            //MovePiece(1);
            //RoatatePiece(1);j
        }
    }

    //움직이는 코드를 함수로 만들자
    public void MovePiece(int x, int y)
    {
        if(y <=0)
        {
            print("이동할수없음");
            return;
        }
        //targetPosition의 방향을 나의 앞으로 설정
        Vector3 dir = transform.forward;
        targetPosition = transform.position + dir * y;

        int targetY = y - currentY;
        int targetX = x - currentX;
        //targetPosition = (new Vector3(0, 0, steps) - transform.position).normalized;

        //충돌 체크

        //밖으로 나가지 못하게 하는 로직 

        //움직인다
        isMoving = true; 
        anim.SetTrigger("Move");
    }
    //각도, 시간을 매개변수로 받는 것이 아니라 위치값을 받는다. 그 위치값에 따라 각도를 정해준다
    private IEnumerator RotatePiece(float targetAngle, float duration)
    {
        float initialAngle = transform.eulerAngles.y; //자신의 각도
        float elapsedTime = 0f; // 시간
        //시간이 흐르다가
        while (elapsedTime < duration) 
        {
            //움직이는 각도를 Lerp값으로 회전
            float angle = Mathf.LerpAngle(initialAngle, targetAngle, elapsedTime / duration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        isWorking = false;
        //isMoving = true;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }
    public bool isWorking = false; //      x,y 좌표가 동시에 실행되서 이거를 구분하기 위해
    public bool notyet = false;
    public bool turn = false;
    public void MoveMethod(int moveX, int moveY)
    {
        int targetX = moveX - currentX;
        int targetY = moveY - currentY;
        //0,0위치에서 회전을 한다면
        // -45도로 회전을 한다
        if (targetX > 0 && isWorking == false && turn == false) //대각선 회전
        {
            isWorking = true;
            turn = true;
            StartCoroutine(RotatePiece(45, 1f));
            print("1");
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime / 5);
        }
        else if (targetX < 0 && isWorking == false)
        {
            print("2");
            isWorking = true;
            StartCoroutine(RotatePiece(-45, 1f));
        }

        if (targetY > 0 && isWorking == false)
        {
            //상대방의 위치로 이동하는 공식
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime / 5);
            isWorking = true;
            MovePiece(targetX, targetY);
        }

        if (isMoving == true)
        {
            print(targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime / 5);
            if (Vector3.Distance (transform.position, targetPosition) < 0.2f)
            {
                turn = false;
                notyet = false;
                isWorking = false;
            }
            //도착했다면 움직이는 것을 멈춘다
            if (transform.position == targetPosition)
            {
                //isMoving = false;
                anim.SetTrigger("Idle");
                isWorking = false;
            }
        }
        //초기화
        if (transform.position == new Vector3(moveX, 0, moveY))
        {
            turn = false; 
            notyet = false;
            isWorking = false;
        }
    }
    public void NewMoveMethod(int moveX, int moveY)
    {
        //회전을 한다면 
        if (isRotate == true)
        {
            //45도 회전
            if (moveX >0 && moveY > 0)
            {

            }
            //-45도 회전
            if(moveX < 0 && moveY > 0)
            {

            }
        }
        //회전을 안 한다면

    }
}
