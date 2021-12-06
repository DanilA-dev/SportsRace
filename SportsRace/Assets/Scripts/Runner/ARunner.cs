using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ARunner : MonoBehaviour
{
    [Header("Body and speed")]
    [SerializeField] protected float defaultSpeed;
    [SerializeField] protected Rigidbody body;
    [Header("CurrentType")]
    [SerializeField] protected SportType runnerType;
    [Space]
    [Header("Track Check")]
    [SerializeField] protected LayerMask whatIsTrack;
    [Space]
    [SerializeField] protected List<RunnerObject> _avaliableRunners = new List<RunnerObject>();


    protected Animator _runnerAnimator;
    protected RunnerObject _currentRunner;

    protected int _finishIndex;
    protected bool _isFinished;


    #region Properties

    public Rigidbody Body { get => body; set => body = value; }
    public Animator RunnerAnimator { get => _runnerAnimator; set => _runnerAnimator = value; }
    public float DefaultSpeed => defaultSpeed;
    public int FinishIndex => _finishIndex;
    public bool IsFinished { get => _isFinished; set => _isFinished = value; }
    public SportType Type => runnerType;

    #endregion

    public abstract void Move(Vector3 dir, float speed);
    public abstract void CheckTrack();
    public abstract void FinishStop();


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

    public void SetAvaliableRunnerList(List<RunnerObject> newList)
    {
        _avaliableRunners = newList;
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
