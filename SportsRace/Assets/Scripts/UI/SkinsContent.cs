using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsContent : MonoBehaviour
{
    [SerializeField] private SkinsContainerData skinContainer;
    [SerializeField] private UserData userData;
    [SerializeField] private SkinObject skinPrefab;

    private List<SkinObject> createdSkins = new List<SkinObject>();

    private void OnEnable()
    {
        CreateSkins();
    }

    private void OnDisable()
    {
        Debug.Log("Clear?");
        ClearSkins();
    }

    private void ClearSkins()
    {
        for (int i = 0; i < createdSkins.Count; i++)
            Destroy(createdSkins[i].gameObject);

        createdSkins.Clear();
    }

    private void CreateSkins()
    {
        for (int i = 0; i < skinContainer.Skins.Count; i++)
        {
            var createdSkin = Instantiate(skinPrefab, this.transform);
            createdSkin.Init(skinContainer.Skins[i], userData, this);
            createdSkins.Add(createdSkin);
        }
    }

    public void UnEquippSkinByType(SportType type)
    {
        var newList = skinContainer.Skins.Where(s => s.SportType == type).ToList();

        foreach (var s in newList)
            s.State = SkinState.Bought;
    }
}
