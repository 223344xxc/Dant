using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    private MainCtrl main;
    private void Start()
    {
        main = GameObject.FindGameObjectWithTag("MAIN").GetComponent<MainCtrl>();        
    }

    public void SelectStage(string StageName)
    {
        main.LoadScene(StageName);
    }
}
