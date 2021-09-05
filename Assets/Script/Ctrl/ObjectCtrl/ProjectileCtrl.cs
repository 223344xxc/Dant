using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    [SerializeField] private float Speed;

    private void Awake()
    {
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        transform.position += transform.localScale.x > 0 ? 
            -transform.right * Time.deltaTime * Speed : 
             transform.right * Time.deltaTime * Speed; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MONSTER"))
        {
            EnemyCtrl ec = collision.gameObject.GetComponent<EnemyCtrl>();

            ec.Damaged_Enemy(PlayerCtrl.Instance.AttackDamage);
        }

    }
}
