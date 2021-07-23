using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField]
    [Header("능력치")]
    private float attackDamage; //공격력

    public float AttackDamage
    {
        get => attackDamage;
        set => attackDamage = value;
    }

    [SerializeField]
    private float defense; //방어력

    public float Defense
    {
        get => defense;
        set => defense = value;
    }

    [SerializeField]
    private float moveSpeed; //이동속도

    public float MoveSpeed
    {
        get => moveSpeed * 0.1f;
        set => moveSpeed = value;
    }
}
