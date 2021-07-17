using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : Ability
{
    [Header("플래이어 능력치")]
    [Tooltip("점프력")]
    [SerializeField]
    private float JumpPower;
    public float JP
    {
        get
        {
            return JumpPower;
        }
        set
        {
            JumpPower = value;
        }
    }

    [Tooltip("대쉬 속도")]
    [SerializeField]
    private float DashSpeed;

    public float DS
    {
        get
        {
            return DashSpeed;
        }
        set
        {
            DashSpeed = value;
        }
    }
}
