using System.Collections.Generic;
using UnityEngine;

public enum SportType
{
    SprintTrack,
    BoxerTrack,
    WaterTrack,
    SandTrack,
    ClimbingTrack,
    SnowTrack,
    SprintObstaclesTrack,
    Finish,
    Start
}

public class TrackEntity : MonoBehaviour
{
    [SerializeField] private SportType trackType;
    [SerializeField] private Transform beginPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private List<ATrackEvent> trackEvents = new List<ATrackEvent>();


    public SportType TrackType => trackType;
    public Transform BeginPoint => beginPoint;
    public Transform EndPoint => endPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (GameController.CurrentState == GameState.Core)
        {
            if (other.TryGetComponent(out ARunner runner))
            {
                runner.PlayTrackTypeParticle(trackType);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameController.CurrentState == GameState.Core)
        {
            if (other.TryGetComponent(out ARunner runner))
                runner.StopLoopingParticles();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(GameController.CurrentState == GameState.Core)
        {
            if (other.TryGetComponent(out ARunner runner))
                 runner.CheckTrack(true);
        }
    }

    public void UnsubscribeFromEvents()
    {
        foreach (var t in trackEvents)
            t.Unsubscribe();
    }
    
}
