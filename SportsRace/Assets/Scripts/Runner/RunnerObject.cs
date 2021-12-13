using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerObject : MonoBehaviour
{
    [SerializeField] private SportType type;
    [SerializeField] private RunnerEntity runnerData;
    [SerializeField] private Vector3 groundPositionOffset;

    public SportType Type => type;
    public RunnerEntity RunnerData => runnerData;
    public Vector3 GroundPositionOffset => groundPositionOffset;

}
