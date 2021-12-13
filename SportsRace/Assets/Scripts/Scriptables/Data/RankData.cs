using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeagueRank
{
    Bronze, Silver, Golden, Platinum
}

[CreateAssetMenu(menuName ="Data/RankData")]
public class RankData : ScriptableObject
{
    public List<Rank> Ranks = new List<Rank>();
}

[System.Serializable]
public class Rank
{
    public LeagueRank CurrentRank;
    public Sprite Icon;
    public int WinsToOpen;
}