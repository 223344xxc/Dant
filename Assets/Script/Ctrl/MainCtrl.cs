using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCtrl : MonoBehaviour
{
    LoadingCtrl loading;
    public static string NowScene = "StartScene";
    public static bool nowSceneLoauded = true;
    void Start()
    {
        loading = GetComponent<LoadingCtrl>();
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string name)
    {
        nowSceneLoauded = false;
        loading.StartLoading(name);
        NowScene = name;
    }
}
