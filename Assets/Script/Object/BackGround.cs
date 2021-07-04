using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public GameObject camera;
    Vector3 pos;

    void Awake()
    {
        
    }

    void FixedUpdate()
    {
        UpdateBackGround();
    }

    void UpdateBackGround()
    {
        pos = transform.position;
        pos.y = camera.transform.position.y;
        transform.position = pos;
    }
}
