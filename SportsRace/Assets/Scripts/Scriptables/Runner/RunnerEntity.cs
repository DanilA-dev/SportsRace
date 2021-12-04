using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/New Runner")]
public class RunnerEntity : ScriptableObject
{
    public string SameTypeAnimValue;
    public string WrongTypeAnimValue;

    public List<RunnerSpeeds> Speeds = new List<RunnerSpeeds>();
}

[System.Serializable]
public class RunnerSpeeds
{
    [SerializeField] private SportType type;
    [SerializeField,Range(1,1000)] private float speed;

    public SportType Type => type;
    public float Speed => speed;
}