using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TracksController : MonoBehaviour
{
    public static TracksController Instance;

    [SerializeField] private float offset;
    [SerializeField] private int tracksAmount;
    [SerializeField] private TrackEntity startTrack;
    [SerializeField] private List<TrackEntity> tracksPrefab = new List<TrackEntity>();

    private HashSet<TrackEntity> levelTracks = new HashSet<TrackEntity>();
    private List<TrackEntity> createdLevelTrack = new List<TrackEntity>();
    public HashSet<TrackEntity> LevelTracks => levelTracks;


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
        createdLevelTrack.Add(startTrack);
        GetLevelTracks();
    }

    public void GetLevelTracks()
    {
        StartCoroutine(SetLevelTracks());
    }

    public void DeleteTracks()
    {
        levelTracks.Clear();
    }

    [ContextMenu("Create")]
    public void CreateTracksPrefabs()
    {
        for (int i = 0; i < tracksAmount; i++)
        {
            var createdTrack = Instantiate(levelTracks.ToList()[Random.Range(0, levelTracks.Count)]);

            var trackRotation = Quaternion.Euler(new Vector3(-90, 0, 90));
            var trackPos = new Vector3(0, 0, (createdLevelTrack[createdLevelTrack.Count - 1].EndPoint.position - createdTrack.BeginPoint.localPosition).z + offset);
            createdTrack.transform.rotation = trackRotation;
            createdTrack.transform.position = trackPos;

            createdLevelTrack.Add(createdTrack);
        }

        for (int i = 0; i < createdLevelTrack.Count; i++)
        {
            Debug.Log(createdLevelTrack[i]);
        }
    }

    private IEnumerator SetLevelTracks()
    {

        while(levelTracks.Count != 3)
        {
            levelTracks.Add(tracksPrefab[Random.Range(0, tracksPrefab.Count)]);
            yield return null;
        }

        for (int i = 0; i < levelTracks.Count; i++)
        {
            Debug.Log(levelTracks.ToList()[i] + "is in Level Tracks!!!");
        }
    }

}
