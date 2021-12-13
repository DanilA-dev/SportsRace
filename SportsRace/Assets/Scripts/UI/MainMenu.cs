using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text metaCoins;
    [SerializeField] private Image blackFadeScreen;

    private void OnEnable()
    {
        blackFadeScreen.DOFade(0, 0.3f).From(1);
    }

    private void Start()
    {
        GameController.OnMenuEnter += UpdateCoins;
        metaCoins.SetText(GameController.Data.Coins.ToString());
    }

    private void OnDestroy()
    {
        GameController.OnMenuEnter -= UpdateCoins;
    }

    private void UpdateCoins()
    {
        metaCoins.SetText(GameController.Data.Coins.ToString());
    }
}
