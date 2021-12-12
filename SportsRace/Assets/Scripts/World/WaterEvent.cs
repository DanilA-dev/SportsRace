using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterEvent : ATrackEvent
{
    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;
    private ARunner _currentRunner;

    private bool _isRunnerOut;
    private bool _subbed;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
    }

    public void ColliderReEnable()
    {
        StartCoroutine(ReEnableTrigger());
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
            r.State = RunnerState.Swim;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isRunnerOut = true;
        if (other.TryGetComponent(out ARunner r))
        {
            r.OnRunnerChanged -= OnRunnerChanged;
            r.State = RunnerState.Default;
        }
    }

    public override void OnRunnerChanged(SportType arg1, ARunner r)
    {
        if (!_subbed)
            return;

        if (_isRunnerOut)
            return;

        r.State = RunnerState.Swim;
        OnSwitchRunner?.Invoke();
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        StopAllCoroutines();
        _subbed = false;
        if (Runner != null)
            Runner.State = RunnerState.Default;
    }
}
