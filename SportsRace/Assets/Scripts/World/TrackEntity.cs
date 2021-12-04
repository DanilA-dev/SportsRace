using UnityEngine;

public enum SportType
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
    [SerializeField] private SportType trackType;
    [SerializeField] private float sameTypeSpeed;
    [SerializeField] private float wrongTypeSpeed;


    #region Properties

    public SportType TrackType => trackType;
    public float WrongTypeSpeed => wrongTypeSpeed;
    public float SameTypeSpeed => sameTypeSpeed;

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ARunner runner))
            runner.CheckTrack();
    }
}
