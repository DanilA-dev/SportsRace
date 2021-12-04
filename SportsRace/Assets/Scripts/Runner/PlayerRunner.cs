using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerRunner : ARunner
{
    [SerializeField] private MeshRenderer renderMaterial;

    public static event Action<float> OnSpeedChange;

    private bool _canMove;
   

    private void Start()
    {
        InitStartType();
        _canMove = true;
    }

    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameState.Core)
            Move(transform.forward, defaultSpeed);
    }

    public void SwitchRunner(TrackType newType, Material m)
    {
        runnerType = newType;
        CheckTrack();
        SetTestMaterial(m);
    }

    private void InitStartType()
    {
        var getTracks = TracksController.Instance.GeneratedTracks.ToList();
        var firtsTrack = getTracks[0];

        runnerType = firtsTrack.TrackType;
        CheckTrack();
    }

    private void SetTestMaterial(Material m)
    {
        renderMaterial.material = m;
    }

    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public void SetSpeed(float speed)
    {
        this.defaultSpeed = speed;
    }


    public override void CheckTrack()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 3, whatIsTrack);
        if(col[0].TryGetComponent(out TrackEntity t))
        {
            if (t.TrackType == runnerType)
            {
                SetSpeed(t.SameTypeSpeed);
                OnSpeedChange?.Invoke(this.defaultSpeed);
                Debug.Log("<color=green> Same Type! </color>");
            }
            else
                Punish(t);
        }
    }

    private void Punish(TrackEntity t)
    {
        Debug.Log("<color=red> Wrong Type! </color>");
        SetSpeed(t.WrongTypeSpeed);
        OnSpeedChange?.Invoke(this.defaultSpeed);
        //bad animation
    }

    public override void OnReset()
    {
        _canMove = true;
        _isFinished = false;
        _finishIndex = 0;
        SetSpeed(defaultSpeed);
        body.useGravity = true;
        body.isKinematic = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 19.54f);
    }

    public override void Move(Vector3 dir, float speed)
    {
        if (!_canMove)
            return;

        //body.velocity = dir * speed * Time.deltaTime;
        transform.position += dir * speed * Time.deltaTime;
    }

    public override void FinishStop()
    {
        _canMove = false;
    }
}
