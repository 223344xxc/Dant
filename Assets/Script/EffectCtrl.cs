using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoBehaviour
{
   
   public void DestroyEffect()
    {
        Destroy(transform.parent.gameObject);
    }
}
