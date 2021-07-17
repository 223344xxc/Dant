using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JoyStickDirection
{
    CENTER,
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public class JoyStickCtrl : MonoBehaviour
{
    [Header("조이스틱 옵션")]
    [Tooltip("조이스틱 최대 거리")]
    public float MaxDistance;
    public float SelectDirectionDistance;
    public GameObject JoyStickBase;
    [Tooltip("조이스틱 속도")]
    public float StickSpeed;
    [Tooltip("조이스틱 방향")]
    public static JoyStickDirection StickDirection;

    public static Vector3 JoyStickPosition;

    public static bool StickFollow = false;

    Vector2 TargetPos;
    Vector2 StartStickPos;
    Vector3 movevel;

    void Awake()
    {
    }

    void Start()
    {
        
    }
    
    void Update()
    {

        StartStickPos = JoyStickBase.transform.position;
        Follow();
    }

    void Follow()
    {
        if (StickFollow) 
        {

            //TargetPos = (Input.mousePosition - (Vector3)StartStickPos).magnitude < MaxDistance ?
            //    ((Vector2)Input.mousePosition - StartStickPos).normalized * ((Vector2)Input.mousePosition - StartStickPos).magnitude :
            //    ((Vector2)Input.mousePosition - StartStickPos).normalized * MaxDistance;
            //transform.position = Vector3.SmoothDamp(transform.position, TargetPos + StartStickPos, ref movevel, StickSpeed);
            if (Input.touchCount > 0)
            {
                TargetPos = (Input.GetTouch(0).position - StartStickPos).magnitude < MaxDistance ?
                    (Input.GetTouch(0).position - StartStickPos).normalized * (Input.GetTouch(0).position - StartStickPos).magnitude :
                    (Input.GetTouch(0).position - StartStickPos).normalized * MaxDistance;
                transform.position = Vector3.SmoothDamp(transform.position, TargetPos + StartStickPos, ref movevel, StickSpeed);
            }

        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, StartStickPos, ref movevel, StickSpeed);
        }
        //조이스틱 pc 테스트
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = StartStickPos + new Vector2(100, 0);
            StickFollow = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position = StartStickPos + new Vector2(-100, 0);
            StickFollow = true;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.position = StartStickPos + new Vector2(0, 100);
            StickFollow = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position = StartStickPos + new Vector2(0, -100);
            StickFollow = true;
        }
        else
            StickFollow = false;

        JoyStickPosition = transform.position - (Vector3)StartStickPos;

        if (Mathf.Atan2(JoyStickPosition.y, JoyStickPosition.x) > 45 * Mathf.Deg2Rad &&
            Mathf.Atan2(JoyStickPosition.y, JoyStickPosition.x) < Mathf.Deg2Rad * 135 && 
            JoyStickPosition.sqrMagnitude > SelectDirectionDistance * SelectDirectionDistance)
        {
            StickDirection = JoyStickDirection.UP;
        }
        else if(Mathf.Atan2(JoyStickPosition.y, JoyStickPosition.x) < -45 * Mathf.Deg2Rad 
            && Mathf.Atan2(JoyStickPosition.y, JoyStickPosition.x) > Mathf.Deg2Rad * -135
            && JoyStickPosition.sqrMagnitude > SelectDirectionDistance * SelectDirectionDistance)
        {
            StickDirection = JoyStickDirection.DOWN;
        }
        else
            StickDirection = JoyStickDirection.CENTER;

        //Debug.Log(StickDirection);
    }

    public void ChangeSitckFollow(bool stickFollow)
    {
        StickFollow = stickFollow;
    }
}
