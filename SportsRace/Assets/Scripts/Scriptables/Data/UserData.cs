using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Data/UserData")]
public class UserData : ScriptableObject
{
    public SkinsContainerData skins;
    public LeagueRank Rank;
    public int Coins;
    public int Cups;
    public int WinsToNextRank;
    public int CurrentLevel;

    [ContextMenu("Clear Data")]
    private void ClearData()
    {
        skins.ClearData();
        Rank = LeagueRank.Bronze;
        Coins = 150;
        Cups = 0;
        WinsToNextRank = 0;
        CurrentLevel = 0;
        PlayerPrefs.DeleteAll();
    }
}
