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
    [SerializeField] private float sameTypeSpeed;
    [SerializeField] private float wrongTypeSpeed;


    #region Properties

    public TrackType TrackType => trackType;
    public float WrongTypeSpeed => wrongTypeSpeed;
    public float SameTypeSpeed => sameTypeSpeed;

    #endregion


    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var p = other.gameObject.GetComponent<PlayerRunner>();

            if (p != null)
                p.CheckTrack();
        }
    }
}
