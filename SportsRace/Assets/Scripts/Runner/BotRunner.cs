using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotRunner : ARunner
{
    [SerializeField,Range(0,10)] private float switchTime;

    private bool _canMove;

    public SportType RunnerType
    {
        get => runnerType;
        set
        {
            runnerType = value;
            ChangeRunner(value);
        }
    }


    private void Start()
    {
        _canMove = true;
        InitStartType();
        GameController.OnCoreEnter += GameController_OnCoreEnter;

    }

    private void GameController_OnCoreEnter()
    {
        CheckTrack();
    }

    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameState.Core)
            Move(transform.forward, defaultSpeed);
    }

    public override void CheckTrack()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 3, whatIsTrack);
        if (col[0].TryGetComponent(out TrackEntity t))
        {
            SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
            _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));

            if (runnerType != t.TrackType)
                Punish(t);
        }
    }

    private void ChangeRunner(SportType value)
    {
        if (_avaliableRunners.Count < 0)
        {
            Debug.LogError("No Avaliable runners!!!");
            return;
        }

        for (int i = 0; i < _avaliableRunners.Count; i++)
        {
            if (_avaliableRunners[i].Type == value)
            {
                _avaliableRunners[i].gameObject.SetActive(true);
                _currentRunner = _avaliableRunners[i];
                _runnerAnimator = _avaliableRunners[i].GetComponent<Animator>();
            }

            else
                _avaliableRunners[i].gameObject.SetActive(false);
        }
    }
    private void InitStartType()
    {
        var getTracks = TracksController.Instance.LevelTracks.ToList();
        var firtsTrack = getTracks[0];
        RunnerType = firtsTrack.TrackType;
    }

    private void Punish(TrackEntity t)
    {
        StartCoroutine(SwitchRunner(t));
    }

    private IEnumerator SwitchRunner(TrackEntity t)
    {
        yield return new WaitForSeconds(switchTime);
        RunnerType = t.TrackType;
        CheckTrack();
    }

    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public void SetSpeed(float speed)
    {
        defaultSpeed = speed;
    }

    public override void FinishStop()
    {
        _canMove = false;
    }

    public override void OnReset()
    {
        _canMove = true;
        _isFinished = false;
        _finishIndex = 0;
        SetSpeed(defaultSpeed);
        body.useGravity = true;
        body.isKinematic = false;
    }

    public override void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        body.velocity = dir * speed * Time.deltaTime;
    }
}
