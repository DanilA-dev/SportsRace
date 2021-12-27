using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SandEvent : ATrackEvent
{
    [SerializeField] private SportType type;
    [SerializeField] private float jumpForce;
    [SerializeField] private float yOffset;
    [SerializeField] private float minimumDistanceToPoint;
    [SerializeField] private Transform jumpPoint1;
    [SerializeField] private Transform jumpPoint2;
    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;
    private ARunner _currentRunner;
    private bool _isRunnerLeave;
    private bool _subbed;
    private bool _isJumpingNow;

    private Vector3 _jumpDir;


    public float JumpForce { get => jumpForce; set => jumpForce = value; }

    private void Awake()
    {
        _coll = GetComponent<Collider>();
    }


    private IEnumerator ReEnableTrigger()
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _coll.enabled = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent(out ARunner r))
        {
            _subbed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            CheckType(r);
            GetJumpDirection(r);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            _isRunnerLeave = true;
            r.State = RunnerState.Default;
        }
    }


    private Vector3 GetJumpDirection(ARunner r)
    {
        Vector3 dirToFirstPoint = jumpPoint1.localPosition - r.transform.position;
        if (dirToFirstPoint.magnitude > minimumDistanceToPoint)
            return dirToFirstPoint.normalized;

        Vector3 dirToSecondPoint = jumpPoint2.localPosition - r.transform.position;
        if (dirToFirstPoint.magnitude > minimumDistanceToPoint)
            return dirToSecondPoint.normalized;

        return Vector3.zero;
    }

    private void CheckType(ARunner r)
    {
        if(r.Type == type)
        {
            if(r.State != RunnerState.JumpSand && !_isJumpingNow)
            {
                StartCoroutine(Jumping(r));
            }

        }
    }

    private IEnumerator Jumping(ARunner r)
    {
        _isJumpingNow = true;
        r.State = RunnerState.JumpSand;
        r.Jump(Vector3.forward, JumpForce);
        yield return new WaitForSeconds(1);
        r.State = RunnerState.Default;
        yield return new WaitForSeconds(1);
        _isJumpingNow = false;
        CheckIfRunnerLeft(r);
    }

    private void CheckIfRunnerLeft(ARunner r)
    {
        if (_isRunnerLeave)
            r.State = RunnerState.Default;
        else
            StartCoroutine(Jumping(r));
    }

    public override void OnRunnerChanged(ARunner r)
    {
        if (!_subbed)
            return;

        StopAllCoroutines();
        r.StopAllCoroutines();
        r.State = RunnerState.Default;
        OnSwitchRunner?.Invoke();
    }

    public void ColliderReEnable()
    {
        StartCoroutine(ReEnableTrigger());
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        StopAllCoroutines();
        _subbed = false;
    }
}
