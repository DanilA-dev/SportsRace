using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerRunner : ARunner, IPlayer
{
    [SerializeField] private List<TapEventVariables> tapVariables = new List<TapEventVariables>();

    [Header("Camera Setup")]
    [SerializeField] private Transform finishRotateCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float menuCameraAngle;
    [SerializeField] private float coreCameraAngle;


    private Vector3 _moveVector;
    private Collider _playerCollider;

    private bool _canCheckTrack = true;

    public static event Action<float> OnSpeedChange;
    public event Action<PlayerRunner> OnTapEvent;
    public event Action OnTrackEventEnter;
    public event Action OnTrackEventExit;


    public List<TapEventVariables> TapVariables => tapVariables;
    public override IPlayer Player { get => this; }
    public override RunnerState State { get => base.State; set => base.State = value; }

    public override Collider RunnerCollider { get => _playerCollider; set => _playerCollider = value; }


    protected override void Start()
    {
        base.Start();
        _playerCollider = GetComponent<Collider>();
        _canMove = true;

    }

    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameState.Core)
        {
            Move(_moveVector, defaultSpeed);
            ApplyGravity();
        }
    }

    public void SetCameraMenu()
    {
        virtualCamera.PreviousStateIsValid = false;
        var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        composer.m_DeadZoneHeight = menuCameraAngle;
    }

    public void SetCameraCore()
    {
        var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        composer.m_DeadZoneHeight = coreCameraAngle;
    }

    private void ApplyGravity()
    {
        var velocityY = -gravity * Time.deltaTime;
        _moveVector = new Vector3(0, velocityY, 1);
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
            return;

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
        if (!_canCheckTrack)
            return;

       if(canCheck)
            StartCoroutine(TrackChecking(time));
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

                PlaySpecialParticle(t);

                if (_runnerAnimator != null && state == RunnerState.Default)
                    _runnerAnimator.Play(_currentRunner.RunnerData.GetAnimationValue(t.TrackType));
           }
        }
    }

    private void PlaySpecialParticle(TrackEntity t)
    {
        if (runnerType == t.TrackType)
            particleController.PlayRunnerSpecial(t.TrackType);
        else
            particleController.StopRunnerSpecialParticles();
    }

    public override void PlayTrackEventParticle(TrackEventParticleType type)
    {
        ParticleController.PlayByTrackEvent(type);
    }

    public override void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        body.velocity = dir * speed * Time.deltaTime;
    }

    public void TurnOnFinishCamera()
    {
        finishRotateCamera.gameObject.SetActive(true);
    }

    public void TurnOffFinishCamera()
    {
        finishRotateCamera.localRotation = Quaternion.Euler(Vector3.zero);
        finishRotateCamera.gameObject.SetActive(false);
    }

    public override void OnMenu()
    {
        base.OnMenu();
        TurnOffFinishCamera();
        SetCameraMenu();
    }

    public override void OnStart()
    {
        base.OnStart();
        SetCameraCore();
    }

    public void DisableButtons(float time)
    {
        GameController.SwitchButtonsController.DisableSwitchButtons(time);
    }

    public void ActivateTapIvent()
    {
        OnTapEvent?.Invoke(this);
    }

    public void SetCheckTracker(bool on)
    {
        _canCheckTrack = on;
    }

    public void OnTappableTrackEnter()
    {
        OnTrackEventEnter?.Invoke();
    }

    public void OnTappableTrackExit()
    {
        OnTrackEventExit?.Invoke();
    }
}

[Serializable]
public class TapEventVariables
{
    public SportType Type;
    public float SpeedPerTap;
    public float MaxTapSpeed;
}
