using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Data/New Runner")]
public class RunnerEntity : ScriptableObject
{

    public List<RunnerSpeeds> Speeds = new List<RunnerSpeeds>();

    public float GetTrackSpeed(SportType type)
    {
        return Speeds.Where(t => t.Type == type).FirstOrDefault().Speed;
    }

    public string GetAnimationValue(SportType type)
    {
        return Speeds.Where(t => t.Type == type).FirstOrDefault().AnimValue.name;
    }

}

[System.Serializable]
public class RunnerSpeeds
{
    [SerializeField] private SportType type;
    [SerializeField,Range(1,1000)] private float speed;
    [SerializeField] private AnimationClip animValue;

    public SportType Type => type;
    public float Speed => speed;
    public AnimationClip AnimValue => animValue;
}