using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapController : MonoBehaviour
{
    [SerializeField] private PlayerRunner player;
    [SerializeField] private Button tapButton;

    private void OnEnable()
    {
        tapButton.onClick.AddListener(() => player.ActivateTapIvent());
        player.OnTrackEventEnter += OnTrackEventEnter;
        player.OnTrackEventExit += OnTrackEventExit;
    }

    private void OnDestroy()
    {
        player.OnTrackEventEnter -= OnTrackEventEnter;
        player.OnTrackEventExit -= OnTrackEventExit;
    }

    private void OnTrackEventEnter()
    {
        tapButton.gameObject.SetActive(true);
    }
    private void OnTrackEventExit()
    {
        tapButton.gameObject.SetActive(false);
    }
}
