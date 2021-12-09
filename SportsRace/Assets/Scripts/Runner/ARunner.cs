using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public enum RunnerState
{
    Default, Climb, ClimbTop, Fall, StandUp, Swim, JumpObstacle, JumpSand, Hit, Finish
}

public abstract class ARunner : MonoBehaviour
{
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
    [Space]
    [SerializeField] protected List<RunnerObject> _avaliableRunners = new List<RunnerObject>();

    private event Action<RunnerState> OnStateChange;
    public static event Action<SportType, ARunner> OnRunnerChanged;


    protected Animator _runnerAnimator;
    protected RunnerObject _currentRunner;

    protected int _finishIndex;
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


    public RunnerState State
    {
        get => state;
        set
        {
            state = value;
            OnStateChange?.Invoke(value);
        }
    }

    public Rigidbody Body { get => body; set => body = value; }
    public Animator RunnerAnimator { get => _runnerAnimator; set => _runnerAnimator = value; }
    public float DefaultSpeed => defaultSpeed;
    public float Gravity { get => gravity; set => gravity = value; }
    public int FinishIndex => _finishIndex;
    public bool IsFinished { get => _isFinished; set => _isFinished = value; }
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
        body.constraints = constrain;
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
        SetAvaliableRunnerList();
    }

    #endregion

    protected virtual void ChangeRunner(SportType value)
    {
        
    }
    private void OnEnable()
    {
        OnStateChange += SetState;
    }

    private void OnDisable()
    {
        OnStateChange -= SetState;
    }

    protected void SetAvaliableRunnerList()
    {
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out RunnerObject r))
                _avaliableRunners.Add(r);
        }
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
                OnFallState();
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
            case RunnerState.Hit:
                OnHitState();
                break;
            case RunnerState.Finish:
                OnFinishState();
                break;

            default: Debug.Log("State is None");
                break;
        }
    }

    #region State Methods

    private void OnFinishState()
    {
        throw new NotImplementedException();
    }

    private void OnHitState()
    {
        throw new NotImplementedException();
    }

    private void OnJumpSandState()
    {
        throw new NotImplementedException();
    }

    private void OnJumpObstacleState()
    {
        throw new NotImplementedException();
    }

    private void OnSwimState()
    {
        body.isKinematic = false;
        _runnerAnimator.Play("Swimming");
        gravity = 8;
        CheckTrack(true);
    }

    private IEnumerator OnStandUpState()
    {
        body.isKinematic = false;
        _canMove = false;
        _runnerAnimator.Play("Stand Up");
        gravity = 8;
        yield return new WaitForSeconds(2);
        State = RunnerState.Default;
    }

    private void OnFallState()
    {
        body.isKinematic = false;
        CheckTrack(false);
        gravity = 10;
        _runnerAnimator.Play("Wall Dump");
    }

    private IEnumerator OnClimbTopState()
    {
        body.isKinematic = true;
        CheckTrack(false);
        _canMove = false;
        gravity = 0;
        _runnerAnimator.Play("Climbing to Top");
        yield return new WaitForSeconds(1);
        State = RunnerState.Default;
        gravity = 20;
       
    }

    private void OnClimbState()
    {
        body.isKinematic = true;
        _canMove = false;
        CheckTrack(false);
        gravity = 0;
        _runnerAnimator.Play("Climbing");
    }

    private void OnDefaultState()
    {
        _canMove = true;
        gravity = 8;
        body.isKinematic = false;
        Move(Vector3.forward, defaultSpeed);
        CheckTrack(true);
    }


    #endregion

    [ContextMenu("Throw Up")]
    public void ThrowUp()
    {
        body.velocity = Vector3.up * 5;
    }

    [ContextMenu("Throw Back")]
    public void ThrowAway(Vector3 dir)
    {
        var t = new Vector3(0, 0, Vector3.Angle(Vector3.back, Vector3.up));
        body.velocity = dir;
    }

    

    public void ClearRunners()
    {
        _avaliableRunners.Clear();
    }

    public void Jump(Vector3 dir, float force)
    {
        Body.velocity = dir * force;
    }
}
