using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("능력치")]
    [SerializeField] private int maxHp; //최대 체력
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }

    [SerializeField] private int hp; //현재 체력
    public int Hp
    {
        get => hp;
        set => hp = value > 0 ? value : 0;
    }

    [SerializeField] private int attackDamage; //공격력
    public int AttackDamage
    {
        get => attackDamage;
        set => attackDamage = value;
    }

    [SerializeField] private float defense; //방어력
    public float Defense
    {
        get => defense;
        set => defense = value;
    }

    [SerializeField] private float moveSpeed; //이동속도
    public float MoveSpeed
    {
        get => moveSpeed * 0.1f;
        set => moveSpeed = value;
    }

    protected bool isDeath = false;          //죽었는가
    protected bool isInvincibility = false;  //무적 유무

    public virtual void Awake()
    {
        InitAbility();
    }
    private void InitAbility()
    {
        Hp = MaxHp;
        isDeath = false;
    }
    public virtual void Death() { isDeath = true; }

    public virtual void Damaged(int Damage)
    {
        Hp -= Damage;
        if (Hp <= 0)
        {
            Hp = 0;
            Death();
        }
    }
}
