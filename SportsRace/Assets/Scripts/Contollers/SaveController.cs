using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;

    [SerializeField] private UserData data;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }

    private void OnEnable()
    {
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
            SaveData();

    }

    private void LoadData()
    {
        data.Coins = PlayerPrefs.GetInt("Coins");
        data.Cups = PlayerPrefs.GetInt("Cups");
        data.Rank = (LeagueRank)PlayerPrefs.GetInt("Rank");
        data.WinsToNextRank = PlayerPrefs.GetInt("Wins");
        data.CurrentLevel = PlayerPrefs.GetInt("Level");

       // foreach (var skin in Instance.data.skins.Skins)
       // {
       //     skin.State = (SkinState)PlayerPrefs.GetInt(skin.SaveString + "_state", 0);
       // }
    }

    public static void SaveData()
    {
        PlayerPrefs.SetInt("Coins", Instance.data.Coins);
        PlayerPrefs.SetInt("Cups", Instance.data.Cups);
        PlayerPrefs.SetInt("Rank", (int)Instance.data.Rank);
        PlayerPrefs.SetInt("Wins", Instance.data.WinsToNextRank);
        PlayerPrefs.SetInt("Level", Instance.data.CurrentLevel);
       /// foreach (var skin in Instance.data.skins.Skins)
       /// {
       ///     PlayerPrefs.SetInt(skin.SaveString + "_state", (int)skin.State);
       /// }
    }
}
