using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_Pawn : MonoBehaviour
{
    //G버튼을 누르면 앞으로 1만큼 움직이고 싶다

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
        
        //1보 이동할 위치
        oneStepPosition = transform.position + Vector3.forward;
        //2보 이동할 위치
        twoStepPosition = transform.position + Vector3.forward * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, oneStepPosition, moveSpeed * Time.deltaTime /5 );
            //도착했다면 움직이는 것을 멈춘다
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
