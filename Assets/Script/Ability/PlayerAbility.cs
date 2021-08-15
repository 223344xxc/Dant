using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : Ability
{
    [Header("플래이어 능력치")]
    [Tooltip("점프력")]
    [SerializeField] private float jumpPower;
    public float JumpPower
    {
        get => jumpPower;
        set => jumpPower = value;
    }

    [Tooltip("대쉬 속도")]
    [SerializeField] private float dashSpeed;

    public float DashSpeed
    {
        get => dashSpeed;
        set => dashSpeed = value;
    }

    [SerializeField] private float invincibilityTime;
    public float InvincibilityTime
    {
        get => invincibilityTime;
        set => invincibilityTime = value;

    }

    protected int HideCount_Start = 3;
    protected int HideCount_End = 5;
}
