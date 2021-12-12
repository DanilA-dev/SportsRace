using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text metaCoins;

    private void Start()
    {
        metaCoins.SetText(GameController.Data.Coins.ToString());
    }
}
