using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCtrl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MONSTER"))
        {
            PlayerCtrl.AddAttackToObject(collision.gameObject.GetComponent<EnemyCtrl>().Damaged);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MONSTER"))
        {
            PlayerCtrl.RemoveAttackToObject(collision.gameObject.GetComponent<EnemyCtrl>().Damaged);
        }
    }
}
