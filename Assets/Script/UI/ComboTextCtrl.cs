using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextCtrl : MonoBehaviour
{
    [SerializeField] private Text ComboText;
    [SerializeField] private Text BackComboText;
    [SerializeField] private Animator anim;

    private bool canPlay = true;

    private void Start()
    {
        PlayerCtrl.AddUpdateUIFun((PlayerCtrl player) => {
            anim.Play("ComboAnim");
            ComboText.text = player.Combo + " Combo";
            BackComboText.text = player.Combo + " Combo";
        });
    }

    #region 이벤트
    public void StartPlay()
    {
        canPlay = false;
    }

    public void EndPlay()
    {
        canPlay = true;
    }
    #endregion
}
