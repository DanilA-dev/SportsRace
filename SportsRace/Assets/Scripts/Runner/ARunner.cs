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
    [SerializeField] protected TrackType runnerType;
    [Space]
    [SerializeField] protected NavMeshAgent agent;
    [Header("Track Check")]
    [SerializeField] protected LayerMask whatIsTrack;

    protected int _finishIndex;
    protected bool _isFinished;

    #region Properties

    public Rigidbody Body { get => body; set => body = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public float DefaultSpeed => defaultSpeed;
    public TrackType RunnerType => runnerType;
    public int FinishIndex => _finishIndex;
    public bool IsFinished { get => _isFinished; set => _isFinished = value; }

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


}
