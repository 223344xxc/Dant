using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    [SerializeField] protected float Speed;

    protected virtual void Awake()
    {
        Destroy(gameObject, 10);
    }

    protected void Update()
    {
        ProjectileUpdate();
    }

    protected virtual void ProjectileUpdate()
    {
        transform.position += transform.localScale.x > 0 ?
                             -transform.right * Time.deltaTime * Speed :
                              transform.right * Time.deltaTime * Speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MONSTER"))
        {
            EnemyCtrl ec = collision.gameObject.GetComponent<EnemyCtrl>();

            ec.Damaged_Enemy(PlayerCtrl.Instance.AttackDamage);
        }

    }
}
