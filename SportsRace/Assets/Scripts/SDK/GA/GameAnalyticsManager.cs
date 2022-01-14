using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour
{
    public static GameAnalyticsManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        GameAnalytics.Initialize();
    }


    public void OnLevelStart(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + level);
        Debug.Log("LevelStarted " + level);
    }

    public void OnLevelComplete(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + level);
        Debug.Log("LevelPassed " + level);
    }

    public void OnLevelFail(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + level);
        Debug.Log("LevelFail " + level);
    }


}
