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

    private float _startClimbSpeed;

    public float ClimbSpeed { get => climbSpeed; set => climbSpeed = value; }

    private void Awake()
    {
        _coll = GetComponent<Collider>();
        _startClimbSpeed = climbSpeed;

        GameController.OnRestartLevel += OnRestart;

    }

    private void OnRestart()
    {
        Init();
    }

    public override void Init()
    {
        climbSpeed = _startClimbSpeed;
        _coll.enabled = true;
        StopAllCoroutines();

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
            CheckType(r);
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
        var Top = new Vector3(r.transform.position.x, topPoint.position.y, topPoint.position.z);


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
               

                if (r.Player != null)
                    r.Player.DisableButtons(2);

                if (r.State == RunnerState.ClimbTop)
                    _coll.enabled = false;
            }

            if(r.State == RunnerState.ClimbTop && r.transform.position.z != Top.z)
                r.transform.position = Vector3.MoveTowards(r.transform.position, Top, 10 * Time.deltaTime);


        }
        else
        {
            if (r.State == RunnerState.Default || r.State == RunnerState.Climb)
            {
                r.State = RunnerState.Fall;
                r.ParticleController.PlayByTrackEvent(TrackEventParticleType.WallHit);

                var jumpDir = new Vector3(r.Body.velocity.x, r.transform.position.y 
                                        + r.TrackVariables.KnockOutYOffset, r.Body.velocity.z
                                        + r.TrackVariables.KnockOutZOffset);

                r.Jump(-jumpDir, r.TrackVariables.KnockOutForce);
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

    private void OnDestroy()
    {
        GameController.OnRestartLevel -= OnRestart;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        StopAllCoroutines();
        _subbed = false;
    }
}
