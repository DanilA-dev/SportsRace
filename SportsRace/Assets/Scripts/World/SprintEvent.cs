using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintEvent : ATrackEvent
{
    //class just for player

    [SerializeField] private float speedPerTap = 1;
    [SerializeField] private float maxSpeed = 300;


    private float _playerDefaultSpeed;
    private bool _subbed;

    public void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerRunner player))
        {
            if(player.Type == SportType.SprintTrack)
            {
                player.OnTappableTrackEnter();
                player.OnTapEvent += OnTapActivate;
                player.OnRunnerChanged += Player_OnRunnerChanged;
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerRunner player))
        {
            player.OnTappableTrackExit();
            player.SetCheckTracker(true);

        }
    }

    private void Player_OnRunnerChanged(ARunner r)
    {
        var player = r.GetComponent<PlayerRunner>();
        player.SetCheckTracker(true);
    }

    private void OnTapActivate(PlayerRunner player)
    {
        player.SetCheckTracker(false);
        player.DefaultSpeed += speedPerTap;

        if (player.DefaultSpeed > maxSpeed)
            player.DefaultSpeed = maxSpeed;
    }
}
