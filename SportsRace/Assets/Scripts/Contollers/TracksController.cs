using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TracksController : MonoBehaviour
{
    public static TracksController Instance;

    [SerializeField] private int tracksAmount;
    [SerializeField] private List<TrackEntity> tracks = new List<TrackEntity>();

    private List<TrackEntity> useableTracks = new List<TrackEntity>();
    private HashSet<TrackEntity> generatedTracks = new HashSet<TrackEntity>();

    public HashSet<TrackEntity> GeneratedTracks => generatedTracks;

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);

        #endregion

    }

    private void OnEnable()
    {
        Generate();
    }

    private void Generate()
    {
        AddUseableTracks();
        GetTracks();
    }

    public void GetTracks()
    {
        for (int i = 0; i < tracksAmount; i++)
        {
            if (useableTracks.Count > 0)
            {
               //var randomIndex = Random.Range(0, useableTracks.Count);
               //var randomTrack = useableTracks[randomIndex];
                generatedTracks.Add(useableTracks[i]); // later do random
                Debug.Log($"Track : {useableTracks[i]} is generated");
            }
        }
        useableTracks.Clear();
    }

    private void AddUseableTracks()
    {
        for (int i = 0; i < tracks.Count; i++)
            useableTracks.Add(tracks[i]);
    }
}
