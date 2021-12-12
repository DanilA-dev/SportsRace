using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text userCoins;

    private void OnEnable()
    {
        SkinObject.OnSkinBought += UpdateCoins;
        UpdateCoins(GameController.Data.Coins);
    }

    private void OnDisable()
    {
        SkinObject.OnSkinBought -= UpdateCoins;
    }

    private void UpdateCoins(int value)
    {
        userCoins.SetText(value.ToString());
    }
}
