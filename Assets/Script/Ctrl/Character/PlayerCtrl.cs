using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None,
    NormalAttack,
    TopAttack,
    JumpTopAttack,
    JumpBottonAttack
}

public enum AttackCollider
{
    None,
    NormalAttack,
    TopAttack
}

public class PlayerCtrl : PlayerAbility
{
    public GameObject TopAttackEffect;
    public GameObject[] AttackColliders;

    private Rigidbody2D rigid;
    private Animator anim;

    private bool IsJump = false;
    private bool IsAttack = false;
    private bool IsDash = false;

    private float DashDistance = 0;
    private bool PassibleMove = true;
    private bool IsJumpTopAttack = false;

    private Vector2 moveVector;
    public Vector2 MoveVector
    {
        get
        {
            if (IsDash)
            {
                moveVector.Set(DashDistance * MoveSpeed * DashSpeed, rigid.velocity.y);
                return moveVector;
            }

            if (PassibleMove)
                moveVector.Set(IsJump ? rigid.velocity.x : JoyStickCtrl.JoyStickPosition.x * MoveSpeed, rigid.velocity.y);
            else
                moveVector.Set(IsJump ? rigid.velocity.x : 0, rigid.velocity.y);
            if (Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) > 3)
                TempScale.Set(JoyStickCtrl.JoyStickPosition.x > 0 ? -1 * StartScale.x : 1 * StartScale.x, StartScale.y, StartScale.z);

            return moveVector;
        }
    }

    private Vector3 StartScale;
    private Vector3 TempScale;
    public Vector3 MoveScale
    {
        get => TempScale;
    }

    private Vector2 JumpVelocity;
    public Vector2 JumpVector
    {
        get
        {
            JumpVelocity.Set(0, JumpPower);
            return JumpVelocity;
        }
    }

    private Vector3 DumyVector;

    public delegate void AttackToObject(float Damage, Vector3 PlayerPos);
    public static AttackToObject AttackHitObject;

    public AttackType NowAttackType;

    public void Awake()
    {
        InitPlayer();
    }
    public void Update()
    {
        PlayerMove();
    }

    private void Temp(float num, Vector3 vec) { }

    private void InitPlayer()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartScale = transform.localScale;
        TempScale = StartScale;
        AttackHitObject += Temp;
    }

    private void PlayerMove()
    {
        rigid.velocity = MoveVector;
        transform.localScale = MoveScale;


        if (Input.GetKeyDown(KeyCode.Space) && !IsJump)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Dash();
        }

        if (JoyStickCtrl.StickFollow && Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) > 3)
        {
            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("GROUND"))
        {
            IsJumpTopAttack = false;
            IsJump = false;
        }
    }


    private void Jump()
    {
        rigid.AddForce(JumpVector);
        IsJump = true;
    }

    private void Attack()
    {
        if (IsAttack)
            return;

        if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == false)
            TopAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == true)
            JumpTopAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.DOWN && IsJump == true)
            JumpBottomAttack();
        else
            NormalAttack();
    }

    private void NormalAttack()
    {
        NowAttackType = AttackType.NormalAttack;
        anim.Play("Attack_1");
    }

    private void TopAttack()
    {
        GameObject Effect = Instantiate(TopAttackEffect);
        Effect.transform.position = transform.position;
        DumyVector.Set(transform.localScale.x < 0 ? -1 * Effect.transform.localScale.x : 1 * Effect.transform.localScale.x, Effect.transform.localScale.y, Effect.transform.localScale.z);
        Effect.transform.localScale = DumyVector;
        IsJump = true;
        anim.Play("TopAttack");
        rigid.AddForce(Vector2.up * 2000);
    }

    private void JumpBottomAttack()
    {
        anim.Play("JumpBottomAttack");
    }

    private void JumpTopAttack()
    {
        if (IsJumpTopAttack)
            return;
        IsJumpTopAttack = true;
        rigid.AddForce(Vector2.up * 2000);
        anim.Play("JumpTopAttack");

    }

    private void SkillGrap()
    {
        anim.Play("Attack_Grap");
    }

    private void Dash()
    {
        if (!IsDash && PassibleMove)
            anim.Play("Dash");
    }

    public void StartDash()
    {
        DashDistance = transform.localScale.x > 0 ? -1 : 1;
        PassibleMove = false;
        IsDash = true;
    }

    public void EndDash()
    {
        DashDistance = 0;
        PassibleMove = true;
        IsDash = false;
    }

    public void SetAttack()
    {
        IsAttack = true;
    }

    public void AttackRelease()
    {
        IsAttack = false;
    }

    public void MoveStop()
    {
        PassibleMove = false;
    }

    public void MoveRelease()
    {
        PassibleMove = true;
    }

    public void ActiveAttackArea()
    {
        AttackColliders[(int)NowAttackType].SetActive(!AttackColliders[(int)NowAttackType].activeSelf);
    }

    public void AttackToHitObject()
    {
        AttackHitObject(1, transform.position);
    }
}
