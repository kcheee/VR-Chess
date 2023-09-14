using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//���� �̵���Ģ : ������ 1��, ������ 2��, 45�� ȸ�� �� ������ 1��, -45�� ȸ�� �� ������ 1��
//������ 1���� MovePiece(0,1), 45�� ȸ���� StartCoroutine(RotatePiece(-45, 1f)),  
//45�� ȸ���� -45ȸ���Ҷ��� �ٽ� ���غ���
//45��ȸ���� x +1�� �޾��� �� -45�� ȸ���� x-1�� �޾��� �� �׸��� ���������� y+1�� �޾��� ����
//45�� ȸ���� targetPosition - this.transform.position.x =1
public class J_Pawn : MonoBehaviour
{
    #region �̱���
    public static J_Pawn Instance { set; get; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    //G��ư�� ������ ������ 1��ŭ �����̰� �ʹ�

    [SerializeField]
    public float moveSpeed = 5f;
    public bool isMoving = false; // �����̴���
    private bool isRotate = false; //ȸ���ϴ���
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
            //�����ߴٸ� �����̴� ���� �����
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
            //2ĭ �����δ�
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

    //�����̴� �ڵ带 �Լ��� ������
    public void MovePiece(int x, int y)
    {
        if(y <=0)
        {
            print("�̵��Ҽ�����");
            return;
        }
        //targetPosition�� ������ ���� ������ ����
        Vector3 dir = transform.forward;
        targetPosition = transform.position + dir * y;

        int targetY = y - currentY;
        int targetX = x - currentX;
        //targetPosition = (new Vector3(0, 0, steps) - transform.position).normalized;

        //�浹 üũ

        //������ ������ ���ϰ� �ϴ� ���� 

        //�����δ�
        isMoving = true; 
        anim.SetTrigger("Move");
    }
    //����, �ð��� �Ű������� �޴� ���� �ƴ϶� ��ġ���� �޴´�. �� ��ġ���� ���� ������ �����ش�
    private IEnumerator RotatePiece(float targetAngle, float duration)
    {
        float initialAngle = transform.eulerAngles.y; //�ڽ��� ����
        float elapsedTime = 0f; // �ð�
        //�ð��� �帣�ٰ�
        while (elapsedTime < duration) 
        {
            //�����̴� ������ Lerp������ ȸ��
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
    public bool isWorking = false; //      x,y ��ǥ�� ���ÿ� ����Ǽ� �̰Ÿ� �����ϱ� ����
    public bool notyet = false;
    public bool turn = false;
    public void MoveMethod(int moveX, int moveY)
    {
        int targetX = moveX - currentX;
        int targetY = moveY - currentY;
        //0,0��ġ���� ȸ���� �Ѵٸ�
        // -45���� ȸ���� �Ѵ�
        if (targetX > 0 && isWorking == false && turn == false) //�밢�� ȸ��
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
            //������ ��ġ�� �̵��ϴ� ����
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
            //�����ߴٸ� �����̴� ���� �����
            if (transform.position == targetPosition)
            {
                //isMoving = false;
                anim.SetTrigger("Idle");
                isWorking = false;
            }
        }
        //�ʱ�ȭ
        if (transform.position == new Vector3(moveX, 0, moveY))
        {
            turn = false; 
            notyet = false;
            isWorking = false;
        }
    }
    public void NewMoveMethod(int moveX, int moveY)
    {
        //ȸ���� �Ѵٸ� 
        if (isRotate == true)
        {
            //45�� ȸ��
            if (moveX >0 && moveY > 0)
            {

            }
            //-45�� ȸ��
            if(moveX < 0 && moveY > 0)
            {

            }
        }
        //ȸ���� �� �Ѵٸ�

    }
}
