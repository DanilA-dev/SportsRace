using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent OnSwitchRunner;

    private Collider _coll;

    private bool _isRunnerOut;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            r.OnRunnerChanged += OnRunnerChange;
            r.State = RunnerState.Swim;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isRunnerOut = true;
        if (other.TryGetComponent(out ARunner r))
        {
            r.OnRunnerChanged -= OnRunnerChange;
            r.State = RunnerState.Default;
        }
    }

    private void OnRunnerChange(SportType arg1, ARunner r)
    {
        if (_isRunnerOut)
            return;

        r.State = RunnerState.Swim;
        OnSwitchRunner?.Invoke();
    }
}
