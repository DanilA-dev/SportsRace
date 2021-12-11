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


    public SportType TrackType => trackType;
    public Transform BeginPoint => beginPoint;
    public Transform EndPoint => endPoint;


    private void OnTriggerStay(Collider other)
    {
        if(GameController.CurrentState == GameState.Core)
        {
            if (other.TryGetComponent(out ARunner runner))
                runner.CheckTrack(true);
        }
    }
    
}
