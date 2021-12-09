using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TracksController : MonoBehaviour
{
    public static TracksController Instance;

    [SerializeField] private bool allowCreate;
    [SerializeField] private int tracksAmount;
    [SerializeField] private TrackEntity startTrack;
    [SerializeField] private TrackEntity finishTrack;
    [SerializeField] private float offset;
    [SerializeField] private Vector3 finishOffset;
    [SerializeField] private List<TrackEntity> tracksPrefab = new List<TrackEntity>();

    private HashSet<TrackEntity> levelTracks = new HashSet<TrackEntity>();
    private List<TrackEntity> createdLevelTrack = new List<TrackEntity>();
    private List<int> trackIndexList = new List<int>();

    private int lastIndexFromThree;

    public HashSet<TrackEntity> LevelTracks => levelTracks;



    private void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance.gameObject);

        #endregion

        createdLevelTrack.Add(startTrack);
        GetLevelTracks();
    }

    private void Start()
    {
        if(allowCreate)
        Invoke("CreateTrack", 0.15f);
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
    public void CreateTrack()
    {
        for (int i = 0; i < tracksAmount; i++)
        {
            var createdTrack = Instantiate(levelTracks.ToList()[RandomGeneratedIndex()]);

            var trackRotation = Quaternion.Euler(new Vector3(-90, 0, 90));
            var nextPos = new Vector3(0, 0, (createdLevelTrack[createdLevelTrack.Count - 1]
                                              .EndPoint.position - createdTrack.BeginPoint.localPosition).z + offset);
            createdTrack.transform.rotation = trackRotation;
            createdTrack.transform.position = nextPos;
            createdLevelTrack.Add(createdTrack);

            if (trackIndexList.Count == levelTracks.Count)
            {
                lastIndexFromThree = trackIndexList[trackIndexList.Count - 1];
                trackIndexList.Clear();
            }
        }
        var finish = Instantiate(finishTrack);
        finish.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 90));
        finish.transform.position = new Vector3(0 + finishOffset.x, 0 + finishOffset.y, (createdLevelTrack[createdLevelTrack.Count - 1]
                                              .EndPoint.position - finishTrack.BeginPoint.localPosition).z + finishOffset.z);
    }

    private int RandomGeneratedIndex()
    {
        int num = Random.Range(0, levelTracks.Count);

        if(num == lastIndexFromThree)
            num = Random.Range(0, levelTracks.Count);

        if (trackIndexList.Contains(num))
            num = RandomGeneratedIndex();
        else
            trackIndexList.Add(num);

        return num;
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
