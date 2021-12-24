using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CupState
{
    Locked, Open
}

public class Cup : MonoBehaviour
{
    [SerializeField] private CupState state;
    [SerializeField] private Vector3 finishLocalPos;
    [SerializeField] private GameObject locked;

    public CupState State => state;
    public Vector3 FinishPos => finishLocalPos;


    public void UnLock()
    {
        locked.SetActive(false);
        state = CupState.Open;
    }

    public void DisableLockSkin()
    {
        locked.gameObject.SetActive(false);
    }

   
}
