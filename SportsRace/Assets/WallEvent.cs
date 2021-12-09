using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class WallEvent : MonoBehaviour
{
    [SerializeField] private float climbTime;
    [SerializeField] private float force;
    [SerializeField] private SportType type;
    [SerializeField] private AnimationClip climbValue;
    [SerializeField] private AnimationClip finishClimbValue;
    [SerializeField] private AnimationClip bumpValue;
    [SerializeField] private Transform upPoint;
    [SerializeField] private Transform topPoint;

    private Collider _coll;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        ARunner.OnRunnerChanged += ARunner_OnRunnerChanged;
    }

    private void OnDisable()
    {
        ARunner.OnRunnerChanged -= ARunner_OnRunnerChanged;
    }

    private void ARunner_OnRunnerChanged(SportType arg1, ARunner arg2)
    {
        StartCoroutine(ReEnableTrigger());
    }

    private IEnumerator ReEnableTrigger()
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _coll.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            CheckType(r);
        }
    }


    private void CheckType(ARunner r)
    {
        var Point = new Vector3(r.transform.position.x, upPoint.position.y, r.transform.position.z);

        if(r.Type == type)
        {
            if(r.transform.position != upPoint.position)
            {
                r.State = RunnerState.Climb;
                r.transform.DOMoveY(upPoint.position.y, climbTime).OnComplete(() => OnTop(r));
            }
        }
        else
        {
            DOTween.Clear();
            r.State = RunnerState.Fall;
            r.ThrowAway(Vector3.back);
            StartCoroutine(Timer(r));
        }
    }

    private void OnTop(ARunner r)
    {
        r.State = RunnerState.ClimbTop;
        _coll.enabled = false;
    }

    private IEnumerator Timer(ARunner r)
    {
        yield return new WaitForSeconds(1);
        r.State = RunnerState.StandUp;
    }
}
