using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum AnimState
{
    Stop,
    Play,
}

public class SpriteAnim : MonoBehaviour
{
    [Header("스프라이트 설정")]
    [SerializeField] private Sprite[] sprites;
    private Sprite[] tempSprites;
    [SerializeField] private float delay;
    public float Delay
    {
        get => delay;
        set => delay = value;
    }
    [SerializeField] private AnimState animState;
    [SerializeField] private bool doEndDestroy = false;
    private Image image;
    private SpriteRenderer spriteRenderer;
    private bool SpriteType = false; // true : 이미지     false : 스프라이트 렌더러
    private float time;
    private int spriteIndex = 0;

    private bool isCasting = false;

    Action eventFun;

    private void Awake()
    {
        InitSpriteAnim();
    }

    private void InitSpriteAnim()
    {
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer)
            SpriteType = false;
        else if (image)
            SpriteType = true;

        SetSprite(sprites[0]);
    }
    private void SetSprite(Sprite sprite)
    {
        if (SpriteType)
            image.sprite = sprite;
        else
            spriteRenderer.sprite = sprite;
    }

    private void Update()
    {
        UpdateSprite();   
    }

    public void SpriteCastAnimation(Sprite[] castAnimSprites, Action endEvent)
    {
        OptionClass.CopyArr(ref tempSprites, sprites);
        OptionClass.CopyArr(ref sprites, castAnimSprites);
        isCasting = true;
        eventFun = endEvent;
        SetVisible(true);
        SetAnimState(AnimState.Play);
    }
   

    private void ReleaseCastAnimation()
    {
        isCasting = false;
        OptionClass.CopyArr(ref sprites, tempSprites);
        SetAnimState(AnimState.Stop);
    }

    private void UpdateSprite()
    {
        if (animState == AnimState.Stop)
            return;

        time += Time.deltaTime;
        if(time >= delay)
        {
            time = 0;
            if(spriteIndex == sprites.Length - 1)
            {
                if (isCasting)
                {
                    ReleaseCastAnimation();
                    eventFun?.Invoke();
                }
                if (doEndDestroy)
                    Destroy(gameObject);
        
            }

            spriteIndex = (spriteIndex + 1) % sprites.Length;
            SetSprite(sprites[spriteIndex]);
        }
    }

    public void SetAnimState(AnimState state)
    {
        switch (state)
        {
            case AnimState.Stop:
                animState = AnimState.Stop;
                spriteIndex = 0;
                SetSprite(sprites[spriteIndex]);
                break;
            case AnimState.Play:
                spriteIndex = 0;
                SetSprite(sprites[spriteIndex]);
                time = 0;
                animState = AnimState.Play;
                break;
            default:
                break;
        }
    }

    public void SetVisible(bool set)
    {
        image.enabled = set;
    }
}
