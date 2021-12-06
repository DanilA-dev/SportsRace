using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : ARunner
{
    public  float gravity;
    private bool _canMove;
    public float force;

    public static event Action<float> OnSpeedChange;
   
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

    public void SwitchRunner(SportType newType)
    {
        RunnerType = newType;
        CheckTrack();
    }

    private void ChangeRunner(SportType value)
    {
        if(_avaliableRunners.Count < 0)
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
        var getTracks = TracksController.Instance.GeneratedTracks.ToList();
        var firtsTrack = getTracks[0];
        RunnerType = firtsTrack.TrackType; 
    }


    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public void SetSpeed(float speed)
    {
        this.defaultSpeed = speed;
    }


    public override void CheckTrack()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 1, whatIsTrack);
        if(col[0].TryGetComponent(out TrackEntity t))
        {
            SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
            _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
            OnSpeedChange?.Invoke(this.defaultSpeed);
        }
    }


    public override void OnReset()
    {
        _canMove = true;
        _isFinished = false;
        _finishIndex = 0;
        body.useGravity = true;
        body.isKinematic = false;
        SetSpeed(defaultSpeed);
    }

    public override void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        body.velocity = dir * speed * Time.deltaTime;
    }

    public override void FinishStop()
    {
        _canMove = false;
    }
}
