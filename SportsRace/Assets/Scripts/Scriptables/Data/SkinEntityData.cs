using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkinState
{
    NotBought, Bought, Equipped
}

[CreateAssetMenu(menuName ="Data/SkinEntityData")]
public class SkinEntityData : ScriptableObject
{
    public SkinState State;
    public SportType SportType;
    public int Price;
    public GameObject skinPrefab;
    public RunnerObject corePrefab;
}
