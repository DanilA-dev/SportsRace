using UnityEngine;


[CreateAssetMenu(menuName = "Data/UserData")]
public class UserData : ScriptableObject
{
    public LeagueRank Rank;
    public int Coins;
    public int WinsToNextLeague;
}
