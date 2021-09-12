using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApidCtrl : EnemyCtrl
{
    [Header("진딧물 옵션")]
    [SerializeField] private float AttackRange;
    [SerializeField] private Vector3 AttackOffset;

    protected override void Update()
    {
        base.Update();

        if (!isDeath && AttackRange > Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position))
        {
            ApidAttack();
        }
    }

    protected override void EnemyRigidbodyUpdate()
    {
        Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position).LogError();
        if (isDeath || AttackRange > Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position))
            return;
        rigidbody.velocity = MoveVel;
    }

    private void ApidAttack()
    {
        "attack".LogError();
        animator.Play("Attack");
    }

    public override void Death()
    {
        base.Death();
        rigidbody.gravityScale = 9.81f;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + AttackOffset, AttackRange);
    }
#endif
}
