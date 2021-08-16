using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyCtrl : MonoBehaviour
{
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    //CharacterController controller;

    [Header("적 능력치")]
    public float maxHp;
    public float hp;
    public float PowerX, PowerY;


    public float Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                animator.Play("Death");
                hp = 0;

            }
            if (hp > maxHp)
                hp = maxHp;
        }
    }


    [Header("적 옵션")]
    [Tooltip("중력값")]
    public float Gravity;
    [Tooltip("움직이는 속도")]
    public float Speed;

    [Header("적 상태")]
    public bool IsMove = true;
    public bool IsGround = false;

    [Tooltip("피격 이펙트 프리펩")]
    public GameObject HitEffectPrefab;
    public GameObject DeathEffectPrefab;

    public Color HitColor;
    Color StartColor;

    Vector3 MoveDir;
    Vector3 StartScale;

    Vector3 MoveVel
    {
        get
        {
            if (IsGround == false)
                MoveDir.y -= Gravity;
            if (IsMove)
            {

            }
            else
            {
                MoveDir.x = 0;
            }

            if (MoveDir.x < 0)
                StartScale.x = Mathf.Abs(gameObject.transform.localScale.x) * -1;
            else if (MoveDir.x > 0)
                StartScale.x = Mathf.Abs(gameObject.transform.localScale.x);

            gameObject.transform.localScale = StartScale;
            return MoveDir;
        }
        set
        {

        }
    }
    
    void Awake()
    {
        InitEnemy();    
    }

    void InitEnemy()
    {
        StartScale = gameObject.transform.localScale;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        StartColor = spriteRenderer.color;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    
        UpdateColor();
    }

    void UpdateColor()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, StartColor, 0.05f);
    }

    public void Damaged(float Damage, Vector3 playerPos, float ImpulseForce = 0, Action Event=null)
    {
        Vector3 ImpulseDir = transform.position - playerPos;
        ImpulseDir.Set(ImpulseDir.x > 0 ? 500 * ImpulseForce : -500 * ImpulseForce, 0, 0);

        rigidbody.AddForce(ImpulseDir);
        spriteRenderer.color = HitColor;
        Hp -= Damage;
        Event?.Invoke();
        if (Hp > 0)
        {
            Destroy(Instantiate(HitEffectPrefab, transform), 1);
        }
        else
        {
            Destroy(Instantiate(DeathEffectPrefab, transform), 1);
            PlayerCtrl.RemoveAttackToObject(Damaged);
            Destroy(gameObject, 3);
        }
    }

    public void AddForceToUp()
    {
        Vector3 ImpulseDir = Vector3.zero;
        ImpulseDir.x = PowerX;
        ImpulseDir.y = PowerY;
        rigidbody.AddForce(ImpulseDir * 0.35f);
    }

    void Move()
    {
        //rigidbody.velocity = MoveVel * Time.deltaTime;
        animator.SetFloat("MoveVel", rigidbody.velocity.x);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            IsGround = true;
            MoveDir.y = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            IsGround = false;
        }
    }

}
