using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TapClimbEvent : TappableTrack
{
    [SerializeField] private WallEvent wallEvent;

    private float _defaultSpeed;

    protected override void OnTapActivate(PlayerRunner player)
    {
        var tapSpeed = player.TapVariables.Where(t => t.Type == type).FirstOrDefault().SpeedPerTap;
        var maxSpeed = player.TapVariables.Where(t => t.Type == type).FirstOrDefault().MaxTapSpeed;

        _defaultSpeed = wallEvent.ClimbSpeed;
        wallEvent.ClimbSpeed += tapSpeed;

        if (wallEvent.ClimbSpeed > maxSpeed)
            wallEvent.ClimbSpeed = maxSpeed;
    }

    protected override void Player_OnRunnerChanged(ARunner r)
    {
        base.Player_OnRunnerChanged(r);
        wallEvent.ClimbSpeed = _defaultSpeed;
    }

}
