using UnityEngine;
using System.Collections;

 public class TapSandEvent : TappableTrack
 {
    [SerializeField] private float startJumpForce = 5;
    [SerializeField] private float forcePerTap = 2;
    [SerializeField] private float maxForce = 10;
    [SerializeField] private SandEvent sandEvent;

    private void OnEnable()
    {
        sandEvent.JumpForce = startJumpForce;
    }

    protected override void OnTapActivate(PlayerRunner player)
    {
        base.OnTapActivate(player);
        sandEvent.JumpForce += forcePerTap;

        if (sandEvent.JumpForce > maxForce)
            sandEvent.JumpForce = maxForce;

    }

    protected override void Player_OnRunnerChanged(ARunner r)
    {
        base.Player_OnRunnerChanged(r);
    }


}
