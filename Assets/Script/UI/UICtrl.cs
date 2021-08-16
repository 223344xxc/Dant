using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    public GameObject PauseUi;
    public GameObject InGameUi;

    public void PauseGame()
    {
        if (Time.timeScale != 0)
        {
            PauseUi.SetActive(true);
            InGameUi.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            PauseUi.SetActive(false);
            InGameUi.SetActive(true);
            Time.timeScale = 1;
        }
    }

}
