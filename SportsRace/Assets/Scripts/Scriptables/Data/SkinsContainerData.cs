using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SkinsCointainerData")]
public class SkinsContainerData : ScriptableObject
{
    public List<SkinEntityData> Skins = new List<SkinEntityData>();
}
