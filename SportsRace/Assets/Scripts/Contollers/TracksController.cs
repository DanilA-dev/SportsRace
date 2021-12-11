using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

public class TracksController : MonoBehaviour
{
    public static TracksController Instance;

    [SerializeField] private int tracksAmount;
    [SerializeField] private TrackEntity startTrack;
    [SerializeField] private TrackEntity finishPrefab;
    [SerializeField] private float offset;
    [SerializeField] private Vector3 finishOffset;
    [SerializeField] private List<TrackEntity> tracksPrefab = new List<TrackEntity>();
    [SerializeField] private List<TrackEntity> createdLevelTrack = new List<TrackEntity>();

    private HashSet<TrackEntity> levelTracks = new HashSet<TrackEntity>();
    private List<int> trackIndexList = new List<int>();

    private int _lastIndexFromThree;
    private TrackEntity _finishTrack;

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


    public IEnumerator CreateTrack()
    {
        yield return StartCoroutine(Clear());
        yield return StartCoroutine(SetLevelTracks());
        yield return new WaitForEndOfFrame();


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
                _lastIndexFromThree = trackIndexList[trackIndexList.Count - 1];
                trackIndexList.Clear();
            }
        }
        _finishTrack = Instantiate(finishPrefab);
        _finishTrack.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 90));
        _finishTrack.transform.position = new Vector3(0 + finishOffset.x, 0 + finishOffset.y, (createdLevelTrack[createdLevelTrack.Count - 1]
                                              .EndPoint.position - finishPrefab.BeginPoint.localPosition).z + finishOffset.z);
    }

    private int RandomGeneratedIndex()
    {
        int num = Random.Range(0, levelTracks.Count);

        if(num == _lastIndexFromThree)
            num = Random.Range(0, levelTracks.Count);

        if (trackIndexList.Contains(num))
            num = RandomGeneratedIndex();
        else
            trackIndexList.Add(num);

        return num;
    }

    private IEnumerator Clear()
    {
        levelTracks.Clear();
        trackIndexList.Clear();
        _lastIndexFromThree = 0;

        if(_finishTrack != null)
            Destroy(_finishTrack.gameObject);

        yield return StartCoroutine(TracksUnsubscribe());

        for (int i = 1; i < createdLevelTrack.Count; i++)
        {
            Destroy(createdLevelTrack[i].gameObject);
            yield return null;
        }
        createdLevelTrack.Clear();
        createdLevelTrack.Add(startTrack);
    }

    private IEnumerator TracksUnsubscribe()
    {
        for (int i = 0; i < createdLevelTrack.Count; i++)
        {
            createdLevelTrack[i].UnsubscribeFromEvents();
            yield return null;
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
            Debug.Log(levelTracks.ToList()[i] + "is in Level Tracks!!!");
    }

}
