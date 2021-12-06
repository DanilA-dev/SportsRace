using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackEvent : MonoBehaviour
{
    [SerializeField] private SportType type;
    [SerializeField] private float tempTime;
    [SerializeField] private AnimationClip animValue;
    [SerializeField] private float force;
    [SerializeField] private Vector3 forceDir;

    [SerializeField] private UnityEvent AfterEvent;
    
    private bool _isPlayerIn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            if(r.Type == type)
            {
                r.RunnerAnimator.Play("Obstacle jump");
                r.Jump(r.transform.up, force);
               // r.Body.AddForce(r.transform.up * force, ForceMode.Impulse);
               // StartCoroutine(AfterJump(r));
            }
        }
    }

    private IEnumerator AfterJump(ARunner r)
    {
        yield return new WaitForSeconds(tempTime);
        r.Body.AddForce(Vector3.down * force, ForceMode.Impulse);
    }
}
