using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField]
    [Header("능력치")]
    [Tooltip("공격력")]
    private float AttackDamage;

    public float AD
    {
        get
        {
            return AttackDamage;
        }
        set
        {
            AttackDamage = value;
        }
    }

    [SerializeField]
    [Tooltip("방어력")]
    private float Defense;

    public float DF
    {
        get
        {
            return Defense;
        }
        set
        {
            Defense = value;
        }
    }

    [SerializeField]
    [Tooltip("이동속도")]
    private float MoveSpeed;

    public float Speed
    {
        get
        {
            return MoveSpeed * 0.1f;
        }
        set
        {
            MoveSpeed = value;
        }
    }


    public virtual void Start() { }

    public virtual void Update() { }
}
