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


    private void OnTriggerStay(Collider other)
    {
        //check if player/bot enter and invoke their enter method(animation)
        if(other.gameObject.CompareTag("Player"))
        {
            var p = other.GetComponentInParent<PlayerRunner>();

            if(p != null && p.TrackTypeRunner != trackType)
                p.CheckTrack();

        }
    }
}
