using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RunnersSelectionController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform bot;

    [SerializeField] private List<RunnerObject> runnersPrefabs = new List<RunnerObject>();

    private List<RunnerObject> _generatedRunners = new List<RunnerObject>();
    private List<RunnerObject> _createdRunners = new List<RunnerObject>();

    public List<RunnerObject> CreatedRunners => _createdRunners;

    public void SetCreatedRunners()
    {
        GetTrackRunners();
        CreateRunners(player);
        CreateRunners(bot); 
    }

    public void GetTrackRunners()
    {
        var generatedTracks = TracksController.Instance.LevelTracks.ToList();

        for (int i = 0; i < runnersPrefabs.Count; i++)
        {
            foreach (var track in generatedTracks)
            {
                if (runnersPrefabs[i].Type == track.TrackType)
                    _generatedRunners.Add(runnersPrefabs[i]);
            }
        }
    }

    public void CreateRunners(Transform tParent)
    {
        for (int i = 0; i < runnersPrefabs.Count; i++)
        {
            var r = Instantiate(runnersPrefabs[i], tParent);
            r.transform.localPosition = Vector3.zero;
            r.gameObject.SetActive(false);
        }
    }

    public void ClearCreatedRunners()
    {
        _createdRunners.Clear();
        ClearRunner(player);
        ClearRunner(bot);
    }

    private void ClearRunner(Transform transform)
    {
        foreach (Transform t in transform)
        {
            if (t.TryGetComponent(out RunnerObject r))
                Destroy(r.gameObject);
        }

        foreach (var r in GameController.Instance.Runners)
            r.ClearRunners();
    }

}
