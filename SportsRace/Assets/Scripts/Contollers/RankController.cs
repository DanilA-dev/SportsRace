using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankController : MonoBehaviour
{
    public static RankController Instance;

    [SerializeField] private UserData data;
    [SerializeField] private RankData rankData;
    [Header("UI")]
    [SerializeField] private Image currentRankImage;
    [SerializeField] private Image nextRankImage;
    [SerializeField] private Sprite winGameSkinBar;
    [SerializeField] private Sprite defaultGameSkinBar;

    [SerializeField] private List<Image> gameBarsImg = new List<Image>();

    private Rank _nextRank;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }

    public void CheckRank()
    {
        if (data.Rank == LeagueRank.Platinum)
            return;

        SetPlayerRank();
        SetRankIcons();
    }

    private void SetPlayerRank()
    {
        _nextRank = rankData.Ranks.Where(r => r.CurrentRank == data.Rank + 1).FirstOrDefault();
        if (_nextRank == null)
            return;

        if (data.WinsToNextRank >= _nextRank.WinsToOpen)
        {
            data.Rank = _nextRank.CurrentRank;
            data.WinsToNextRank = 0;
        }
    }

    private void SetRankIcons()
    {
        var nextRank = rankData.Ranks.Where(r => r.CurrentRank == data.Rank + 1).FirstOrDefault();

        for (int i = 0; i < rankData.Ranks.Count; i++)
        {
            if (data.Rank == rankData.Ranks[i].CurrentRank)
            {
                currentRankImage.sprite = rankData.Ranks[i].Icon;

                if(_nextRank != null)
                    nextRankImage.sprite = rankData.Ranks.Where(r => r.CurrentRank == data.Rank + 1)
                                                     .FirstOrDefault().Icon;
            }
        }

        for (int i = 0; i < gameBarsImg.Count; i++)
        {
            if (data.WinsToNextRank == 0)
                gameBarsImg[i].sprite = defaultGameSkinBar;

            for (int j = 0; j < data.WinsToNextRank; j++)
                gameBarsImg[j].sprite = winGameSkinBar;
        }
    }
}
