using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text metaCoins;
    [SerializeField] private float fadeTime;
    [SerializeField] private float timeToPlayButtonAppear;
    [SerializeField] private Image blackFadeScreen;
    [SerializeField] private Button playButton;

    private void OnEnable()
    {
        playButton.enabled = false;

        blackFadeScreen.DOFade(0, fadeTime).From(1);
        playButton.transform.DOScale(1, timeToPlayButtonAppear).From(0).OnComplete(() => playButton.enabled = true);
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
