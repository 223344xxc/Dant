﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICtrl : MonoBehaviour
{
    enum UiState
    {
        None,
        Pause,
        InGame,
        EndGame
    }

    [SerializeField] private Image fadeImage;

    [SerializeField] private Sprite WinBackGround;
    [SerializeField] private Sprite LoseBackGround;

    private Dictionary<UiState, GameObject> UiList;
    private Text PlayerHpText;
    private Image EndGameBackGround;

    [SerializeField] private Text minText;
    [SerializeField] private Text secText;

    [SerializeField] private static int timeMin;
    [SerializeField] private static float timeSec;

    [SerializeField] private Color fadeColor;
    public float fadeSpeed;

    public void Awake()
    {
        InitUi();
        ShowUi(UiState.InGame);
        PlayerCtrl.AddGameOverFun(FadeIn);
    }

    private void InitUi()
    {
        UiList = new Dictionary<UiState, GameObject>();
        AddUiDictionary(UiState.InGame, "InGame");
        AddUiDictionary(UiState.Pause, "Pause");
        AddUiDictionary(UiState.EndGame, "EndGame");
        PlayerHpText = UiList[UiState.EndGame].transform.Find("EndGamePanel").transform.Find("Hart").transform.Find("HartCount").GetComponent<Text>();
        EndGameBackGround = UiList[UiState.EndGame].transform.Find("EndGamePanel").transform.Find("BackGround").GetComponent<Image>();

    }

    private void AddUiDictionary(UiState uiState, string uiName)
    {

        UiList.Add(uiState, GameObject.Find(uiName));
    }

    public void PauseGame()
    {
        
        if (Time.timeScale != 0)
        {
            ShowUi(UiState.Pause);
            GameOption.SetGameState(GameState.Pause);
            Time.timeScale = 0;
        }
        else
        {
            ShowUi(UiState.InGame);
            GameOption.SetGameState(GameState.Play);
            Time.timeScale = 1;
        }
    }

    private void ShowUi(UiState uiState)
    {
        for (UiState i = UiState.Pause; (int)i < (int)UiState.EndGame + 1; i++)
        {
            UiList[i].SetActive(false);
        }

        switch (uiState)
        {
            case UiState.EndGame:
                EndGameBackGround.sprite = PlayerCtrl.Instance.Hp > 0 ? WinBackGround : LoseBackGround;
                PlayerHpText.text = "X " + PlayerCtrl.Instance.Hp;
                break;
            default:
                break;
        }

        UiList[uiState].SetActive(true);
    }

    private void ShowEndGameUi()
    {
        ShowUi(UiState.EndGame);
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
        fadeImage.enabled = true;
        StartCoroutine(FadeScreen(true));
    }
    public void FadeIn()
    {
        fadeImage.enabled = true;
        StartCoroutine(FadeScreen(false));
    }

    private IEnumerator FadeScreen(bool fadeState) // true = out  / false = in
    {
        while (true)
        {
            fadeColor.a = fadeState ? 0f : 0.5f;

            fadeImage.color = Color.Lerp(this.fadeImage.color, fadeColor, this.fadeSpeed);
            if (fadeState && fadeImage.color.a <= 0.1f)
            {
                fadeImage.enabled = false;
                GameOption.StartGame();
                break;
            }
            else if (!fadeState && fadeImage.color.a >= 0.45f)
            {
                GameOption.EndGame();
                ShowEndGameUi();
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
