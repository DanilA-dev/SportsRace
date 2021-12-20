using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButtonInitializer : MonoBehaviour
{
    [SerializeField] private PlayerRunner player;
    [SerializeField] private SwitchButtonData buttonData;
    [SerializeField] private List<SwitchRunnerButton> switches = new List<SwitchRunnerButton>();

    public List<SwitchRunnerButton> Switches => switches;

    private bool _isAllButtonsDisabled;

    public void OnEnable()
    {
        foreach (var b in switches)
        {
            b.SwitchButton.enabled = true;
            b.SelectBorder.gameObject.SetActive(false);
        }
    }

    public void SetSwtichButtons()
    {
        var t = TracksController.Instance.LevelTracks.ToList();

        for (int i = 0; i < switches.Count; i++)
            switches[i].Init(player, t[i].TrackType, this, buttonData);
    }

    public void DisableSwitchButtons(float time)
    {
        if(!_isAllButtonsDisabled)
            StartCoroutine(ButtonsDisable(time));
    }

    private IEnumerator ButtonsDisable(float time)
    {
        _isAllButtonsDisabled = true;

        foreach (var b in switches)
            b.SwitchButton.enabled = false;

        yield return new WaitForSeconds(time);

        foreach (var b in switches)
            b.SwitchButton.enabled = true;

        _isAllButtonsDisabled = false;
    }
}
