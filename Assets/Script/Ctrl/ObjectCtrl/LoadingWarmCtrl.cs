using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingWarmCtrl : MonoBehaviour
{
    [SerializeField] private float rotSpeed;

    private void Awake()
    {
        StartCoroutine(rotWarm());
    }


    IEnumerator rotWarm()
    {
        while (true)
        {
            gameObject.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
