using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICtrl : MonoBehaviour
{
    [SerializeField] private GameObject PauseUi;
    [SerializeField] private GameObject InGameUi;
    [SerializeField] private Image FadeImage;
    
    [SerializeField] private Text minText;
    [SerializeField] private Text secText;

    [SerializeField] private static int timeMin;
    [SerializeField] private static float timeSec;

    [SerializeField] private Color fadeColor;
    public float fadeSpeed;

    public void Awake()
    {
        PlayerCtrl.AddGameOverFun(FadeIn);
    }

    public void PauseGame()
    {
        
        if (Time.timeScale != 0)
        {
            PauseUi.SetActive(true);
            InGameUi.SetActive(false);
            GameOption.SetGameState(GameState.Pause);
            Time.timeScale = 0;
        }
        else
        {
            PauseUi.SetActive(false);
            InGameUi.SetActive(true);
            GameOption.SetGameState(GameState.Play);
            Time.timeScale = 1;
        }
    }

    public void Start()
    {
        if (GameOption.IsGameState(GameState.None))
            FadeOut();
    }
    public void Update()
    {
        if (!GameOption.IsGameState(GameState.Play))
            return;
        TimerUpdate();
    }
    public void FadeOut()
    {
        FadeImage.enabled = true;
        StartCoroutine(FadeScreen(true));
    }
    public void FadeIn()
    {
        FadeImage.enabled = true;
        StartCoroutine(FadeScreen(false));
    }

    private IEnumerator FadeScreen(bool fadeState) // true = out  / false = in
    {
        while (true)
        {
            fadeColor.a = fadeState ? 0f : 0.5f;

            FadeImage.color = Color.Lerp(this.FadeImage.color, fadeColor, this.fadeSpeed);
            if (fadeState && FadeImage.color.a <= 0.1f)
            {
                FadeImage.enabled = false;
                GameOption.StartGame();
                break;
            }
            else if (!fadeState && FadeImage.color.a >= 0.45f)
            {
                GameOption.EndGame();
                break;
            }

            yield return null;

        }
    }

    public void TimerUpdate()
    {
        timeSec += Time.deltaTime;

        if((int)timeSec == 59)
        {
            timeMin += 1;
            timeSec = 0;
        }

        minText.text = timeMin.ToString("f0");
        secText.text = timeSec.ToString("f0");
    }
}
