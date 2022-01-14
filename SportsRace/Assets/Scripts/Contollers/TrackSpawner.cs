using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    [SerializeField] private int tracksAmount;

    [SerializeField] private List<TrackPool> tracks = new List<TrackPool>();
    [SerializeField] private List<TrackEntity> createdTracks = new List<TrackEntity>();


    public List<TrackEntity> CreatedTracks => createdTracks;
    private ObjectPooler<TrackEntity> _objectPooler;

    private void Awake()
    {
        for (int i = 0; i < tracks.Count; i++)
        {
            _objectPooler = new ObjectPooler<TrackEntity>(tracks[i].Track, this.transform);
            _objectPooler.CreatePool(tracks[i].Quantity);
        }

        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out TrackEntity track))
            {
                createdTracks.Add(track);
                t.parent = null;
            }
        }

        
    }

    [ContextMenu("Create")]
    private void Create()
    {
        foreach (var t in createdTracks)
        {
            t.transform.parent = null;
        }

        for (int i = 0; i < tracksAmount; i++)
        {
            var newTrack = GetRandomTrack();
            newTrack.gameObject.SetActive(true);

            var trackRotation = Quaternion.Euler(new Vector3(-90, 0, 90));
            var nextPos = new Vector3(newTrack.PosOffset.x, newTrack.PosOffset.y, (createdTracks[createdTracks.Count - 1]
                                              .EndPoint.position - newTrack.BeginPoint.localPosition).z + 9.8f);

            newTrack.transform.position = nextPos;
            newTrack.transform.rotation = trackRotation;
        }
    }

    private TrackEntity GetRandomTrack()
    {
        return createdTracks[Random.Range(0, createdTracks.Count)];
    }

}

[System.Serializable]
public class TrackPool
{
    public TrackEntity Track;
    public int Quantity;
}