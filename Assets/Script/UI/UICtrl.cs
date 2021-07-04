using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    public GameObject PauseUi;

    public void PauseGame()
    {
        if (Time.timeScale != 0)
        {
            PauseUi.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            PauseUi.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
