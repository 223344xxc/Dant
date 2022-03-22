using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    PlayerCtrl player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        "trigger".Log();
        if (collision.CompareTag("MONSTER"))
        {
            "monster".Log();
            player.Damaged(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MONSTER"))
        {
            player.Damaged(1);
        }
    }
}
