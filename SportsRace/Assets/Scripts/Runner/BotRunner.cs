using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotRunner : ARunner
{
    [SerializeField,Range(0,10)] private float switchTime;


    private Vector3 moveVector;

    public override RunnerState State { get => base.State; set => base.State = value; }

    protected override void Start()
    {
        base.Start();
        _canMove = true;
        GameController.OnCoreEnter += GameController_OnCoreEnter;

    }

    private void GameController_OnCoreEnter()
    {
        CheckTrack(true);
    }

    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameState.Core)
        {
            Move(moveVector, defaultSpeed);
            ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        var velocityY = -gravity * Time.deltaTime;
        moveVector = new Vector3(0, velocityY, 1);
    }

    public override void CheckTrack(bool canCheck, float time = 0)
    {
        if (canCheck)
        {
            StartCoroutine(TrackChecking(time));
        }
    }

    private IEnumerator TrackChecking(float time)
    {
        yield return new WaitForSeconds(time);

      // Collider[] c = Physics.OverlapSphere(transform.position, 2, whatIsTrack);
      // foreach (var coll in c)
      // {
      //     if (coll.TryGetComponent(out TrackEntity t))
      //     {
      //         SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
      //
      //         if (state == RunnerState.Default)
      //             _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
      //
      //         if (runnerType != t.TrackType)
      //             Punish(t);
      //     }
      // }
       var rayPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
       RaycastHit hit;
       if (Physics.Raycast(rayPos, Vector3.down, out hit, whatIsTrack))
       {
           if (hit.collider.TryGetComponent(out TrackEntity t))
           {
               SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
       
               if (state == RunnerState.Default)
                   _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
       
               if (runnerType != t.TrackType)
                   Punish(t);
           }
       }
    }

    protected override void ChangeRunner(SportType value)
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


    private void Punish(TrackEntity t)
    {
        StartCoroutine(SwitchRunner(t));
    }

    private IEnumerator SwitchRunner(TrackEntity t)
    {
        yield return new WaitForSeconds(switchTime);
        RunnerType = t.TrackType;
        CheckTrack(true);
    }

    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }


    public override void FinishStop()
    {
        _canMove = false;
    }

    public override void SetFinishAnimation()
    {
        base.SetFinishAnimation();
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
