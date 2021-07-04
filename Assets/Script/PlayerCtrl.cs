using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Vector3 MoveDir = Vector3.zero;
    Vector3 PlayerStartingScale;
    Vector3 TempVector;


    public float MoveSpeed = 10;
    float TempMoveSpeed = 0;

    public Animator animator;

    Rigidbody PlayerRigid;

    void Start()
    {
        InitPlayer();
    }

    void Update()
    {
        PlayerMove();
    }

    void InitPlayer()
    {
        PlayerStartingScale = transform.localScale;
        PlayerRigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }


    void PlayerMove()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (MoveDir.x > 0)
        {
            TempVector = PlayerStartingScale;
            TempVector.x = -PlayerStartingScale.x;
            transform.localScale = TempVector;
        }
        else if (MoveDir.x < 0)
        {
            TempVector = PlayerStartingScale;
            transform.localScale = TempVector;
        }

        TempVector.x = MoveDir.x * MoveSpeed;
        TempVector.y = PlayerRigid.velocity.y;
        TempVector.z = PlayerRigid.velocity.z;

        PlayerRigid.velocity = TempVector;
        animator.SetFloat("MoveVel", Mathf.Abs(PlayerRigid.velocity.x));
        
    }

    void ResetMoveSpeed()
    {
        MoveSpeed = TempMoveSpeed;
    }

    void ChangeAttackStateToFalse()
    {
        animator.SetBool("IsAttack", false);
    }

    void ChangeAttackStateToTrue()
    {

        animator.SetBool("IsAttack", true);
    }

    void ChangeNextAttackToFalse()
    {
        animator.SetBool("NextAttack", false);
    }

    void ChangeNextAttackToTrue()
    {
        animator.SetBool("NextAttack", true);
    }


    void Attack()
    {
        if(animator.GetBool("IsAttack") == true)
        {
            animator.SetBool("NextAttack", true);
        }
        animator.SetBool("IsAttack", true);
    }

    void Jump()
    {
        PlayerRigid.AddForce(0, 300, 0);
    }

    void Dash()
    {
        animator.SetTrigger("IsDash");
        TempMoveSpeed = MoveSpeed;
        MoveSpeed *= 3;
    }
}
