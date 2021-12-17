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

    [SerializeField] private Transform start;
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
    [SerializeField] private RunnerTrackVariables trackVariables;
    [SerializeField] protected List<RunnerObject> _avaliableRunners = new List<RunnerObject>();

    private event Action<RunnerState> OnStateChange;
    public event Action<ARunner> OnRunnerChanged;


    protected Animator _runnerAnimator;
    protected RunnerObject _currentRunner;

    protected RigidbodyConstraints defaultDodyConstrain;
    protected int _finishIndex;
    protected bool _isFinished;
    protected bool _canMove;

    #region Properties

    public RunnerTrackVariables TrackVariables => trackVariables;
    public virtual IPlayer Player { get; }

    public abstract Collider RunnerCollider { get; set; }

    public RunnerParticles ParticleController => particleController;
    public SportType RunnerType
    {
        get => runnerType;
        set
        {
            runnerType = value;
            ChangeRunner(value);
            OnRunnerChanged?.Invoke(this);
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

    #endregion

    #region Virtual Methods


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
        defaultDodyConstrain = body.constraints;
    }
    protected virtual void ChangeRunner(SportType value)
    {
        PlayTrackEventParticle(TrackEventParticleType.SwitchCharacter);
    }

    public virtual void SetFinishAnimation() { }

    public virtual void CheckPosition() { }
    
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

    public virtual void PlayRunnerSpecialParticle(SportType type)
    {
        particleController.PlayRunnerSpecial(type);
    }

    public virtual void StopTrackTypeParticles()
    {
        particleController.StopTrackTypeParticles();
    }

    public virtual void StopRunnerSpecialParticles()
    {
        particleController.StopRunnerSpecialParticles();
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




    #region State Methods
    private IEnumerator OnLandState()
    {
        StopTrackTypeParticles();
        PlayAnimation("Falling to Landing");
        gravity = 70;
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
    }


    private IEnumerator OnStunnedState()
    {
        PlayAnimation("Obstacle jump");
        _canMove = false;
        yield return new WaitForSeconds(0.3f);
        _canMove = true;
        gravity = 50;
        //PlayTrackEventParticle(TrackEventParticleType.ObstacleHit);
        PlayAnimation("After obstacle break");
        yield return new WaitForSeconds(1.3f);
        State = RunnerState.Default;
    }

    private void OnFinishState()
    {
        StopTrackTypeParticles();
        StopRunnerSpecialParticles();
        UnFreezeBody(RigidbodyConstraints.FreezePositionX);
        _canMove = false;
        PlayAnimation("Obstacle jump");
    }

    private IEnumerator OnHitHardState()
    {
        PlayAnimation("Boxer Punch");
        yield return new WaitForSeconds(0.7f);
        State = RunnerState.Default;
    }
    private void OnHitWeakState()
    {
        StopTrackTypeParticles();
        PlayAnimation("Weak Punch");
    }

    private void OnJumpSandState()
    {
        StopTrackTypeParticles();
        _canMove = false;
        PlayAnimation("Sand Jump");
    }

    private void OnJumpObstacleState()
    {
        StopTrackTypeParticles();
        PlayAnimation("Obstacle jump");
        _canMove = false;
        gravity = 8;
    }

    private void OnSwimState()
    {
        body.isKinematic = false;
        PlayAnimation("Swimming");
        gravity = 0;
        CheckTrack(true);
    }

    private IEnumerator OnStandUpState()
    {
        StopTrackTypeParticles();
        body.isKinematic = false;
        _canMove = false;
        PlayAnimation("Stand Up");
        gravity = 8;
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
    }

    private IEnumerator OnFallState()
    {
        StopTrackTypeParticles();
        body.isKinematic = false;
        _canMove = false;
        CheckTrack(false);
        gravity = 10;
        PlayAnimation("Wall Dump");
       // PlayTrackEventParticle(TrackEventParticleType.WallHit);
        yield return new WaitForSeconds(1.5f);
        State = RunnerState.StandUp;
    }

    private IEnumerator OnClimbTopState()
    {
        StopTrackTypeParticles();
        body.isKinematic = true;
       // RunnerCollider.enabled = false;
        CheckTrack(false);
        _canMove = false;
        gravity = 0;
        PlayAnimation("Climbing to Top");
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
        yield return new WaitForSeconds(0.6f);
        RunnerCollider.enabled = true;
        State = RunnerState.Land;
    }

    private void OnClimbState()
    {
        StopTrackTypeParticles();
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

        if(getTracks.Count > 0 && _avaliableRunners.Count > 0)
            RunnerType = firtsTrack.TrackType;
    }

    public virtual void OnMenu()
    {
        StopAllCoroutines();
        particleController.StopTrackTypeParticles();
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

    public virtual void OnStart()
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


[Serializable]
public class RunnerTrackVariables
{
    [Header("Jump Track")]
    [SerializeField] private float sandJumpForce = 10;
    [SerializeField] private float sandJumpYoffset;

    [Header("Wall Track")]
    [SerializeField] private float knockOutForce;
    [SerializeField] private float knockOutYOffset;
    [SerializeField] private float knockOutZOffset;


    public float SandJumpForce => sandJumpForce;
    public float YOffset => sandJumpYoffset;

    public float KnockOutForce => knockOutForce;
    public float KnockOutYOffset => knockOutYOffset;
    public float KnockOutZOffset => knockOutZOffset;
}