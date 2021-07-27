using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
    private string sceneName;
    public string SceneName
    {
        get => sceneName;
        set => sceneName = value;
    }


    void Start()
    {
        //씬 넘기기 테스트
        sceneName = "StageSelectScene";
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //임시 씬 전환.. 나중에 구조를 바꿀 예정
        if (Input.GetMouseButton(0) && sceneName != "None") {
            SceneCtrl.LoadScene(SceneName);
            
            if(sceneName == "StageSelectScene")
            {
                sceneName = "InGameScene";
            }
            else
            {
                sceneName = "None";
            }
        }

    }
}
