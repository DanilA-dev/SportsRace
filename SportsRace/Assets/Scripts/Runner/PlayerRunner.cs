using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderMaterial;
    [SerializeField] private TrackType trackTypeRunner;
    [SerializeField] private float speed;

    [SerializeField] private bool _isGameStarted = false;

    private void Update()
    {
        if (!_isGameStarted)
            return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SwitchRunner(TrackType newType, Material m)
    {
        trackTypeRunner = newType;
        CheckTrack();
        SetTestMaterial(m);
    }

    private void SetTestMaterial(Material m)
    {
        renderMaterial.material = m;
    }

    private void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    private void ReduceSpeed(float reduceMultiplier)
    {
        speed *= reduceMultiplier;
    }

    private void CheckTrack()
    {
        if(Physics.CheckSphere(transform.position, 1))
        {
            if(TryGetComponent<TrackEntity>(out TrackEntity t))
            {
                if (t.TrackType == trackTypeRunner)
                {
                    SetSpeed(t.TrackSpeed);
                    Debug.Log("<color=green> Same Type! </color>");
                }
                else
                    Punish(t);
            }    
        }
    }

    private void Punish(TrackEntity t)
    {
        Debug.Log("<color=red> Wrong Type! </color>");
        ReduceSpeed(t.SpeedReduce);
        //bad animation
        //speed reduce
    }
}
