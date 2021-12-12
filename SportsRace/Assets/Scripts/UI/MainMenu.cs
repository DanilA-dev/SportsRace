using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text metaCoins;

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
