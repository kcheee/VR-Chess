using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
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


    //ȸ���ؼ� ������ �̵������� �ٽ� ������ �� ȸ���Ѵ�.

    [SerializeField]
    public float moveSpeed = 5f;
    public bool isMoving = false; // �����̴���
    public bool isRightRotate = false; //������ȸ���ϴ���(+)
    public bool isLeftRotate = false; //����ȸ���ϴ���(-)

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

    float myAngle; //���� �������� ����
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

    //��� ������ �������� ����ϴ� �Լ� (���� PawnMove�� ����)
    void PieceMove(int targetX, int targetY)
    {
        Vector3 targetPos = new Vector3(targetX, 0, targetY) - transform.position;
        float dot = Vector3.Dot(transform.right, targetPos);
        //������ ���� ȸ�������� ���Ѵ�.
        int dir = (dot > 0) ? 1 : (dot < 0) ? -1 : (Vector3.Dot(transform.forward, targetPos) < 0) ? 1 : 0;
        //����� ������ ������ ���
        float angle = Vector3.Angle(transform.forward, targetPos);
        StartCoroutine(RotatePiece(angle * dir, (1.0f / 45) * angle, targetX, targetY, true));

        return;
    }

    float angle = 0;
    int nDir = 0;
    //��� ������ �������� ����ϴ� �Լ� 
    public void PawnMove(int targetX, int targetY)
    {
        Debug.Log("����");
        nDir = 0;
        Vector3 targetPos =  new Vector3 (targetX, 0, targetY) - transform.position; 
        float dot = Vector3.Dot(transform.right, targetPos);

        if (dot > 0)
        {
            nDir = 1;
            print("�����ʿ� ȸ���ؾ���");
        }
        else if (dot < 0)
        {
            nDir = -1;
            print("���ʿ� ȸ���ؾ���");
        }
        else // dot = 0 ��/��
        {
            float d = Vector3.Dot(transform.forward, targetPos);
            if (d < 0) //��
            {
                nDir = 1;
                print("�ڷ� ���ƶ�");
            }
            else //��
            {
                nDir = 0;
                print("ȸ�� ���� ���ƶ�");
            }
        }
        //����� ������ ������ ���
        angle = Vector3.Angle(transform.forward, targetPos);
        //StartCoroutine(Attack(targetX, targetY));
        StartCoroutine(RotatePiece(angle * nDir, (1.0f / 45) * angle, targetX, targetY, true));
        
        return;
        #region ���ڵ�
        
        #endregion
    }
    //1. ��ġ���� �޴´�
    //2. ���� �ִ��� Ȯ���Ѵ�
    //3. ������ ���� ��ġ���� �����̴� ���� �ƴ϶� -1��ŭ ����
    //4. ������ �ִϸ��̼� �ϰ� �ٽ�+1��ŭ ����
    //++�ٽ� ������ ȸ���Ѵ�.
    public void Attack(int targetX, int targetY)
    {
        //���� �ִ��� Ȯ���Ѵ�. => ���� �������ϴ� ��ǥ���� ���� �ִٸ�
        //������ �����ϴ� �� : Chessman �ڵ��� bool���� isWhite 


        //ch.SetValue(targetX, targetY);
        //ch[targetX, targetY];
        //
        Chessman targetPosition = ch[targetX, targetY];
        // ���̶��
        if(ch[targetX, targetY].isWhite == false)
        {
            Debug.Log("������ ����� ��");

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
        while (elapsedTime / duration <  1 /* ���� ������ */)
        {
            elapsedTime +=  Time.deltaTime;
            transform.position = Vector3.Lerp(currentPos, targetPosition, elapsedTime / duration);
            //���߰�
            yield return null;// new WaitForSeconds(0.05f);
            print("�����δ�");
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.04f);
        //anim.SetTrigger("Idle");
        anim.CrossFade("Idle", 0.5f, 0);

        // ������ �Ϸ� move �Ϸ�
        // ������ �ϴ� �Լ��� ¥�� �����ϰ� �� ��ġ�� �̵��ϰ� �״��� �� �ѱ�.

        // ----------- �ϳѱ� ----------------
        BoardManager.Instance.PieceIsMove = false;
        // ----------- �ϳѱ� ----------------

        //yield return new WaitForSeconds(1f);


        //���� ���� �������� �ʱⰪ���� �ȵǾ��ִٸ� ȸ����Ų��.
        //�ʱⰢ�� : v3.orgin; ����� ���� currnetAngle 
        StartCoroutine(RotatePiece(-angle * nDir, (1.0f / 45) * angle, targetX, targetY, false));


    }
    //ȸ�� ����(�Ϸ�)
    private IEnumerator RotatePiece(float targetAngle, float duration, int x, int y, bool moveFoward)
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

        if(moveFoward)
        {
            StartCoroutine(StraightMove(x, y));
        }

        //ȸ���ؼ� ������ �̵�������
        //�ٽ� ������ �� ȸ���Ѵ�.
        //transform.rotation = Quaternion.Euler(0f, myAngle, 0f);

        
    }
}
