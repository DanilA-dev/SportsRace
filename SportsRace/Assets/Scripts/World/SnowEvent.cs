using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowEvent : ATrackEvent
{
    [SerializeField] private SportType type;
    [SerializeField] private Vector3 offset;

    private Vector3 _defaultLocalPos;

    public override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            if (r.Type == type)
                return;

            LoverRunners(r);
        }
    }

    private void LoverRunners(ARunner r)
    {
        foreach (Transform t in r.transform)
        {
            if(t.TryGetComponent(out RunnerObject runnerObj))
            {
                _defaultLocalPos = runnerObj.transform.localPosition;

                if (runnerObj.Type != type)
                    runnerObj.transform.localPosition = new Vector3(runnerObj.transform.localPosition.x,
                                                               runnerObj.transform.localPosition.y + offset.y,
                                                                runnerObj.transform.localPosition.z);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ARunner r))
        {
            foreach (Transform t in r.transform)
            {
                if (t.TryGetComponent(out RunnerObject runnerObj))
                {
                    runnerObj.transform.localPosition = _defaultLocalPos;
                }

            }
        }
    }
}  
