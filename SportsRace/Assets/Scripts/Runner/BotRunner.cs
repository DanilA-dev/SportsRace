using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotRunner : ARunner
{
    [SerializeField,Range(0,10)] private float switchTime;


    private Vector3 _moveVector;
    private Collider _botCollider;


    public override RunnerState State { get => base.State; set => base.State = value; }
    public override Collider RunnerCollider { get => _botCollider; set => _botCollider = value; }


    protected override void Start()
    {
        base.Start();
        _botCollider = GetComponent<Collider>();
        _canMove = true;
    }


    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameState.Core || GameController.CurrentState == GameState.Finish)
        {
            Move(_moveVector, defaultSpeed);
            ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        var velocityY = -gravity * Time.deltaTime;
        _moveVector = new Vector3(0, velocityY, 1);
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

       var rayPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
       RaycastHit hit;
       if (Physics.Raycast(rayPos, Vector3.down, out hit, whatIsTrack))
       {
           if (hit.collider.TryGetComponent(out TrackEntity t))
           {
                SetSpeed(_currentRunner.RunnerData.GetTrackSpeed(t.TrackType));

                if (_runnerAnimator != null && state == RunnerState.Default)
                    _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));

                if (runnerType != t.TrackType && t.TrackType != SportType.Start)
                {
                    Punish(t);
                    particleController.StopRunnerSpecialParticles();
                }
           }
       }
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
                base.ChangeRunner(value);
                CurrentRunner = _avaliableRunners[i];
                RunnerAnimator = CurrentRunner.GetComponent<Animator>();
                CurrentRunner.gameObject.SetActive(true);
            }

            else
                _avaliableRunners[i].gameObject.SetActive(false);
        }
    }

    public override void SetFinishAnimation()
    {
        if (_runnerAnimator == null)
            return;

        if (_finishIndex > 1)
            _runnerAnimator.Play("Defeated");
        else if (_finishIndex == 1)
            _runnerAnimator.Play("Victory");
    }

    public override void CheckPosition()
    {
        if (_finishIndex > 1)
            _runnerAnimator.Play("Defeated");
        else if (_finishIndex == 1)
            _runnerAnimator.Play("Victory");
    }


    public override void OnMenu()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        base.OnMenu();
    }

    private void Punish(TrackEntity t)
    {
        StartCoroutine(SwitchRunner(t));
    }

    private IEnumerator SwitchRunner(TrackEntity t)
    {
        yield return new WaitForSeconds(switchTime);

        if(Type != t.TrackType)
        {
            RunnerType = t.TrackType;
            particleController.PlayRunnerSpecial(t.TrackType);
        }

        CheckTrack(true);
    }

    public override void PlayTrackEventParticle(TrackEventParticleType type)
    {
        ParticleController.PlayByTrackEvent(type);
    }

    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public override void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        body.velocity = dir * speed * Time.deltaTime;
    }
}
