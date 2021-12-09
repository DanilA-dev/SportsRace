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

    [SerializeField] private UserData data;
    [SerializeField] private RunnersSelectionController runnersController;
    [SerializeField] private List<ARunner> runners = new List<ARunner>();

    private GameState _currentState;

    private event Action<GameState> OnGameStateChanged;

    public static event Action OnMenuEnter;
    public static event Action OnCoreEnter;


    #region Properties
    public static UserData Data { get => Instance.data; set => Instance.data = value; }

    public static GameState CurrentState
    {
        get => Instance._currentState;
        set
        {
            Instance._currentState = value;
            Instance.OnGameStateChanged?.Invoke(value);
        }
    }

    public List<ARunner> Runners => runners;

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
        CurrentState = GameState.Menu;
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
        UIController.TurnOnPanel(UIPanelType.Win);
    }

    private void OnLoseState()
    {
        UIController.TurnOnPanel(UIPanelType.Lose);
    }

    private void OnCoreState()
    {
        UIController.TurnOnPanel(UIPanelType.Core);
        OnCoreEnter?.Invoke();
    }

    private void OnMenuState()
    {
        foreach (var r in runners)
            r.OnReset();

        runnersController.ClearCreatedRunners();
        runnersController.SetCreatedRunners();
        OnMenuEnter?.Invoke();
        UIController.TurnOnPanel(UIPanelType.Menu);
    }

    #endregion
}
