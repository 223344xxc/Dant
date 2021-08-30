using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarCtrl : MonoBehaviour
{
    [SerializeField] private SpriteAnim[] harts;
    [SerializeField] private Sprite[] HartDeathSprites;
    

    public void HpBarDamage(int hp)
    {
        if (hp == 0)
        {
            harts[hp].Delay = 0.1f;
            harts[0].SpriteCastAnimation(HartDeathSprites, () => harts[0].SetVisible(false));
            return;
        }

        harts[hp].Delay = 0.1f;
        harts[hp].SpriteCastAnimation(HartDeathSprites, () => harts[hp].SetVisible(false));
        harts[hp - 1].SetAnimState(AnimState.Play);
    }
}
