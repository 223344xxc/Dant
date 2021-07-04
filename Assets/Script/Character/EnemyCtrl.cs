using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        get
        {
            return hp;
        }
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

    void Damaged(float Damage, Vector3 playerPos)
    {
        Vector3 ImpulseDir = transform.position - playerPos;
        //ImpulseDir.y += 10;
        rigidbody.AddForce(ImpulseDir.normalized * 500);
        spriteRenderer.color = HitColor;
        Hp -= Damage;
        if (Hp > 0)
        {
            Destroy(Instantiate(HitEffectPrefab, transform), 1);
        }
        else
        {
            Destroy(Instantiate(DeathEffectPrefab, transform), 1);
            Destroy(gameObject, 3);
        }
        PlayerCtrl.ComboResetTime = PlayerCtrl.ResetTime;
        PlayerCtrl.CurrentCombo += 1;
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
        if (collision.gameObject.CompareTag("ATTACKAREA"))
        {
            PlayerCtrl.attack += Damaged;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ATTACKAREA"))
        {
            PlayerCtrl.attack += Damaged;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            IsGround = false;
        }
        if (collision.gameObject.CompareTag("ATTACKAREA"))
        {
            PlayerCtrl.attack -= Damaged;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ATTACKAREA"))
        {
            PlayerCtrl.attack -= Damaged;
        }
    }

}
