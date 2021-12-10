using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxEvent : MonoBehaviour
{
    [SerializeField] private SportType type;
    [SerializeField] private float destroyForce;
    [SerializeField] private Transform forceToPoint;
    [SerializeField] private List<Rigidbody> boxPropParts = new List<Rigidbody>();
    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;
    private bool _propDestroyed;

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
            r.OnRunnerChanged += OnRunnerChange;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            CheckType(r);
        }
    }

    private void CheckType(ARunner r)
    {
        if(r.Type == type)
        {
            if(r.State != RunnerState.HitHard && !_propDestroyed)
            {
                r.State = RunnerState.HitHard;
                _propDestroyed = true;
                StartCoroutine(DestroyBoxProp(1));
            }
        }
        else
        {
              r.State = RunnerState.HitWeak;
        }
    }

    private IEnumerator DestroyBoxProp(float time)
    {
        yield return new WaitForSeconds(time);

        foreach (var r in boxPropParts)
        {
            r.isKinematic = false;
            r.AddForce(forceToPoint.localPosition * destroyForce,ForceMode.Impulse);
        }
    }

    private void OnRunnerChange(SportType arg1, ARunner r)
    {
        StopAllCoroutines();
        r.StopAllCoroutines();
        r.State = RunnerState.Default;
        OnSwitchRunner?.Invoke();
    }

    public void ColliderReEnable()
    {
        StartCoroutine(ReEnableTrigger());
    }
}
