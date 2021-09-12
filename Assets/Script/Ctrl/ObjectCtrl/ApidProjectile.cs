using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApidProjectile : ProjectileCtrl
{
    public Vector3 moveDirection;
    [SerializeField] private GameObject HitEffect;

    protected override void ProjectileUpdate()
    {
        transform.position += moveDirection.normalized * Time.deltaTime * Speed;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCtrl.Instance.Damaged(1);
            Instantiate(HitEffect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
