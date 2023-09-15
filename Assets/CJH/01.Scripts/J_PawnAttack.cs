using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. 위치값을 받는다
//2. 적이 있는지 확인한다
//3. 있으면 원래 위치까지 움직이는 것이 아니라 -1만큼 가서
//4. 때리는 애니메이션 하고 다시+1만큼 간다
//++다시 앞으로 회전한다.

//애니메이터에서 각각 공격하는 애니메이션을 통일해서 넣자
public class J_PawnAttack : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
    }
    ////상대방을 나의 최초 각도라고 생각하자.
    //void OriginRotate(int x, int y)
    //{

    //    Vector3 originPos = new Vector3(x, 0, y) - transform.position;
    //    float dot = Vector3.Dot(transform.right, originPos);
    //    int dir = (dot > 0) ? 1 : (dot < 0) ? -1 : (Vector3.Dot(transform.forward, originPos) < 0) ? 1 : 0;
    //    //최초의 각도와 나와의 각도를 잰다
    //    float angle = Vector3.Angle(transform.forward, originPos);
    //    StartCoroutine(OriginRotPiece(angle * dir, (1f / 45) * angle, x, y);


    //}

}
