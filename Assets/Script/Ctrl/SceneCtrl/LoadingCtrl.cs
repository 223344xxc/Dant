using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCtrl : MonoBehaviour
{
    public bool LoadEnd = false;
    AsyncOperation op;

    public void StartLoading(string sceneName)
    {
        op = SceneManager.LoadSceneAsync(sceneName);

        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);

        StartCoroutine(Loading());
        
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(2f);
        while (!op.isDone)
        {
            yield return null;
        }
        EndLoading();
    }



    public void EndLoading()
    {
        SceneManager.UnloadSceneAsync("Loading");
    }
}
