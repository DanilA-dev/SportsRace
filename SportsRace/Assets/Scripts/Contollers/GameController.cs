using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None, Menu, Core, Win, Lose
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private GameState _currentState;

    private event Action<GameState> OnGameStateChanged;

    #region Properties

    public static GameState CurrentState
    {
        get => Instance._currentState;
        set
        {
            Instance._currentState = value;
            Instance.OnGameStateChanged?.Invoke(value);
        }
    }

    #endregion

    private void Awake()
    {
        #region Singleton Init

        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);

        #endregion

        OnGameStateChanged += SetGameState;
    }

    private void Start()
    {
        _currentState = GameState.Menu;
    }

    public  void SetGameState(GameState newState)
    {
        switch(newState)
        {
            case GameState.None:
                Debug.LogError($"<color=red> Gamestate is {newState}</color>");
                break;

            case GameState.Menu:
                OnMenuState();
                break;

            case GameState.Core:
                OnCoreState();
                break;

            case GameState.Lose:
                OnLoseState();
                break;

            case GameState.Win:
                OnWinState();
                break;
        }
    }

    #region GameState Methods

    private void OnWinState()
    {
        Debug.Log("Enter Win State");
    }

    private void OnLoseState()
    {
        Debug.Log("Enter Lose State");
    }

    private void OnCoreState()
    {
        Debug.Log("Enter Core State");
    }

    private void OnMenuState()
    {
        Debug.Log("Enter Menu State");
    }

    #endregion
}
