using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomBugCtrl : EnemyCtrl
{
    [Header("폭탄벌래 폭발 범위")]
    [SerializeField] private float explodRadius;
    [SerializeField] private Vector3 explodOffset;

    protected override void Update()
    {
        base.Update();

        ExplodChack();
    }

    private void ExplodChack()
    {
        if (Vector3.Distance(explodOffset + transform.position, PlayerCtrl.Instance.transform.position) < explodRadius)
        {
            MoveSpeed = 0;
            animator.Play("Explosion");
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explodOffset + transform.position, explodRadius);
    }
#endif
}
