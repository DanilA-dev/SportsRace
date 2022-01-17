using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxEvent : ATrackEvent
{
    [SerializeField] private SportType type;
    [SerializeField] private bool isTapToDestroy;
    [SerializeField] private float destroyForce;
    [SerializeField] private Transform forceToPoint;
    [SerializeField] private List<Rigidbody> boxPropParts = new List<Rigidbody>();
    [SerializeField] private UnityEvent OnSwitchRunner;

    private Vector3[] _boxPropPositions = new Vector3[2];
    private Quaternion[] _boxPropRotations = new Quaternion[2];


    private Collider _coll;
    private ARunner _currentRunner;
    private bool _propDestroyed;
    private bool _subbed;

    private bool _initTapToDestroy;

    public bool IsTapToDestroy { get => isTapToDestroy; set => isTapToDestroy = value; }

    private void Awake()
    {
        _coll = GetComponent<Collider>();
        _initTapToDestroy = isTapToDestroy;

        for (int i = 0; i < boxPropParts.Count; i++)
        {
            _boxPropPositions[i] = boxPropParts[i].transform.localPosition;
            _boxPropRotations[i] = boxPropParts[i].transform.localRotation;
        }

        GameController.OnRestartLevel += OnRestartLevel;

    }

    private void OnRestartLevel()
    {
        Init();
    }

    public override void Init()
    {
        for (int i = 0; i < boxPropParts.Count; i++)
        {
            boxPropParts[i].transform.localPosition = _boxPropPositions[i];
            boxPropParts[i].transform.localRotation = _boxPropRotations[i];
            boxPropParts[i].isKinematic = true;
        }

        StopAllCoroutines();
        _propDestroyed = false;
        _coll.enabled = true;
        IsTapToDestroy = _initTapToDestroy;
    }


    private IEnumerator ReEnableTrigger()
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(0.02f);
        _coll.enabled = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.TryGetComponent(out ARunner r))
        {
            _subbed = true;
            CheckType(r);
        }
    }


    private void CheckType(ARunner r)
    {
        if(r.Type == type)
        {
            if(r.State != RunnerState.HitHard && !_propDestroyed)
            {
                if (isTapToDestroy)
                {
                    r.State = RunnerState.Idle;
                    return;
                }

                BreakProp(r);
            }
        }
        else
            r.State = RunnerState.HitWeak;
    }


    public void BreakProp(ARunner r)
    {
        r.State = RunnerState.HitHard;
        StartCoroutine(DestroyingProp(0.3f, r));
    }

    private IEnumerator DestroyingProp(float time, ARunner runner)
    {
        yield return new WaitForSeconds(time);
        runner.ParticleController.PlayByTrackEvent(TrackEventParticleType.BoxPunch);
        foreach (var r in boxPropParts)
        {
            r.isKinematic = false;
            r.AddForce(forceToPoint.localPosition * destroyForce,ForceMode.Impulse);
        }
        _propDestroyed = true;
       // Unsubscribe();
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


    private void OnDestroy()
    {
        GameController.OnRestartLevel -= OnRestartLevel;

    }

    public override void Unsubscribe()
    {
        _subbed = false;
        base.Unsubscribe();
        StopAllCoroutines();
    }
}
