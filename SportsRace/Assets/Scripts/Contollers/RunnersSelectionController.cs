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
        GenerateRunners();
        CreateRunners(player);
       // CreateRunners(bot); //CHECK LINE 49!
    }

    public void GenerateRunners()
    {
        var generatedTracks = TracksController.Instance.GeneratedTracks.ToList();

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
        for (int i = 0; i < _generatedRunners.Count; i++)
        {
            var r = Instantiate(_generatedRunners[i], tParent);
            r.transform.localPosition = Vector3.zero;
            r.gameObject.SetActive(false);
            _createdRunners.Add(r);
        }

        //takes the summary!!! of both player and bot!!!

       // var gameRunners = GameController.Instance.Runners;
       // foreach (var r in gameRunners)
       //     r.SetAvaliableRunnerList(_generatedRunners);
    }


    public void ClearCreatedRunners()
    {
        for (int i = 0; i < _createdRunners.Count; i++)
            Destroy(_createdRunners[i].gameObject);

        _createdRunners.Clear();

        var runners = GameController.Instance.Runners;
        foreach (var r in runners)
            r.ClearRunners();
    }

}
