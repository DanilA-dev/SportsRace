using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public enum RunnerState
{
    Default, Climb, ClimbTop, Fall, StandUp, Swim,
    JumpObstacle, JumpSand, HitWeak, HitHard, Stunned, Finish,
    Land
}

public abstract class ARunner : MonoBehaviour
{

    [SerializeField]private Transform start;
    [SerializeField] private GameObject rotateCamera;
    [Header("Body and speed")]
    [SerializeField] protected float gravity;
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected Rigidbody body;
    [Header("CurrentType")]
    [SerializeField] protected SportType runnerType;
    [SerializeField] protected RunnerState state;
    [Space]
    [Header("Track Check")]
    [SerializeField] protected LayerMask whatIsTrack;
    [Header("Particles Controller")]
    [SerializeField] protected RunnerParticles particleController;
    [Space]
    [SerializeField] protected List<RunnerObject> _avaliableRunners = new List<RunnerObject>();

    private event Action<RunnerState> OnStateChange;
    public event Action<SportType, ARunner> OnRunnerChanged;


    protected Animator _runnerAnimator;
    protected RunnerObject _currentRunner;

    protected RigidbodyConstraints defaultDodyConstrain;
    protected  int _finishIndex;
    protected bool _isFinished;
    protected bool _canMove;

    #region Properties

    public SportType RunnerType
    {
        get => runnerType;
        set
        {
            runnerType = value;
            ChangeRunner(value);
            OnRunnerChanged?.Invoke(value, this);
        }
    }


    public virtual RunnerState State
    {
        get => state;
        set
        {
            state = value;
            OnStateChange?.Invoke(value);
        }
    }

    public virtual RunnerObject CurrentRunner { get => _currentRunner; set => _currentRunner = value; }
    public Rigidbody Body { get => body; set => body = value; }
    public virtual Animator RunnerAnimator { get => _runnerAnimator; set => _runnerAnimator = value; }
    public float DefaultSpeed => defaultSpeed;
    public float Gravity { get => gravity; set => gravity = value; }
    public virtual int FinishIndex => _finishIndex;
    public virtual bool IsFinished { get => _isFinished; set => _isFinished = value; }
    public SportType Type => runnerType;

    #endregion

    #region Abstract Methods

    public abstract void Move(Vector3 dir, float speed);
    public abstract void CheckTrack(bool canCheck, float delay = 0);
    public abstract void FinishStop();

    #endregion

    #region Virtual Methods

    public virtual void SwitchRunner(object someRunner) { }
    public virtual void InitStartRunner(object someRunner) { }

    public virtual void OnReset() { }

    public virtual void FreezeBody(RigidbodyConstraints constrain)
    {
        body.constraints = ~constrain;
    }

    public virtual void UnFreezeBody(RigidbodyConstraints constrain)
    {
        body.constraints &= ~constrain;
    }
    
    public virtual void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }
    

    public virtual void SetSpeed(float value)
    {
        defaultSpeed = value;
    }

    protected virtual void Start()
    {
        // SetAvaliableRunnerList();
        defaultDodyConstrain = body.constraints;
    }
    protected virtual void ChangeRunner(SportType value)
    {
        PlayTrackEventParticle(TrackEventParticleType.SwitchCharacter);
    }

    public virtual void SetFinishAnimation()
    {
        if (_finishIndex > 1)
            _runnerAnimator.Play("Defeated");
        else if (_finishIndex == 1)
            _runnerAnimator.Play("Victory");
    }
    
    public virtual void ThrowAway(Vector3 dir)
    {
        body.velocity = dir;
    }

    public virtual void PlayTrackTypeParticle(SportType type)
    {
        particleController.PlayByTrackType(type);
    }

    public virtual void PlayTrackEventParticle(TrackEventParticleType type)
    {
        particleController.PlayByTrackEvent(type);
    }

    public virtual void StopLoopingParticles()
    {
        particleController.StopAllLoopingParticles();
    }

    public virtual void ToggleRotationCameara(bool on)
    {
        if(rotateCamera != null)
            rotateCamera.SetActive(on);
    }

    #endregion

    
    private void OnEnable()
    {
        OnStateChange += SetState;
    }


    private void OnDisable()
    {
        OnStateChange -= SetState;
    }


    public void SetState(RunnerState newState)
    {
        switch (newState)
        {
            case RunnerState.Default:
                OnDefaultState();
                break;
            case RunnerState.Climb:
                OnClimbState();
                break;
            case RunnerState.ClimbTop:
                StartCoroutine(OnClimbTopState());
                break;
            case RunnerState.Fall:
                StartCoroutine(OnFallState());
                break;
            case RunnerState.StandUp:
                StartCoroutine(OnStandUpState());
                break;
            case RunnerState.Swim:
                OnSwimState();
                break;
            case RunnerState.JumpObstacle:
                OnJumpObstacleState();
                break;
            case RunnerState.JumpSand:
                OnJumpSandState();
                break;
            case RunnerState.HitWeak:
                OnHitWeakState();
                break;
            case RunnerState.HitHard:
                StartCoroutine(OnHitHardState());
                break;
            case RunnerState.Stunned:
                StartCoroutine(OnStunnedState());
                break;
            case RunnerState.Finish:
                OnFinishState();
                break;
            case RunnerState.Land:
                StartCoroutine(OnLandState());
                break;

            default: Debug.Log("State is None");
                break;
        }
    }

    private IEnumerator OnLandState()
    {
        PlayAnimation("Falling to Landing");
        gravity = 50;
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
    }



    #region State Methods
    private IEnumerator OnStunnedState()
    {
        PlayAnimation("Obstacle jump");
        _canMove = false;
        yield return new WaitForSeconds(0.3f);
        _canMove = true;
        gravity = 50;
        PlayTrackEventParticle(TrackEventParticleType.ObstacleHit);
        PlayAnimation("After obstacle break");
        yield return new WaitForSeconds(1.3f);
        State = RunnerState.Default;
    }

    private void OnFinishState()
    {
        UnFreezeBody(RigidbodyConstraints.FreezePositionX);
        _canMove = false;
        PlayAnimation("Obstacle jump");
    }

    private IEnumerator OnHitHardState()
    {
        PlayAnimation("Boxer Punch");
        yield return new WaitForSeconds(1f);
        State = RunnerState.Default;
    }
    private void OnHitWeakState()
    {
        PlayAnimation("Weak Punch");
    }

    private void OnJumpSandState()
    {
        _canMove = false;
        PlayAnimation("Sand Jump");

    }

    private void OnJumpObstacleState()
    {
        PlayAnimation("Obstacle jump");
        _canMove = false;
        gravity = 8;
    }

    private void OnSwimState()
    {
        body.isKinematic = false;
        PlayAnimation("Swimming");
        gravity = 8;
        CheckTrack(true);
    }

    private IEnumerator OnStandUpState()
    {
        body.isKinematic = false;
        _canMove = false;
        PlayAnimation("Stand Up");
        gravity = 8;
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
    }

    private IEnumerator OnFallState()
    {
        body.isKinematic = false;
        _canMove = false;
        CheckTrack(false);
        gravity = 10;
        PlayAnimation("Wall Dump");
        PlayTrackEventParticle(TrackEventParticleType.WallHit);
        yield return new WaitForSeconds(3);
        State = RunnerState.StandUp;
    }

    private IEnumerator OnClimbTopState()
    {
        body.isKinematic = true;
        CheckTrack(false);
        _canMove = false;
        gravity = 0;
        PlayAnimation("Climbing to Top");
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
        yield return new WaitForSeconds(0.3f);
        State = RunnerState.Land;
    }

    private void OnClimbState()
    {
        body.isKinematic = true;
        _canMove = false;
        CheckTrack(false);
        gravity = 0;
        PlayAnimation("Climbing");
    }

    private void OnDefaultState()
    {
        CheckTrack(true);
        _canMove = true;
        gravity = 8;
        body.isKinematic = false;
        Move(Vector3.forward, defaultSpeed);
    }


    #endregion

    private void PlayAnimation(string name)
    {
        if (RunnerAnimator == null)
            return;

        RunnerAnimator.Play(name);
    }

    public void SetAvaliableRunnerList()
    {
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out RunnerObject r))
                _avaliableRunners.Add(r);
        }
        InitStartType();
    }

    public void InitStartType()
    {
        var getTracks = TracksController.Instance.LevelTracks.ToList();
        var firtsTrack = getTracks[0];
        RunnerType = firtsTrack.TrackType;
    }

    public void OnMenu()
    {
        StopAllCoroutines();
        ToggleRotationCameara(false);
        body.constraints = defaultDodyConstrain;
        _canMove = false;
        _isFinished = false;
        _finishIndex = 0;
        body.useGravity = true;
        body.isKinematic = false;
        body.velocity = Vector3.zero;
        transform.position = start.position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnStart()
    {
        State = RunnerState.Default;
        _canMove = true;
    }


    public void Jump(Vector3 dir, float force)
    {
        body.velocity = dir * force;
    }

    public void ClearRunners()
    {
        _avaliableRunners.Clear();
    }

}
