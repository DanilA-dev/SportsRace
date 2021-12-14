using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : ARunner, IPlayer
{

    private Vector3 moveVector;

    public static event Action<float> OnSpeedChange;

    public override IPlayer Player { get => this; }
    public override RunnerState State { get => base.State; set => base.State = value; }

    protected override void Start()
    {
        base.Start();
        _canMove = true;
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
        base.ChangeRunner(value);

        if (_avaliableRunners.Count < 0)
        {
            Debug.LogError("No Avaliable runners!!!");
            return;
        }

        for (int i = 0; i < _avaliableRunners.Count; i++)
        {
            if (_avaliableRunners[i].Type == value)
            {
                CurrentRunner = _avaliableRunners[i];
                CurrentRunner.gameObject.SetActive(true);
                RunnerAnimator = CurrentRunner.GetComponent<Animator>();
            }
                
            else
                _avaliableRunners[i].gameObject.SetActive(false);
        }
    }

    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public override void SetFinishAnimation()
    {
         if (_runnerAnimator == null)
             return;
        
         if (_finishIndex > 1)
             _runnerAnimator.Play("Defeated");
         else if (_finishIndex == 1)
             _runnerAnimator.Play("Victory");
        
        GameController.CurrentState = GameState.Finish;
    }

    public override void CheckPosition()
    {
        GameController.Data.Coins += GameController.SessionScore;
        SaveController.SaveData();

        if (_finishIndex == 1)
        {
            GameController.Data.WinsToNextRank++;
            GameController.CurrentState = GameState.Win;
        }
        else
            GameController.CurrentState = GameState.Lose;
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
       var rayPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
       RaycastHit hit;
       if (Physics.Raycast(rayPos, Vector3.down, out hit, whatIsTrack))
       {
           if (hit.collider.TryGetComponent(out TrackEntity t))
           {
                SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));
                OnSpeedChange?.Invoke(this.defaultSpeed);

                if (_runnerAnimator != null && state == RunnerState.Default)
                    _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
           }
       }
    }

    public override void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        body.velocity = dir * speed * Time.deltaTime;
    }

    public void DisableButtons(float time)
    {
        GameController.SwitchButtonsController.DisableSwitchButtons(time);
    }
}
