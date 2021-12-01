using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<IRunner>() != null)
        {
            var r = other.GetComponentInParent<IRunner>();
            r.Stop();
            StartCoroutine(PedestalRoutine());
        }
    }

    private IEnumerator PedestalRoutine()
    {
        yield break;
    }    
}
