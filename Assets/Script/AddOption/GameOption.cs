using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    None,
    Play,
    Pause,
    Over,
}

public static class GameOption
{
    public static Func<GameState> getGameState;
    public static Action<GameState> setGameState;

    private static Action startGame;
    private static Action endGame;

    public static GameState? GetGameState()
    {
        return getGameState?.Invoke();
    }

    public static void SetGameState(GameState state)
    {
        setGameState?.Invoke(state);
    }

    public static bool IsGameState(GameState state)
    {
        return state == getGameState?.Invoke();
    }

    public static void BindStartGameFunction(Action startgame) { startGame = startgame; }
    public static void BindEndGameFunction(Action endgame) { endGame = endgame; }
    public static void StartGame()
    {
        startGame?.Invoke();
    }

    public static void EndGame()
    {
        endGame?.Invoke();
    }
    
}
