using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleTrackEvent : ATrackEvent
{
    [SerializeField] private SportType type;
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private float force;
    [SerializeField] private float propForce;
    [SerializeField] private List<Rigidbody> obstacleProp = new List<Rigidbody>();

    [SerializeField] private UnityEvent OnSwitchRunner;

    private Vector3[] _propInitPos = new Vector3[2];
    private Quaternion[] _propRotations = new Quaternion[2];


    private Collider _coll;
    private ARunner _currentRunner;
    private bool _isJumped;
    private bool _subbed;

    private void Awake()
    {
        _coll = GetComponent<Collider>();

        for (int i = 0; i < obstacleProp.Count; i++)
        {
            _propInitPos[i] = obstacleProp[i].transform.localPosition;
            _propRotations[i] = obstacleProp[i].transform.localRotation;
        }

        GameController.OnRestartLevel += OnRestart;
    }

    private void OnRestart()
    {
        Init();
    }

    public override void Init()
    {
        for (int i = 0; i < obstacleProp.Count; i++)
        {
            obstacleProp[i].transform.localPosition = _propInitPos[i];
            obstacleProp[i].transform.localRotation = _propRotations[i];
            obstacleProp[i].isKinematic = true;
        }
        StopAllCoroutines();
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
        if(other.TryGetComponent(out ARunner r))
        {
            CheckType(r);
        }
    }

    private void CheckType(ARunner r)
    {
        if (r.Type == type)
        {
            if (r.State != RunnerState.JumpObstacle)
                StartCoroutine(JumpingOver(r));

        }
        else
        {
            if(r.State != RunnerState.Stunned)
            {
                StartCoroutine(StunJumping(r));
            }
        }
    }

    private IEnumerator StunJumping(ARunner r)
    {
        var jumpDir = new Vector3(r.Body.velocity.x, r.transform.position.y + 1, r.Body.velocity.z);
        r.ParticleController.PlayByTrackEvent(TrackEventParticleType.ObstacleHit);
        r.State = RunnerState.Stunned;
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

    public override void OnRunnerChanged(ARunner r)
    {
        if (!_subbed)
            return;

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

    private void OnDestroy()
    {
        GameController.OnRestartLevel -= OnRestart;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        StopAllCoroutines();
        _subbed = false;
    }
}
