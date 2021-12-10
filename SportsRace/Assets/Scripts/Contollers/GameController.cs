using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None, Menu, Core, Win, Lose, Finish
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private UserData data;
    [SerializeField] private RunnersSelectionController runnersController;
    [SerializeField] private SwitchButtonInitializer switchButtonsController;
    [SerializeField] private List<ARunner> runners = new List<ARunner>();

    private int _sessionScore;
    private GameState _currentState;

    public static event Action OnMenuEnter;
    public static event Action OnCoreEnter;
    public static event Action<int> OnSessionScoreChange;
    private event Action<GameState> OnGameStateChanged;


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

    private void OnEnable()
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

            case GameState.Finish:
                OnFinishState();
                break;

        }
    }

    #region GameState Methods

    private void OnFinishState()
    {
        UIController.TurnOffPanel(UIPanelType.Core);
    }

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

        foreach (var r in runners)
            r.OnStart();
    }

    private async void OnMenuState()
    {
       await TracksController.Instance.CreateTrack();

        runnersController.ClearCreatedRunners();
        runnersController.SetCreatedRunners(); // сделать рестарт не через выход в меню а не рестарт сцены(щас ошибка)
        OnMenuEnter?.Invoke();
        UIController.TurnOnPanel(UIPanelType.Menu);
        SaveController.SaveData();
        switchButtonsController.SetSwtichButtons();

        foreach (var r in runners)
        {
            r.SetAvaliableRunnerList();
            r.OnMenu();
        }    
    }

    #endregion


    public static void GetSessionScore(int value)
    {
        Instance._sessionScore += value;
        OnSessionScoreChange?.Invoke(Instance._sessionScore);
    }
    
}
