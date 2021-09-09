using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextCtrl : MonoBehaviour
{
    private Text ComboText;
    private Text BackComboText;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ComboText = GetComponent<Text>();
        BackComboText = transform.Find("ShadowText").GetComponent<Text>();
    }

    private void Start()
    {
        PlayerCtrl.UpdatePlayerUI = (PlayerCtrl player) => {
            anim.Play("ComboAnim");
            ComboText.text = player.Combo + " Combo";
            BackComboText.text = player.Combo + " Combo";
        };
    }
}
