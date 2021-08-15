using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextCtrl : MonoBehaviour
{
    public Text ComboText;
    public Text BackComboText;


    private void Start()
    {
        PlayerCtrl.AddUpdateUIFun((PlayerCtrl player) => {
            ComboText.text = player.Combo + " Combo";
            BackComboText.text = player.Combo + " Combo";
        });
    }
}
