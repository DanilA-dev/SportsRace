using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FinishCup : MonoBehaviour
{

    [SerializeField] private Vector3 cupScale;

    public void SetNexCup(List<Cup> cups)
    {
        var cup = cups.Where(c => c.State == CupState.Locked).FirstOrDefault();
        var createdCup = Instantiate(cup, this.transform);
        createdCup.DisableLockSkin();
        createdCup.transform.localPosition = createdCup.FinishPos;
        createdCup.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        createdCup.transform.localScale = cupScale;

        foreach (Transform child in createdCup.transform)
            child.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
