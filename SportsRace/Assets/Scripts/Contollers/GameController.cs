using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    None, Menu, Core, Win, Lose, Finish, Pause, UnPause
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private UserData data;
    [SerializeField] private RunnersSelectionController runnersController;
    [SerializeField] private CupsController cupsController;
    [SerializeField] private SwitchButtonInitializer switchButtonsController;
    [SerializeField] private List<ARunner> runners = new List<ARunner>();

    private int _sessionScore;
    private GameState _currentState;

    private int _gameFirstEnter;


    public static event Action OnMenuEnter;
    public static event Action OnCoreEnter;
    public static event Action<int> OnSessionScoreChange;
    private event Action<GameState> OnGameStateChanged;


    #region Properties
    public static CupsController CupController => Instance.cupsController;
    public static SwitchButtonInitializer SwitchButtonsController => Instance.switchButtonsController;
    public static int GameFirstEnter { get => Instance._gameFirstEnter; set => Instance._gameFirstEnter = value; }
    public static UserData Data { get => Instance.data; set => Instance.data = value; }
    public static int SessionScore { get => Instance._sessionScore; set => Instance._sessionScore = value; }

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
        _gameFirstEnter = PlayerPrefs.GetInt("FirstStart");
    }

    private void OnApplicationQuit()
    {
        if (!PlayerPrefs.HasKey("FirstStart"))
        {
            _gameFirstEnter = 1;
            PlayerPrefs.SetInt("FirstStart", _gameFirstEnter);
        }
    }


    public  void SetGameState(GameState newState)
    {
        switch(newState)
        {
            case GameState.None:
                Debug.LogError($"<color=red> Gamestate is {newState}</color>");
                break;

            case GameState.Menu:
                StartCoroutine(OnMenuState());
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

            case GameState.Pause:
                OnPauseState();
                break;

            case GameState.UnPause:
                OnUnPauseState();
                break;
            
        }
    }


    #region GameState Methods
    private void OnUnPauseState()
    {
        Time.timeScale = 1;
        UIController.TurnOffPanel(UIPanelType.Pause);
    }


    private void OnPauseState()
    {
        foreach (var r in runners)
            r.StopAllCoroutines();

        Time.timeScale = 0;
        TracksController.Instance.UnSubFromTracks();
        UIController.TurnOnPanel(UIPanelType.Pause);
    }

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

    private IEnumerator OnMenuState()
    {
        Time.timeScale = 1;
        _sessionScore = 0;
        
        //yield return StartCoroutine(runnersController.ClearCreatedRunners());
        yield return StartCoroutine(TracksController.Instance.CreateTrack());

        RankController.Instance.CheckRank();
        runnersController.SetCreatedRunners(); 
        OnMenuEnter?.Invoke();
        UIController.TurnOnPanel(UIPanelType.Menu);
        SaveController.SaveData();
        switchButtonsController.SetSwtichButtons();

        foreach (var r in runners)
        {
            r.StopAllCoroutines();
            r.SetAvaliableRunnerList();
            r.OnMenu();
        }
        TracksController.Instance.CheckFullTrack();
    }

    #endregion


    public static void GetSessionScore(int value)
    {
        Instance._sessionScore += value;
        OnSessionScoreChange?.Invoke(Instance._sessionScore);
    }
    
}
