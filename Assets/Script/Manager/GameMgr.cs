using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    private static GameState gameState = GameState.None;

    private PlayerCtrl player;

    public static void SetGameState(GameState gameState)
    {
        Debug.Log(gameState);
        GameMgr.gameState = gameState;
    }
    public static GameState GetGameState()
    {
        return GameMgr.gameState;
    }

    public void Awake()
    {
        Time.timeScale = 0;
        InitGame();
    }

    public void InitGame()
    {
        GameOption.getGameState = () => { return GetGameState(); };
        GameOption.setGameState = (GameState state) => { SetGameState(state); };
        GameOption.BindStartGameFunction(StartGame);
        GameOption.BindEndGameFunction(EndGame);
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        
    }

    public static void StartGame()
    {
        Time.timeScale = 1;
        SetGameState(GameState.Play);
    }

    public static void EndGame()
    {
        SetGameState(GameState.Over);
    }
}
