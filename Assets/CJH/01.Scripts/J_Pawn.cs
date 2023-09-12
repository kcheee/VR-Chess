using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Pawn : MonoBehaviour
{
    //G��ư�� ������ ������ 1��ŭ �����̰� �ʹ�

    [SerializeField]
    public float moveSpeed = 1f;
    private bool isMoving = false;
    Vector3 oneStepPosition;
    Vector3 twoStepPosition;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
        //1�� �̵��� ��ġ
        oneStepPosition = transform.position + Vector3.forward;
        //2�� �̵��� ��ġ
        twoStepPosition = transform.position + Vector3.forward * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, oneStepPosition, moveSpeed * Time.deltaTime /5 );
            //�����ߴٸ� �����̴� ���� �����
            if(transform.position == oneStepPosition)
            {
                isMoving = false;
                anim.SetTrigger("Idle");
            }
            
        }
        if (Input.GetKeyDown(KeyCode.G) && !isMoving)
        {
            isMoving = true;
            anim.SetTrigger("Move");
        }
    }
    
}
