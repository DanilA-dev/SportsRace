using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerRunner : MonoBehaviour, IRunner
{
    [SerializeField] private MeshRenderer renderMaterial;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private TrackType trackTypeRunner;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private LayerMask whatIsTrack;

    public static event Action<float> OnSpeedChange;

    private bool _canMove;

    public float DefaultSpeed => defaultSpeed;
    public TrackType TrackTypeRunner => trackTypeRunner;


    private void Start()
    {
        InitStartType();
        _canMove = false;
        agent.enabled = false;
    }

    private void Update()
    {
        if (GameController.CurrentState == GameState.Core)
            Move(transform.forward, defaultSpeed);
    }

    public void SwitchRunner(TrackType newType, Material m)
    {
        trackTypeRunner = newType;
        CheckTrack();
        SetTestMaterial(m);
    }

    private void InitStartType()
    {
        var getTracks = TracksController.Instance.GeneratedTracks.ToList();
        var firtsTrack = getTracks[0];

        trackTypeRunner = firtsTrack.TrackType;
        CheckTrack();
    }

    private void SetTestMaterial(Material m)
    {
        renderMaterial.material = m;
    }

    public void SetSpeed(float speed)
    {
        this.defaultSpeed = speed;
    }


    public void CheckTrack()
    {
        //better do with check spehere 

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, whatIsTrack))
        {
            if(hit.collider.TryGetComponent(out TrackEntity t))
            {
                if (t.TrackType == trackTypeRunner)
                {
                    SetSpeed(t.SameTypeSpeed);
                    OnSpeedChange?.Invoke(this.defaultSpeed);
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
        SetSpeed(t.WrongTypeSpeed);
        OnSpeedChange?.Invoke(this.defaultSpeed);
        //bad animation
    }


    public void FinishStop()
    {
        _canMove = false;
        agent.enabled = true;
        defaultSpeed = 0;
    }

    public void ResetToStart()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 19.54f);
    }

    public void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        transform.position += dir * speed * Time.deltaTime;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
