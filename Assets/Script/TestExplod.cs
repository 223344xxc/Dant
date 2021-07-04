using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExplod : MonoBehaviour
{
    public GameObject ExplodPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(ExplodPrefab);
            transform.localScale = Vector3.zero;
            Invoke("Reset_I", 1);
        }
    }

    void Reset_I()
    {
        transform.localScale = Vector3.one;
    }

}
