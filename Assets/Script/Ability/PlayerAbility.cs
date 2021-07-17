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

}
