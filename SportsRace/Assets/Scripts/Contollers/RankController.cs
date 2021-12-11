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


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);
    }

    public void CheckRank()
    {
        SetPlayerRank();
        SetRankIcons();
    }

    private void SetPlayerRank()
    {
        var nextRank = rankData.Ranks.Where(r => r.CurrentRank == data.Rank + 1).FirstOrDefault();
        if (data.WinsToNextRank >= nextRank.WinsToOpen)
        {
            data.Rank = nextRank.CurrentRank;
            data.WinsToNextRank = 0;
        }
    }

    private void SetRankIcons()
    {
        for (int i = 0; i < rankData.Ranks.Count; i++)
        {
            if (data.Rank == rankData.Ranks[i].CurrentRank)
            {
                currentRankImage.sprite = rankData.Ranks[i].Icon;
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
