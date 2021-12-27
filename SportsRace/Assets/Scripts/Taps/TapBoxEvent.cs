using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Events;

public class TapBoxEvent : TappableTrack
 {
    [SerializeField] private BoxEvent boxEvent;
     
    protected override void OnTapActivate(PlayerRunner player)
    {
        base.OnTapActivate(player);
        boxEvent.IsTapToDestroy = false;
        boxEvent.ColliderReEnable();
    }

    protected override void Player_OnRunnerChanged(ARunner r)
    {
        base.Player_OnRunnerChanged(r);
        boxEvent.IsTapToDestroy = true;
    }


}
