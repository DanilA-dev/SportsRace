using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableObstacle : MonoBehaviour
{
    [SerializeField] private SportType type;
    [SerializeField] private List<Rigidbody> breakParts = new List<Rigidbody>();

    [SerializeField] private UnityEvent OnBreak;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out ARunner r))
        {
            if (r.Type != type)
                Break(r);
        }
    }

    private void Break(ARunner runner)
    {
        foreach (var r in breakParts)
        {
            r.isKinematic = false;
            r.AddForce(Vector3.forward * 10, ForceMode.Impulse);
        }
        runner.Body.AddForce(-Vector3.forward * 300, ForceMode.Impulse);
        OnBreak?.Invoke();
    }
}
