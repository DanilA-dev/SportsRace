using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SkinsCointainerData")]
public class SkinsContainerData : ScriptableObject
{
    public List<SkinEntityData> Skins = new List<SkinEntityData>();

    public RunnerObject GetSkinRunner(SportType type)
    {
        var runnerSkin = Skins.Where(r => r.SportType == type && r.State == SkinState.Equipped)
                              .FirstOrDefault().corePrefab;

        return runnerSkin;
    }

    public List<SkinEntityData> GetDefaultSkins()
    {
        return Skins.Where(s => s.Type == SkinType.Default).ToList();
    }

    public List<SkinEntityData> GetNewSkins()
    {
        return Skins.Where(s => s.Type != SkinType.Default).ToList();
    }

    public List<SkinEntityData> GetEquippedSkins()
    {
        return Skins.Where(s => s.State == SkinState.Equipped).ToList();
    }

    public void ClearData()
    {
        for (int i = 0; i < Skins.Count; i++)
        {
            if (Skins[i].Type == SkinType.Default)
                Skins[i].State = SkinState.Equipped;
            else
                Skins[i].State = SkinState.NotBought;
        }
    }
}
