using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class J_PawnMove : MonoBehaviour
{
    //���� �̵���Ģ : ������ 1��, ������ 2��, 45�� ȸ�� �� ������ 1��, -45�� ȸ�� �� ������ 1��
    //������ 1���� MovePiece(0,1), 45�� ȸ���� StartCoroutine(RotatePiece(-45, 1f)),  
    //45�� ȸ���� -45ȸ���Ҷ��� �ٽ� ���غ���
    //45��ȸ���� x +1�� �޾��� �� -45�� ȸ���� x-1�� �޾��� �� �׸��� ���������� y+1�� �޾��� ����
    //45�� ȸ���� targetPosition - this.transform.position.x =1

    [SerializeField]
    public float moveSpeed = 5f;
    public bool isMoving = false; // �����̴���
    public bool isRightRotate = false; //������ȸ���ϴ���(+)
    public bool isLeftRotate = false; //����ȸ���ϴ���(-)

    Animator anim;
    public int currentX;
    public int currentY;
    private Vector3 targetPosition;
    bool isDelay;
    float currentTime;
    float dealyTime;

    float myAngle; //���� �������� ����
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
    }

   public void PawnMove(int targetX, int targetY)
    {
        Debug.Log("����");
        int dir = 0;
        Vector3 targetPos =  new Vector3 (targetX, 0, targetY) - transform.position; 
        float dot = Vector3.Dot(transform.right, targetPos);

        if (dot > 0)
        {
            dir = 1;
            print("�����ʿ� ȸ���ؾ���");
        }
        else if (dot < 0)
        {
            dir = -1;
            print("���ʿ� ȸ���ؾ���");
        }
        else // dot = 0 ��/��
        {
            float d = Vector3.Dot(transform.forward, targetPos);
            if (d < 0) //��
            {
                dir = 1;
                print("�ڷ� ���ƶ�");
            }
            else //��
            {
                dir = 0;
                print("ȸ�� ���� ���ƶ�");
            }
        }
        //����� ������ ������ ���
        float angle = Vector3.Angle(transform.forward, targetPos);

        StartCoroutine(RotatePiece(angle * dir, (1.0f / 45) * angle, targetX, targetY));

        return;
        //Ÿ�� ��ġ�� ������ ��ġ�� �Ÿ��� ���
        int moveX = targetX - currentX;
        int moveY = targetY - currentY;

        //ȸ���� �Ѵ�
        if(moveY >0)
        {
            if(moveX >0) //������ �밢�� ȸ��(+)
            {
                StartCoroutine(RotatePiece(0, (1.0f / 0) * 0, 0, targetY));
                //StopCoroutine("RotatePiece");
                //���� �ڷ�ƾ�� ���� �Ϸ�ǰ��� �����̵��ϰ�ʹ�
                //StartCoroutine(StraightMove());

            }
            else if(moveX <0) //���� �밢�� ȸ��(-)
            {
                //StartCoroutine(RotatePiece(-45, 1));
                //���� �ڷ�ƾ�� ���� �Ϸ�ǰ��� �����̵��ϰ�ʹ�
                //StartCoroutine(StraightMove(0, 1));

            }
            else if(moveX == 0) //�����̵�
            {
                StartCoroutine(StraightMove(0, 1));
            }
        }
        else if(moveY < 0)
        {
            print("�̵��Ҽ�����");
            //return;
        }
    }

    //�����̵�(�Ϸ�)
    IEnumerator StraightMove(int targetX, int targetY)
    {
        //�ʱ� ��ġ
        Vector3 currentPos = transform.position;
        //Ÿ���� ��ġ
        targetPosition = new Vector3(targetX, 0, targetY);
        
        //�帥�ð� üũ
        float elapsedTime = 0;
        //�Ÿ����
        float dist = Vector3.Distance(transform.position, targetPosition);
        //�ð�
        float duration = dist / moveSpeed;


        //Ÿ���� ����
        Vector3 dir = transform.forward;
        anim.Play("Move", 0, 0);
        while (elapsedTime / duration < 1)
        {
            elapsedTime +=  Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, targetPosition, elapsedTime / duration);
            
            yield return null;// new WaitForSeconds(0.05f);
            print("�����δ�");
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.04f);
        //anim.SetTrigger("Idle");
        anim.CrossFade("Idle", 0.5f, 0);
        //yield return new WaitForSeconds(1f);
        
    }

    //ȸ�� ����(�Ϸ�)
    private IEnumerator RotatePiece(float targetAngle, float duration, int x, int y)
    {
        if(duration > 0)
        {
            //�ڽ��� ����
            float currentAngle = 0;// transform.eulerAngles.y;
            float elapsedTime = 0f; // �ð�
                                    //�ð��� �帣��
            while (elapsedTime < duration)
            {
                //ȸ���� Lerp������ ȸ��
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
        StartCoroutine(StraightMove(x, y));

    }
}
