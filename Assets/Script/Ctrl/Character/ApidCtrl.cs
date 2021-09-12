using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApidCtrl : EnemyCtrl
{
    [Header("진딧물 옵션")]
    [SerializeField] private float attackDelay;
    [SerializeField] private float AttackRange;
    [SerializeField] private Vector3 AttackOffset;
    [SerializeField] private GameObject apidBulletPrefab;

    bool passibleAttack = true;
    public override Vector3 MoveVel
    {
        get
        {
            var distance = Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position);

            if (distance >= 1.5f && distance <= trackingRadius && !isDeath)
                moveVel.x = PlayerCtrl.Instance.transform.position.x - transform.position.x > 0 ? MoveSpeed * 10 : -MoveSpeed * 10;
            else
                moveVel.x = 0;

            if (AttackRange > Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position))
                moveVel.x = 0;


            moveVel.y = rigidbody.velocity.y;
            if (Mathf.Abs(moveVel.x) > 0)
                moveScale.x = moveVel.x > 0 ? -Mathf.Abs(StartScale.x) : Mathf.Abs(StartScale.x);
            return moveVel;
        }
        set => moveVel = value;
    }

    protected override void Update()
    {
        base.Update();

        if (!isDeath && passibleAttack && AttackRange > Vector3.Distance(PlayerCtrl.Instance.transform.position, trakingOffset + transform.position))
        {
            ApidAttack();
        }
    }

    protected override void EnemyRigidbodyUpdate()
    {
        if (isDeath)
            return;
        rigidbody.velocity = MoveVel;
    }

    private void ApidAttack()
    {
        animator.Play("Attack");
        passibleAttack = false;
        Invoke("TurnAttackState", attackDelay);
    }

    private void TurnAttackState()
    {
        passibleAttack = true;
    }

    public void SpawnProjectile()
    {
        ApidProjectile projectile = Instantiate(apidBulletPrefab).GetComponent<ApidProjectile>();
        projectile.gameObject.transform.position = transform.position + AttackOffset;
        projectile.moveDirection = PlayerCtrl.Instance.transform.position - projectile.gameObject.transform.position + PlayerCtrl.Instance.playerCenterOffset;
        projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(projectile.transform.position.y - PlayerCtrl.Instance.transform.position.y ,
                                                                         projectile.transform.position.x - PlayerCtrl.Instance.transform.position.x) * Mathf.Rad2Deg);
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
