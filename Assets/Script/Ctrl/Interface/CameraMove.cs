using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Tooltip("카메라가 타겟을 따라가는 시간")]
    public float CamMoveTime;
    public GameObject Target;
    public Vector3 TargetPos;
    public Vector3 Offset;
    Vector3 vel;
   

    private void LateUpdate()
    {

        TargetPos = Target.transform.position;
        TargetPos.y = 0;
        gameObject.transform.position = Vector3.SmoothDamp(transform.position, TargetPos + Offset, ref vel, CamMoveTime);

    }

}
