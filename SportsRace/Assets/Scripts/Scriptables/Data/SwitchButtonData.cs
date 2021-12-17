using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SwitchButtonsData")]
public class SwitchButtonData : ScriptableObject
{
    public List<ButtonData> buttonsData = new List<ButtonData>();
}

[System.Serializable]
public class ButtonData
{
    public string Name;
    public SportType Type;
    public Sprite Sprite;
}
