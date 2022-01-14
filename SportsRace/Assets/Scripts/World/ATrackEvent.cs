using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATrackEvent : MonoBehaviour
{

    protected ARunner Runner { get; set; }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            Runner = r;
            Runner.OnRunnerChanged += OnRunnerChanged;
        }
    }

    public virtual void OnRunnerChanged(ARunner arg2) { }

    public virtual void Init() { }

    public virtual void Unsubscribe()
    {
        StopAllCoroutines();
        if (Runner != null)
            Runner.OnRunnerChanged -= OnRunnerChanged;
    }
}
