using UnityEngine;

public enum SportType
{
    SprintTrack,
    BoxerTrack,
    WaterTrack,
    SandTrack,
    ClimbingTrack,
    SnowTrack,
    SprintObstaclesTrack
}

public class TrackEntity : MonoBehaviour
{
    [SerializeField] private SportType trackType;

    public SportType TrackType => trackType;
    


    private void OnTriggerStay(Collider other)
    {
        if(GameController.CurrentState == GameState.Core)
        {
            if (other.TryGetComponent(out ARunner runner))
                runner.CheckTrack();
        }
    }
}
