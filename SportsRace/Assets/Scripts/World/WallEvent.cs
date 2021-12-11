using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class WallEvent : ATrackEvent
{
    [SerializeField] private float climbTime;
    [SerializeField] private float climbSpeed;
    [SerializeField] private SportType type;
    [SerializeField] private Transform upPoint;
    [SerializeField] private Transform topPoint;

    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;
    private bool _isRunnerUp;

    private ARunner _currentRunner;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
    }



    private IEnumerator ReEnableTrigger()
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _coll.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            _currentRunner = r;
            _currentRunner.OnRunnerChanged += OnRunnerChange;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            CheckType(r);
        }
    }


    private void CheckType(ARunner r)
    {
        var Point = new Vector3(r.transform.position.x, upPoint.position.y, r.transform.position.z);

        if (r.Type == type)
        {
            if(r.State != RunnerState.ClimbTop)
            {
                r.State = RunnerState.Climb;
                r.transform.position = Vector3.MoveTowards(r.transform.position, Point, climbSpeed * Time.deltaTime);
            }

            if (r.State == RunnerState.Climb && r.transform.position.y == Point.y)
            {
                r.State = RunnerState.ClimbTop;
                if (r.State == RunnerState.ClimbTop)
                    _coll.enabled = false;
            }


        }
        else
        {
            if (r.State == RunnerState.Default || r.State == RunnerState.Climb)
            {
                r.State = RunnerState.Fall;
                r.ThrowAway(Vector3.back);
            }
        }
    }

    private void OnRunnerChange(SportType arg1, ARunner r)
    {
        r.StopAllCoroutines();
        r.State = RunnerState.Default;
        OnSwitchRunner?.Invoke();
    }

    private IEnumerator MoveUpRoutine(ARunner r)
    {
        var Point = new Vector3(r.transform.position.x, upPoint.position.y, r.transform.position.z);

        for (float i = 0; i < climbTime; i+= Time.deltaTime)
        {
            r.transform.position = Vector3.MoveTowards(r.transform.position, Point, climbSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        OnTop(r);
    }

    public void ColliderReEnable()
    {
        StartCoroutine(ReEnableTrigger());
    }


    private void OnTop(ARunner r)
    {
        r.State = RunnerState.ClimbTop;
        _isRunnerUp = true;
        _coll.enabled = false;
    }

    private IEnumerator Timer(ARunner r)
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(1);
        _coll.enabled = true;
        r.State = RunnerState.StandUp;
    }

    public override void Unsubscribe()
    {
        if (_currentRunner != null)
        {
            _currentRunner.OnRunnerChanged -= OnRunnerChange;
        }

        StopAllCoroutines();
    }
}
