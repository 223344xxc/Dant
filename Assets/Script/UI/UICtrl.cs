using System.Collections;
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

    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject LoseText;

    private Dictionary<UiState, GameObject> UiList;
    private Text PlayerHpText;
    private Image EndGameBackGround;
    private MainCtrl main;


    [SerializeField] private Text minText;
    [SerializeField] private Text secText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text maxCompoText;
    [SerializeField] private Text AttacksText;

    [SerializeField] private static float timeSec;

    [SerializeField] private Color fadeColor;
    [SerializeField] private Image[] flowerImage;
    [SerializeField] private Sprite activeFlowerImage;
    [SerializeField] private Sprite unActiveFlowerImage;

    public float fadeSpeed;

    private bool StartedGame = false;
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
        main = GameObject.Find("Main").GetComponent<MainCtrl>();
        Time.timeScale = 0;
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
                if (PlayerCtrl.Instance.Hp > 0)
                {
                    WinText.SetActive(true);
                    PlayerPrefs.SetFloat("Stage1_ClearTime", timeSec);
                    PlayerPrefs.SetInt("Stage1_FlowerCount", PlayerCtrl.Instance.flowerCount);
                }
                else
                    LoseText.SetActive(true);

                timeText.text = "Time : " + minText.text + ":" + secText.text;
                maxCompoText.text = "Max Combo : " + PlayerCtrl.Instance.MaxCombo; 
                AttacksText.text = "Attacks : " + PlayerCtrl.Instance.KillCount;

                PlayerHpText.text = "X " + PlayerCtrl.Instance.Hp;
                for(int i = 0; i < PlayerCtrl.Instance.flowerCount; i++)
                {
                    flowerImage[i].sprite = activeFlowerImage;
                }

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
        //if (GameOption.IsGameState(GameState.None))
        //    FadeOut();
    }
    public void Update()
    {
        GameOption.GetGameState().LogError();
        MainCtrl.nowSceneLoauded.LogError();
        if (!StartedGame && MainCtrl.nowSceneLoauded)
            FadeOut();

        if (!GameOption.IsGameState(GameState.Play))
            return;
        TimerUpdate();
    }
    
    public void outGame()
    {
        GameOption.SetGameState(GameState.None);
        timeSec = 0;
    }

    public void SceneLoad(string sceneName)
    {
        outGame();
        if (main)
            if (sceneName == "")
                main.LoadScene(MainCtrl.NowScene);
            else
                main.LoadScene(sceneName);

    }

    public void FadeOut()
    {
        StartedGame = true;
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

        minText.text = (timeSec / 60).ToString("f0");
        secText.text = (timeSec % 60).ToString("f0");
    }
}
