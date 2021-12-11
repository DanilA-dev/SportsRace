using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SandEvent : ATrackEvent
{
    [SerializeField] private SportType type;
    [SerializeField] private float jumpForce;
    [SerializeField] private float minimumDistanceToPoint;
    [SerializeField] private Transform jumpPoint1;
    [SerializeField] private Transform jumpPoint2;
    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;
    private ARunner _currentRunner;
    private bool _isRunnerLeave;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            _currentRunner = r;
            _currentRunner.OnRunnerChanged += OnRunnerChange;
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
            if(r.State != RunnerState.JumpSand)
            {
                StartCoroutine(Jumping(r));
            }

        }
    }

    private IEnumerator Jumping(ARunner r)
    {
            r.State = RunnerState.JumpSand;
            r.Jump(-GetJumpDirection(r), jumpForce);
            yield return new WaitForSeconds(1);
            r.RunnerAnimator.Play("Idle");
            yield return new WaitForSeconds(1);
            CheckIfRunnerLeft(r);
    }

    private void CheckIfRunnerLeft(ARunner r)
    {
        if (_isRunnerLeave)
            r.State = RunnerState.Default;
        else
            StartCoroutine(Jumping(r));
    }

    private void OnRunnerChange(SportType newType, ARunner r)
    {
       // StopAllCoroutines();
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
        if (_currentRunner != null)
        {
            _currentRunner.OnRunnerChanged -= OnRunnerChange;
            Debug.Log($"Event {this.name} is unsubed!");
        }
        StopAllCoroutines();
    }
}
