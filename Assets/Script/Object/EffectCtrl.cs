using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoBehaviour
{
    [Header("이펙트 속성")]
    [Tooltip("이펙트 스프라이트")]
    public Sprite[] image;
    [Tooltip("이펙트 반복 유무")]
    public bool IsLoop = false;
    int SpriteIndex = 0;
    

    [Tooltip("이펙트 지연시간")]
    public float DelayTime;
    float TempTime;

    public GameObject parent;

    SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        InitEffect();
    }

    void InitEffect()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }
    
    void Update()
    {
        TempTime += Time.deltaTime;

        if(TempTime >= DelayTime)
        {
            TempTime = 0;
            SpriteIndex += 1;
            spriteRenderer.sprite = image[SpriteIndex];

            if (SpriteIndex >= image.Length - 1)
            {
                if (IsLoop)
                    SpriteIndex = 0;
                else
                    Destroy(parent);
            }
        }
    }
}
