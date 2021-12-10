using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButtonInitializer : MonoBehaviour
{
    [SerializeField] private PlayerRunner player;
    [SerializeField] private List<SwitchRunnerButton> switches = new List<SwitchRunnerButton>();

    public void SetSwtichButtons()
    {
        var t = TracksController.Instance.LevelTracks.ToList();

        for (int i = 0; i < switches.Count; i++)
        {
            switches[i].Init(player, t[i].TrackType);
        }
    }
}
