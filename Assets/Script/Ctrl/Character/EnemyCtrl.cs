using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyCtrl : Ability
{
    protected Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    protected Animator animator;

    [Header("적 능력치")]
    [SerializeField] private float PowerX, PowerY;
    [SerializeField] private float trackingRadius;
    [SerializeField] protected Vector3 trakingOffset; 

    private bool IsMove = true;
    private bool IsGround = false;

    [Tooltip("피격 이펙트 프리펩")]
    [SerializeField] private GameObject HitEffectPrefab;
    [SerializeField] private GameObject DeathEffectPrefab;

    [SerializeField] private Color HitColor;
    private Color startColor;

    protected Vector3 StartScale;
    public Vector3 moveVel;
    public virtual Vector3 MoveVel
    {
        get
        {
            var distance = Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position);

            if (distance >= 1.5f && distance <= trackingRadius && !isDeath)
                moveVel.x = PlayerCtrl.Instance.transform.position.x - transform.position.x > 0 ? MoveSpeed * 10 : -MoveSpeed * 10;
            else
                moveVel.x = 0;

            moveVel.y = rigidbody.velocity.y;
            if (Mathf.Abs(moveVel.x) > 0)
                moveScale.x = moveVel.x > 0 ? -Mathf.Abs(StartScale.x) : Mathf.Abs(StartScale.x);
            return moveVel;
        }
        set => moveVel = value;
    }

    protected Vector3 moveScale;
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

    private void InitEnemy()
    {
        StartScale = gameObject.transform.localScale;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startColor = spriteRenderer.color;
        moveScale = StartScale;
    }

    protected virtual void Update()
    {
        Move();
        UpdateColor();
    }

    protected virtual void FixedUpdate()
    {
        EnemyRigidbodyUpdate();
    }

    protected virtual void EnemyRigidbodyUpdate()
    {
        if (isDeath)
            return;
        rigidbody.velocity = MoveVel;
    }

    private void UpdateColor()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, startColor, 0.05f);
    }

    public void Damaged_Enemy(int damage, float ImpulseForce = 0, Action Event=null)
    {
        Vector3 ImpulseDir = transform.position - PlayerCtrl.Instance.transform.position;
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
            Destroy(Instantiate(HitEffectPrefab, transform), 1);
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
        ImpulseDir.x = PowerX * transform.localScale.x;
        ImpulseDir.y = PowerY;

        try
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        catch
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        rigidbody.AddForce(ImpulseDir * 0.35f);
    }
    public void DestroyEvent()
    {
        Destroy(gameObject);
    }

    private void Move()
    {
        animator.SetFloat("MoveVel", Mathf.Abs(rigidbody.velocity.x));
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


#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(trakingOffset + transform.position, trackingRadius);
    }
#endif
}
