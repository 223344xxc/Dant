using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None,
    NormalAttack,
    TopAttack,
}

public class PlayerCtrl : PlayerAbility
{
    private Rigidbody2D rigid;
    private Animator anim;

    public bool IsJump = false;
    public bool IsAttack = false;
    private bool PassibleMove = true;

    private Vector2 MoveVector;
    public Vector2 MoveVec
    {
        get
        {
            if (PassibleMove)
                MoveVector.Set(IsJump ? rigid.velocity.x : JoyStickCtrl.JoyStickPosition.x * Speed, rigid.velocity.y);
            else
                MoveVector.Set(IsJump ? rigid.velocity.x : 0, rigid.velocity.y);

            if (Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) > 3)
                TempScale.Set(JoyStickCtrl.JoyStickPosition.x > 0 ? -1 * StartScale.x : 1 * StartScale.x, StartScale.y, StartScale.z);
            return MoveVector;
        }
    }

    private Vector3 StartScale;
    private Vector3 TempScale;
    public Vector3 MoveScale
    {
        get
        {
            return TempScale;
        }
    }

    private Vector2 JumpVelocity;
    public Vector2 JumpVector
    {
        get
        {
            JumpVelocity.Set(0, JP);
            return JumpVelocity;
        }
    }




    public GameObject TopAttackEffect;

    private Vector3 DumyVector;

    public override void Start()
    {
        base.Start();
        InitPlayer();
    }

    void InitPlayer()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartScale = transform.localScale;
        TempScale = StartScale;
    }

    public override void Update()
    {
        base.Update();
        PlayerMove();
    }

    void PlayerMove()
    {
        rigid.velocity = MoveVec;
        transform.localScale = MoveScale;

        if (Input.GetKeyDown(KeyCode.Space) && !IsJump)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if(JoyStickCtrl.StickFollow && Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) > 3)
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
            IsJump = false;
        }
    }


    void Jump()
    {
        rigid.AddForce(JumpVector);
        IsJump = true;
    }

    void Attack()
    {
        if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == false)
            TopAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == true)
            JumpTopAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.DOWN && IsJump == true)
            JumpBottomAttack();
        else
            NormalAttack();
    }

    void NormalAttack()
    {
        anim.Play("Attack_1");
    }

    void TopAttack()
    {
        GameObject Effect = Instantiate(TopAttackEffect);
        Effect.transform.position = transform.position;
        DumyVector.Set(transform.localScale.x < 0 ? -1 * Effect.transform.localScale.x : 1 * Effect.transform.localScale.x, Effect.transform.localScale.y, Effect.transform.localScale.z);
        Effect.transform.localScale = DumyVector;
        
        IsJump = true;
        anim.Play("TopAttack");
        rigid.AddForce(Vector2.up * 2000);
    }

    void JumpBottomAttack()
    {
        anim.Play("JumpBottomAttack");
    }

    void JumpTopAttack()
    {
        rigid.AddForce(Vector2.up * 2000);
        anim.Play("JumpTopAttack");
    }

    void SkillGrap()
    {
        anim.Play("Attack_Grap");
    }

    void Dash()
    {
        anim.Play("Dash");
    }

    public void MoveStop()
    {
        PassibleMove = false;
    }

    public void MoveRelease()
    {
        PassibleMove = true;
    }
}
