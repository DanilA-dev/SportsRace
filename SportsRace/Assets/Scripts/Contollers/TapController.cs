using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TapController : MonoBehaviour
{
    [SerializeField] private float punchDuration;
    [SerializeField] private float punchStrenght;
    [SerializeField] private PlayerRunner player;
    [SerializeField] private Button tapButton;

    private void OnEnable()
    {
        tapButton.onClick.AddListener(OnTap);
        player.OnTrackEventEnter += OnTrackEventEnter;
        player.OnTrackEventExit += OnTrackEventExit;
    }

    private void OnDestroy()
    {
        player.OnTrackEventEnter -= OnTrackEventEnter;
        player.OnTrackEventExit -= OnTrackEventExit;
    }

    private void OnTap()
    {
        player.ActivateTapIvent();
        tapButton.transform.DOShakeScale(punchDuration, punchStrenght);
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
