using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerObject : MonoBehaviour
{
    [SerializeField] private SportType type;
    [SerializeField] private RunnerEntity runnerData;

    public SportType Type => type;
    public RunnerEntity RunnerData => runnerData;
}
