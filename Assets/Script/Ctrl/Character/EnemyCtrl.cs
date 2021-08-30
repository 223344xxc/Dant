using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyCtrl : Ability
{
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    Animator animator;

    [Header("적 능력치")]
    public float PowerX, PowerY;
    public float Radius;
    public Vector3 circleOffset;
    public LayerMask layer;

    [Header("적 옵션")]
    [Tooltip("중력값")]
    public float Gravity;

    [Header("적 상태")]
    public bool IsMove = true;
    public bool IsGround = false;

    [Tooltip("피격 이펙트 프리펩")]
    public GameObject HitEffectPrefab;
    public GameObject DeathEffectPrefab;


    [SerializeField] private Color HitColor;
    private Color startColor;

    private Vector3 StartScale;
    public Vector3 moveVel;
    public Vector3 MoveVel
    {
        get
        {
            var distance = Vector3.Distance(PlayerCtrl.Instance.transform.position, transform.position);
  
            moveScale.x = moveVel.x > 0 ? -Mathf.Abs(StartScale.x) : Mathf.Abs(StartScale.x);
            return moveVel;
        }
        set => moveVel = value;
    }

    private Vector3 moveScale;
    public Vector3 MoveScale
    {
        get => moveScale;
        set => moveScale = value;
    }

    public override void Awake()
    {
        base.Awake();
        InitEnemy();    
    }

    void InitEnemy()
    {
        StartScale = gameObject.transform.localScale;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startColor = spriteRenderer.color;
        moveScale = StartScale;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
        Move();
    
        UpdateColor();
    }

    private void FixedUpdate()
    {
  
        EnemyRigidbodyUpdate();
    }

    private void EnemyRigidbodyUpdate()
    {
        rigidbody.velocity = MoveVel;
    }

    private void UpdateColor()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, startColor, 0.05f);
    }

    public void Damaged_Enemy(int damage, Vector3 playerPos, float ImpulseForce = 0, Action Event=null)
    {
        Vector3 ImpulseDir = transform.position - playerPos;
        ImpulseDir.Set(ImpulseDir.x > 0 ? 500 * ImpulseForce : -500 * ImpulseForce, 0, 0);

        rigidbody.AddForce(ImpulseDir);
        spriteRenderer.color = HitColor;

        Damaged(damage);
        Event?.Invoke();
        if (Hp > 0)
        {
            Destroy(Instantiate(HitEffectPrefab, transform), 1);
        }
        else
        {
            Destroy(Instantiate(DeathEffectPrefab, transform), 1);
            PlayerCtrl.RemoveAttackToObject(Damaged_Enemy);
            Destroy(gameObject, 3);
        }
    }
    public override void Death()
    {
        base.Death();
        animator.Play("Death");
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
        animator.SetFloat("MoveVel", rigidbody.velocity.x);
        transform.localScale = MoveScale;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            IsGround = true;
            moveVel.y = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            IsGround = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + circleOffset, Radius);
    }
}
