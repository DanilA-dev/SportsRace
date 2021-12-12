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
}
