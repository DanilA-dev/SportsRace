using UnityEngine;

public enum TrackType
{
    RunningTrack,
    WallObstacleTrack,
    WaterTrack,
    SandTrack,
    ClimbingTrack,
    SnowTrack,
    RunningObstaclesTrack
}

public class TrackEntity : MonoBehaviour
{
    [SerializeField] private TrackType trackType;
    [SerializeField] private float trackSpeed;
    [SerializeField] private float speedReduce;


    #region Properties

    public TrackType TrackType => trackType;
    public float SpeedReduce => speedReduce;
    public float TrackSpeed => trackSpeed;

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        //check if player/bot enter and invoke their enter method(animation)
    }
}
