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
    private bool _subbed;
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

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent(out ARunner r))
        {
            _subbed = true;
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

                if(r.Player != null)
                    r.Player.DisableButtons(2);

                if (r.State == RunnerState.ClimbTop)
                    _coll.enabled = false;
            }


        }
        else
        {
            if (r.State == RunnerState.Default || r.State == RunnerState.Climb)
            {
                r.State = RunnerState.Fall;
                r.ParticleController.PlayByTrackEvent(TrackEventParticleType.WallHit);
                r.ThrowAway(Vector3.back * 0.7f);
            }
        }
    }

    public override void OnRunnerChanged(ARunner r)
    {
        if (!_subbed)
            return;

        r.StopAllCoroutines();
        r.State = RunnerState.Default;
        OnSwitchRunner?.Invoke();
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


    public override void Unsubscribe()
    {
        base.Unsubscribe();
        StopAllCoroutines();
        _subbed = false;
    }
}
