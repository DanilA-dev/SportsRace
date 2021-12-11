using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATrackEvent : MonoBehaviour
{

    public virtual void Unsubscribe()
    {
        StopAllCoroutines();
    }
}
