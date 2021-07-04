using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackArea{
    NormalAttack,
    TopAttackArea,
}

public class PlayerCtrl : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [Header("플레이어 옵션")]
    [Tooltip("중력값")]
    public float Gravity;
    [Tooltip("움직이는 속도")]
    public float Speed;
    [Tooltip("점프력")]
    public float JumpPower;
    [Tooltip("콤보 수")]
    public static int CurrentCombo = 0;
    public static float ResetTime = 3;
    public static float ComboResetTime;


    [Header("플레이어 상태")]
    public bool IsMove = true;
    public bool IsAttack = false;
    public bool NextAttackAccept = false;
    public bool IsGround = true;
    public bool IsJump = false;



    [Header("프리펩")]
    [Tooltip("위 공격 이펙트 프리펩")]
    public GameObject TopAttackEffectPrefab;

    public GameObject[] attackArea;

    public Vector3 MoveDir;
    Vector3 AnimAddPos;
    Vector3 StartScale;
    Animator animator;

    public delegate void AttackMonster(float Damage, Vector3 playerPos);
    public static event AttackMonster attack;

    Vector3 MoveVel
    {
        get
        {
            if (IsGround == false)
                MoveDir.y -= Gravity;
            if (IsMove)
            {
                //MoveDir.x = Input.GetAxisRaw("Horizontal") * Speed;
                MoveDir.x = Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) < 0.3f ? 0 : JoyStickCtrl.JoyStickPosition.x * 0.05f;

                animator.SetFloat("MoveVel", Mathf.Abs(MoveDir.x));
            }
            else
            {
                MoveDir.x = 0;
                animator.SetFloat("MoveVel", Mathf.Abs(MoveDir.x));
            }

            if (MoveDir.x < 0)
                StartScale.x = Mathf.Abs(gameObject.transform.localScale.x);
            else if (MoveDir.x > 0)
                StartScale.x = Mathf.Abs(gameObject.transform.localScale.x)* -1;

            gameObject.transform.localScale = StartScale;
            
            return MoveDir;
        }
        set
        {
            
        }
    }

    void Awake()
    {
        InitPlayer();
    }
    
    void InitPlayer()
    {
        StartScale = gameObject.transform.localScale;
        //controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        for(int i = 0; i < attackArea.Length; i++)
        {
            attackArea[i].SetActive(false);
        }
    }
    void Start()
    {
        
    }
    void Update()
    {
        //    Move();
        UpdateCombo();
    }
    
    void FixedUpdate()
    {
        Move();    

    }

    void UpdateCombo()
    {
        ComboResetTime -= Time.deltaTime;
        if(ComboResetTime <= 0)
        {
            CurrentCombo = 0;
        }
    }

    void Move()
    {

        if (Input.GetKeyDown(KeyCode.Space) && IsMove)
        {
            rigidbody.AddForce(new Vector2(0, 3000));
            animator.Play("Jump");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //StartCoroutine(Dash());
            animator.Play("Dash");  
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.Play("TopAttack");
            GameObject Effect = Instantiate(TopAttackEffectPrefab);
            Effect.transform.position += gameObject.transform.position;
            Effect.transform.localScale = transform.localScale;
            rigidbody.AddForce(new Vector2(0, 2000));
        }

        rigidbody.velocity = new Vector2((MoveVel * Speed * 1).x, rigidbody.velocity.y);
        Attack();
       
    }

    public void Jump()
    {
        if (IsMove)
        {
            rigidbody.AddForce(new Vector2(0, JumpPower * 1000));
            animator.Play("Jump");
            IsJump = true;
        }
    }

    public void NextAttackToFalse()
    {
        NextAttackAccept = false;
    }
    public void NextAttackToTrue()
    {
        NextAttackAccept = true;
    }

    public void StartAttack()
    {
        animator.SetBool("NextAttack", false);
        IsAttack = true;
    }

    public void StopAttack()
    {
        IsAttack = false;
    }

    public void PlayerStop()
    {
        IsMove = false;
        animator.SetBool("PlayerMove", false);
    }
    public void PlayerMove()
    {
        IsMove = true;
        animator.SetBool("PlayerMove", true);
    }

    public void SetAnimPos(float PosX)
    {

        AnimAddPos.x = PosX;
    }


    public void Attack_()
    {
        if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && (IsMove && !IsAttack) && IsGround)
        {
            animator.Play("TopAttack");
            GameObject Effect = Instantiate(TopAttackEffectPrefab);
            Effect.transform.position += gameObject.transform.position;
            Effect.transform.localScale = transform.localScale;
            rigidbody.AddForce(new Vector2(0, 2000));

        }
        else if (IsMove && !IsAttack)
        {
            animator.Play("Attack_1");
            animator.SetBool("NextAttack", false);
        }

        if (!IsMove && NextAttackAccept)
            animator.SetBool("NextAttack", true);

        if (!IsGround && !IsAttack)
        {
            if (JoyStickCtrl.StickDirection == JoyStickDirection.DOWN)
            {
                animator.Play("JumpBottomAttack");
            }

            if (JoyStickCtrl.StickDirection == JoyStickDirection.UP)
            {
                animator.Play("JumpTopAttack");
            }
        }
    }

    void Attack()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(Input.GetKey(KeyCode.W) && (IsMove && !IsAttack) && IsGround)
        //    {
        //        animator.Play("TopAttack");
        //        GameObject Effect = Instantiate(TopAttackEffectPrefab);
        //        Effect.transform.position += gameObject.transform.position;
        //        Effect.transform.localScale = transform.localScale;
        //        MoveDir.y = 50;
                
        //    }
        //    else if (IsMove && !IsAttack)
        //    {
        //        animator.Play("Attack_1");
        //        animator.SetBool("NextAttack", false);
        //    }

        //    if (!IsMove && NextAttackAccept)
        //        animator.SetBool("NextAttack", true);

        //    if (!IsGround && !IsAttack)
        //    {
        //        if (Input.GetKey(KeyCode.S))
        //        {
        //            animator.Play("JumpBottomAttack");
        //        }

        //        if (Input.GetKey(KeyCode.W))
        //        {
        //            animator.Play("JumpTopAttack");
        //        }
        //    }
        //}
     

    }

    public void AttackToHitObject()
    {
        attack?.Invoke(10, transform.position);
        
    }

    public void ActiveAttackArea(int area)
    {
        attackArea[(int)area].SetActive(!attackArea[area].activeSelf);
    }


    public void Dash()
    {
        StartCoroutine(Dash_());
        animator.Play("Dash");
    }

    IEnumerator Dash_()
    {
        PlayerMove();
        float TempSpeed = Speed;
        Speed *=2;
        yield return new WaitForSeconds(0.3f);
        Speed = TempSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            IsJump = false;
            IsGround = true;
            MoveDir.y = 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND") && IsJump)
        {
            IsGround = true;
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
