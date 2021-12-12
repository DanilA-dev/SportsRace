using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SkinObject : MonoBehaviour, IPointerDownHandler
{
    [Header("Price")]
    [SerializeField] private TMP_Text priceText;
    [Header("UI Toogle Panels")]
    [SerializeField] private GameObject pricePanel;
    [SerializeField] private GameObject boughtPanel;
    [SerializeField] private GameObject equipPanel;
    [SerializeField] private GameObject modelPrefab;

    private SkinEntityData _skinData;
    private SkinsContent _skinContent;
    private UserData _userData;

    public static event Action<int> OnSkinBought;


    public void Init(SkinEntityData skinData, UserData data, SkinsContent skinContent)
    {
        _skinData = skinData;
        _userData = data;
        _skinContent = skinContent;
        CheckIfBought();
        SwapModel();
    }

    private void SwapModel()
    {
        modelPrefab.SetActive(false);
        var pos = new Vector3(modelPrefab.transform.localPosition.x, modelPrefab.transform.localPosition.y, -100);

        var m = Instantiate(_skinData.skinPrefab, this.transform);
        m.transform.localPosition = pos;
        m.transform.rotation = modelPrefab.transform.rotation;
        m.transform.localScale = modelPrefab.transform.localScale;
    }

    private void CheckIfBought()
    {
        switch (_skinData.State)
        {
            case SkinState.NotBought:
                OnNotBoughtState();
                break;

            case SkinState.Bought:
                OnBoughtState();
                break;

            case SkinState.Equipped:
                OnEquippedState();
                break;
        }

    }

    #region InitStates

    private void OnEquippedState()
    {
        equipPanel.SetActive(true);
        boughtPanel.SetActive(false);
        pricePanel.SetActive(false);
    }

    private void OnBoughtState()
    {
        equipPanel.SetActive(false);
        boughtPanel.SetActive(true);
        pricePanel.SetActive(false);
    }

    private void OnNotBoughtState()
    {
        equipPanel.SetActive(false);
        boughtPanel.SetActive(false);
        pricePanel.SetActive(true);
        priceText.text = _skinData.Price.ToString();
    }

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_skinData.State == SkinState.NotBought && _userData.Coins < _skinData.Price ||
            _skinData.State == SkinState.Equipped)
            return;

        if (_skinData.State == SkinState.Bought)
            EquipSkin();

        if (_userData.Coins >= _skinData.Price && _skinData.State != SkinState.Bought)
            BuySkin();
    }

    

    private void EquipSkin()
    {
        _skinContent.UnEquippSkinByType(_skinData.SportType);
        _skinData.State = SkinState.Equipped;
        CheckIfBought();
    }

    private void BuySkin()
    {
        _userData.Coins -= _skinData.Price;
        OnSkinBought?.Invoke(_userData.Coins);
        _skinData.State = SkinState.Bought;
        CheckIfBought();
    }
}
