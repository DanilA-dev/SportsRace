using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using DG.Tweening;

public class TracksController : MonoBehaviour
{
    public static TracksController Instance;

    [SerializeField] private int tracksAmount;
    [SerializeField] private TrackEntity startTrack;
    [SerializeField] private TrackEntity finishPrefab;
    [SerializeField] private float offset;
    [SerializeField] private Vector3 finishOffset;
    [SerializeField] private List<TrackEntity> tracksPrefab = new List<TrackEntity>();
    [SerializeField] private List<TrackEntity> createdLevelTracks = new List<TrackEntity>();

    private List<Vector3> tracksPos = new List<Vector3>();

    private HashSet<TrackEntity> levelTracks = new HashSet<TrackEntity>();
    private List<TrackEntity> tracksChecker = new List<TrackEntity>();
    private List<int> trackIndexList = new List<int>();

    private int _lastIndexFromThree;
    private TrackEntity _finishTrack;

    public HashSet<TrackEntity> LevelTracks => levelTracks;
    public List<TrackEntity> CreatedTracks => createdLevelTracks;



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
           var nextPos = new Vector3(createdTrack.PosOffset.x, createdTrack.PosOffset.y, (createdLevelTracks[createdLevelTracks.Count - 1]
                                             .EndPoint.position - createdTrack.BeginPoint.localPosition).z + offset);
           createdTrack.transform.rotation = trackRotation;
           createdTrack.transform.position = nextPos;
          
           createdLevelTracks.Add(createdTrack);
          
           if (trackIndexList.Count == levelTracks.Count)
           {
               _lastIndexFromThree = trackIndexList[trackIndexList.Count - 1];
               trackIndexList.Clear();
           }
        }

        _finishTrack = Instantiate(finishPrefab);
        _finishTrack.transform.rotation = Quaternion.Euler(Vector3.zero);
        _finishTrack.transform.position = new Vector3(0 + finishOffset.x, 0 + finishOffset.y, (createdLevelTracks[createdLevelTracks.Count - 1]
                                              .EndPoint.position - finishPrefab.BeginPoint.localPosition).z + finishOffset.z);

        var finishCup = _finishTrack.GetComponentInChildren<FinishCup>();
        finishCup.SetNexCup(GameController.CupController.ShelfCups);
    }


    public void CheckFullTrack()
    {
        for (int j = 0; j < createdLevelTracks.Count - 1; j++)
        {
             if (createdLevelTracks[j].TrackType == createdLevelTracks[j + 1].TrackType)
                 GameController.CurrentState = GameState.Menu;
        }
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

        for (int i = 1; i < createdLevelTracks.Count; i++)
        {
            Destroy(createdLevelTracks[i].gameObject);
            yield return null;
        }
        createdLevelTracks.Clear();
        createdLevelTracks.Add(startTrack);
    }

    private IEnumerator TracksUnsubscribe()
    {
        for (int i = 0; i < createdLevelTracks.Count; i++)
        {
            createdLevelTracks[i].UnsubscribeFromEvents();
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


    public void UnSubFromTracks()
    {
        foreach (var t in createdLevelTracks)
        {
            t.UnsubscribeFromEvents();
        }
    }
}
