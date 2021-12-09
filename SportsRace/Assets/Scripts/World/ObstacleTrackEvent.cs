using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleTrackEvent : MonoBehaviour
{
    [SerializeField] private SportType type;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private float force;
    [SerializeField] private float propForce;
    [SerializeField] private List<Rigidbody> obstacleProp = new List<Rigidbody>();

    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;
    private bool _isJumped;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            r.OnRunnerChanged += OnRunnerChange;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            CheckType(r);
        }
    }

    private void CheckType(ARunner r)
    {
        if (r.Type == type)
        {
            if (r.State != RunnerState.JumpObstacle && !_isJumped)
                StartCoroutine(JumpingOver(r));

        }
        else
        {
            if(r.State != RunnerState.Stunned && !_isJumped)
            {
                StartCoroutine(StunJumping(r));
            }
        }
    }

    private IEnumerator StunJumping(ARunner r)
    {
        r.State = RunnerState.Stunned;
        var jumpDir = new Vector3(r.Body.velocity.x, r.transform.position.y + 1, r.Body.velocity.z);
        r.Jump(jumpDir, force);
        foreach (var body in obstacleProp)
        {
            body.isKinematic = false;
            body.AddForce(Vector3.forward * propForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1);
        _isJumped = true;
    }

    private IEnumerator JumpingOver(ARunner r)
    {
        var jumpDir = new Vector3(r.Body.velocity.x, r.transform.position.y + force, r.Body.velocity.z);

        r.State = RunnerState.JumpObstacle;
        r.Jump(jumpDir, force);
        yield return new WaitForSeconds(0.6f);
        _isJumped = true;
        r.State = RunnerState.Default;
    }

    private void OnRunnerChange(SportType arg1, ARunner r)
    {
        StopAllCoroutines();
        r.StopAllCoroutines();
        r.State = RunnerState.Default;
        OnSwitchRunner?.Invoke();
    }

    private IEnumerator ReEnableTrigger()
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _coll.enabled = true;
    }

    public void ColliderReEnable()
    {
        StartCoroutine(ReEnableTrigger());
    }

}
