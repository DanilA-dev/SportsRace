using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkinState
{
    NotBought, Bought, Equipped
}

public enum SkinType
{
    Default, New
}

[CreateAssetMenu(menuName ="Data/SkinEntityData")]
public class SkinEntityData : ScriptableObject
{
    public string SaveString;
    public SkinState State;
    public SkinType Type;
    public SportType SportType;
    public int Price;
    public GameObject skinPrefab;
    public RunnerObject corePrefab;
}
