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

    private void LoadData()
    {
        data.Coins = PlayerPrefs.GetInt("Coins");
        data.Cups = PlayerPrefs.GetInt("Cups");
        data.Rank = (LeagueRank)PlayerPrefs.GetInt("Rank");
    }

    public static void SaveData()
    {
        PlayerPrefs.SetInt("Coins", Instance.data.Coins);
        PlayerPrefs.SetInt("Cups", Instance.data.Cups);
        PlayerPrefs.SetInt("Rank", (int)Instance.data.Rank);
    }
}
