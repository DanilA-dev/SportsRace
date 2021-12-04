using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRunner : ARunner
{
    [SerializeField,Range(0,10)] private float switchTime;
    [SerializeField] private MeshRenderer renderMaterial;

    private bool _canMove;

    private void FixedUpdate()
    {
        if (GameController.CurrentState == GameState.Core)
            Move(transform.forward, defaultSpeed);
    }

    public override void CheckTrack()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 3, whatIsTrack);
        if (col[0].TryGetComponent(out TrackEntity t))
        {
            if (t.TrackType == runnerType)
                SetSpeed(t.SameTypeSpeed);
            else
                Punish(t);
        }
    }

    private void Punish(TrackEntity t)
    {
        SetSpeed(t.WrongTypeSpeed);
        StartCoroutine(SwitchRunner(t));
    }

    private IEnumerator SwitchRunner(TrackEntity t)
    {
        yield return new WaitForSeconds(switchTime);
        runnerType = t.TrackType;
        CheckTrack();
        renderMaterial.material = t.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public override void SetFinishPosition(int index)
    {
        _finishIndex = index;
    }

    public void SetSpeed(float speed)
    {
        defaultSpeed = speed;
    }

    public override void FinishStop()
    {
        _canMove = false;
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
}
