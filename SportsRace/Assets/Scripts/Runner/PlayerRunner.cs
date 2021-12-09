using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : ARunner
{
    [SerializeField] private float force;
    [SerializeField] private float downRay;

    private Vector3 moveVector;

    public static event Action<float> OnSpeedChange;

    public override RunnerState State { get => base.State; set => base.State = value; }

    protected override void Start()
    {
        base.Start();
        _canMove = true;
        InitStartType();
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

    public void SwitchRunner(SportType newType)
    {
        RunnerType = newType;
        CheckTrack(true);
    }

    protected override void ChangeRunner(SportType value)
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
        var getTracks = TracksController.Instance.LevelTracks.ToList();
        var firtsTrack = getTracks[0];
        RunnerType = firtsTrack.TrackType; 
    }


    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public override void SetFinishAnimation()
    {
        base.SetFinishAnimation();
        GameController.CurrentState = GameState.Finish;
    }

    public override void CheckTrack(bool canCheck, float time = 0)
    {
       if(canCheck)
       {
            StartCoroutine(TrackChecking(time));
       }
      
    }

    private IEnumerator TrackChecking(float time)
    {
        yield return new WaitForSeconds(time);

        // var collPos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        //
        // Collider[] c = Physics.OverlapSphere(collPos, 3, whatIsTrack);
        // foreach (var coll in c)
        // {
        //     if(coll.TryGetComponent(out TrackEntity t))
        //     {
        //         SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
        //         OnSpeedChange?.Invoke(this.defaultSpeed);
        //
        //         if (state == RunnerState.Default)
        //             _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
        //     }
        // }


        var rayPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
       RaycastHit hit;
       if (Physics.Raycast(rayPos, Vector3.down, out hit, whatIsTrack))
       {
           if (hit.collider.TryGetComponent(out TrackEntity t))
           {
               SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
               OnSpeedChange?.Invoke(this.defaultSpeed);
       
               if(state == RunnerState.Default)
               _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
           }
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
