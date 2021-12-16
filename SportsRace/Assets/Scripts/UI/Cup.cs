using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private GameObject locked;


    public void UnLock()
    {
        locked.SetActive(false);
    }

   
}
