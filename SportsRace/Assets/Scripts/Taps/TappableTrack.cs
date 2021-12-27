using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TappableTrack : MonoBehaviour, ITrackTappable
{
    [SerializeField] protected SportType type;
    [SerializeField] private UnityEvent OnRunnerChange;


    private Collider _coll;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerRunner player))
        {
            player.OnTapEvent += OnTapActivate;
            player.OnRunnerChanged += Player_OnRunnerChanged;

            if (player.Type == type)
                player.OnTappableTrackEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerRunner player))
        {
            player.OnTappableTrackExit();
            player.SetCheckTracker(true);

            player.OnTapEvent -= OnTapActivate;
            player.OnRunnerChanged -= Player_OnRunnerChanged;
        }
    }

    protected virtual void Player_OnRunnerChanged(ARunner r)
    {
        var player = r.GetComponent<PlayerRunner>();
        player.OnTappableTrackExit();
        player.SetCheckTracker(true);

        player.OnTapEvent -= OnTapActivate;
        player.OnRunnerChanged -= Player_OnRunnerChanged;

        OnRunnerChange?.Invoke();
    }

    protected virtual void OnTapActivate(PlayerRunner player)
    {
        var tapSpeed = player.TapVariables.Where(t => t.Type == type).FirstOrDefault().SpeedPerTap;
        var maxSpeed = player.TapVariables.Where(t => t.Type == type).FirstOrDefault().MaxTapSpeed;

        if (player.Type == type)
        {
            player.SetCheckTracker(false);
            player.DefaultSpeed += tapSpeed;

            if (player.DefaultSpeed > maxSpeed)
                player.DefaultSpeed = maxSpeed;
        }
    }

    private IEnumerator ReEnableTrigger()
    {
        _coll.enabled = false;
        yield return new WaitForSeconds(0.1f);
        _coll.enabled = true;
    }
    public void ColliderReEnable()
    {
        StartCoroutine(ReEnableTrigger());
    }

}
